using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Enums;
using Microsoft.Deployment.Common.ErrorCode;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.AzureCustom.Twitter
{
    [Export(typeof(IAction))]
    public class UpdateTwitterLogicApp : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {

            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            var resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            var location = request.DataStore.GetJson("SelectedLocation", "Name");

            var deploymentName = request.DataStore.GetValue("DeploymentName");
            var functionAppHostingPlan = request.DataStore.GetValue("functionAppHostingPlan");
            var sitename = request.DataStore.GetValue("SiteName");

            var search = request.DataStore.GetValue("SearchQuery");
            var logicAppName = request.DataStore.GetValue("LogicAppName");
            var requestUri = request.DataStore.GetValue("RequestUri");

            search = search.StartsWith("@") ? "@" + search : search;

            // Deploy logic app with updated trigger URL for last action

            var param = new AzureArmParameterGenerator();

            param.AddStringParam("logicAppUri", requestUri);
            param.AddStringParam("sitename", sitename);
            param.AddStringParam("resourcegroup", resourceGroup);
            param.AddStringParam("subscription", subscription);
            param.AddStringParam("search", search);
            param.AddStringParam("LogicAppName", logicAppName);

            var armParamTemplate = JsonUtility.GetJObjectFromObject(param.GetDynamicObject());
            armParamTemplate.Remove("parameters");
            armParamTemplate.Add("parameters", armParamTemplate["parameters"]);

            var helper = new DeploymentHelper();
            var deploymentResponse = await helper.DeployLogicApp(subscription, azureToken, resourceGroup, armParamTemplate, deploymentName);

            if (!deploymentResponse.IsSuccess)
            {
                return deploymentResponse;
            }

            //Log logic app
            request.Logger.LogResource(request.DataStore, logicAppName,
            DeployedResourceType.LogicApp, CreatedBy.BPST, DateTime.UtcNow.ToString("o"));

            return new ActionResponse(ActionStatus.Success);

        }
    }
}

