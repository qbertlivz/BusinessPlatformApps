using Microsoft.Deployment.Common.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Azure;
using System.IO;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using System.Threading;
using Microsoft.Deployment.Common.ErrorCode;
using System.Net.Http;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Microsoft.Deployment.Common.Enums;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class DeployNotifierLogicApp : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            string subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            string resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");

            string deploymentName = request.DataStore.GetValue("DeploymentName");
            string logicAppTrigger = string.Empty;
            string logicAppName = request.DataStore.GetValue("logicAppName");
            string sqlConnectionName = request.DataStore.GetValue("sqlConnectionName");

            // Read from file
            var logicAppJsonLocation = "Service/Notifier/notifierLogicApp.json";

            if (deploymentName == null)
            {
                deploymentName = request.DataStore.CurrentRoute;
            }

            var param = new AzureArmParameterGenerator();
            param.AddStringParam("logicAppTrigger", string.Empty);
            param.AddStringParam("logicAppName", logicAppName);
            param.AddStringParam("sqlConnectionName", request.DataStore.GetValue("sqlConnectionName"));
            param.AddStringParam("resourceGroup", resourceGroup);
            param.AddStringParam("subscription", subscription);

            var armTemplate = JsonUtility.GetJObjectFromJsonString(System.IO.File.ReadAllText(Path.Combine(request.Info.App.AppFilePath, logicAppJsonLocation)));
            var armParamTemplate = JsonUtility.GetJObjectFromObject(param.GetDynamicObject());
            armTemplate.Remove("parameters");
            armTemplate.Add("parameters", armParamTemplate["parameters"]);

            //Deploy logic app 
            var deploymentResponse = await DeployLogicApp(subscription, azureToken, resourceGroup, armTemplate, deploymentName);

            if (!deploymentResponse.IsSuccess)
            {
                return deploymentResponse;
            }

            //Get logic app trigger Url
            AzureHttpClient azureClient = new AzureHttpClient(azureToken, subscription, resourceGroup);
            var response = await azureClient.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Post, $"providers/Microsoft.Logic/workflows/{logicAppName}/triggers/manual/listCallbackUrl", "2016-06-01", string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse(ActionStatus.Failure);
            }

            var postUrl = JsonUtility.GetJObjectFromJsonString(await response.Content.ReadAsStringAsync());

            // Deploy logic app with updated trigger URL for last action
            var newParam = new AzureArmParameterGenerator();

            newParam.AddStringParam("logicAppTrigger", postUrl["value"].ToString());
            newParam.AddStringParam("logicAppName", logicAppName);
            newParam.AddStringParam("sqlConnectionName", request.DataStore.GetValue("sqlConnectionName"));
            newParam.AddStringParam("resourceGroup", resourceGroup);
            newParam.AddStringParam("subscription", subscription);

            armParamTemplate = JsonUtility.GetJObjectFromObject(newParam.GetDynamicObject());
            armTemplate.Remove("parameters");
            armTemplate.Add("parameters", armParamTemplate["parameters"]);
            
            deploymentResponse = await DeployLogicApp(subscription, azureToken, resourceGroup, armTemplate, deploymentName);

            if (!deploymentResponse.IsSuccess)
            {
                return deploymentResponse;
            }
            
            request.DataStore.AddToDataStore("NotifierTriggerUrl", postUrl["value"].ToString());

            //Log logic app
            request.Logger.LogResource(request.DataStore, logicAppName,
                DeployedResourceType.LogicApp, CreatedBy.BPST, DateTime.UtcNow.ToString("o"));

            return new ActionResponse(ActionStatus.Success);
        }

        public async Task<ActionResponse> DeployLogicApp(string subscription, string azureToken, string resourceGroup, JObject armTemplate, string deploymentName)
        {
            SubscriptionCloudCredentials creds = new TokenCloudCredentials(subscription, azureToken);
            Microsoft.Azure.Management.Resources.ResourceManagementClient client = new ResourceManagementClient(creds);

            var deployment = new Microsoft.Azure.Management.Resources.Models.Deployment()
            {
                Properties = new DeploymentPropertiesExtended()
                {
                    Template = armTemplate.ToString(),
                    Parameters = JsonUtility.GetEmptyJObject().ToString()
                }
            };

            var validate = await client.Deployments.ValidateAsync(resourceGroup, deploymentName, deployment, new CancellationToken());

            if (!validate.IsValid)
                return new ActionResponse(
                    ActionStatus.Failure,
                    JsonUtility.GetJObjectFromObject(validate),
                    null,
                    DefaultErrorCodes.DefaultErrorCode,
                    $"Azure:{validate.Error.Message} Details:{validate.Error.Details}");

            var deploymentItem = await client.Deployments.CreateOrUpdateAsync(resourceGroup, deploymentName, deployment, new CancellationToken());

            var resp = await WaitForAction(client, resourceGroup, deploymentItem.Deployment.Name);

            if (!resp.IsSuccess)
            {
                return resp;
            }

            return await WaitForAction(client, resourceGroup, deploymentName);
        }

        private async Task<ActionResponse> WaitForAction(ResourceManagementClient client, string resourceGroup, string deploymentName)
        {
            while (true)
            {
                Thread.Sleep(5000);
                var status = await client.Deployments.GetAsync(resourceGroup, deploymentName, new CancellationToken());
                var operations =
                    await
                        client.DeploymentOperations.ListAsync(resourceGroup, deploymentName,
                            new DeploymentOperationsListParameters(), new CancellationToken());
                var provisioningState = status.Deployment.Properties.ProvisioningState;

                if (provisioningState == "Accepted" || provisioningState == "Running")
                    continue;

                if (provisioningState == "Succeeded")
                    return new ActionResponse(ActionStatus.Success, operations);

                var operation = operations.Operations.First(p => p.Properties.ProvisioningState == ProvisioningState.Failed);
                var operationFailed =
                    await
                        client.DeploymentOperations.GetAsync(resourceGroup, deploymentName, operation.OperationId,
                            new CancellationToken());

                return new ActionResponse(ActionStatus.Failure, operationFailed);
            }
        }
    }
}