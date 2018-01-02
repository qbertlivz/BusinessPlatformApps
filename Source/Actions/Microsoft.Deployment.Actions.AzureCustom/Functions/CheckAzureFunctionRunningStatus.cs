using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.ErrorCode;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Azure;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class CheckAzureFunctionRunningStatus : BaseAction
    {
        public const int ATTEMPTS = 92;
        public const int STATUS = 4;
        public const int WAIT = 10000;

        public const string SUCCESS = "Lithium ETL Successful.";
        public const string ERROR = "Error occured in Lithium ETL.";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            var resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            var location = request.DataStore.GetJson("SelectedLocation", "Name");
            var sitename = request.DataStore.GetValue("FunctionName");

            await Task.Delay(60000);

            string storageAccountName = request.DataStore.GetValue("storageaccountname");
            string storageAccountKey = request.DataStore.GetValue("StorageAccountKey");            

            var accountCredentials = new StorageCredentials(storageAccountName, storageAccountKey);
            CloudStorageAccount account = new CloudStorageAccount(accountCredentials, false);
            CloudTableClient tableClient = account.CreateCloudTableClient();

            CloudTable functionLogsTable = tableClient.GetTableReference($"AzureWebJobsHostLogs" + DateTime.Now.ToString("yyyymm"));

            TableQuery query = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "I"));

            var queryResult = functionLogsTable.ExecuteQuery(query);

            foreach (var entity in queryResult.ToList())
            {
                if(entity.PartitionKey == "I" && entity["LogOutput"] != null )
                {
                    return entity["LogOutput"].ToString().Contains(SUCCESS) ? new ActionResponse(ActionStatus.Success) : new ActionResponse(ActionStatus.Failure);
                }
            }

            return new ActionResponse(ActionStatus.Failure);

            //AzureHttpClient client = new AzureHttpClient(azureToken, subscription, resourceGroup);

            //// Force sync
            //dynamic obj = new ExpandoObject();
            //obj.subscriptionId = subscription;
            //obj.SiteId = new ExpandoObject();
            //obj.SiteId.Name = sitename;
            //obj.SiteId.ResourceGroup = resourceGroup;
            //HttpResponseMessage result = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, "https://web1.appsvcux.ext.azure.com/websites/api/Websites/KuduSync", JsonUtility.GetJsonStringFromObject(obj));
            //var resultBody = await result.Content.ReadAsStringAsync();

            //return await IsReady(client, sitename) ? new ActionResponse(ActionStatus.Success) : new ActionResponse(ActionStatus.Failure);


            //AzureHttpClient client = new AzureHttpClient(azureToken, subscription, resourceGroup);
            //var responseMessage = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Post, $"providers/Microsoft.Web/sites/{sitename}/restart", "2016-08-01", "{}");

            ////if restart is successful, check the function running status.
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    await Task.Delay(60000);
            //    return new ActionResponse(ActionStatus.Success);
            //    //var functionResponse = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Get, $"providers/Microsoft.Web/sites/{sitename}/functions/LithiumETL", "2016-08-01", "{}");
            //}
            //else
            //{
            //    return new ActionResponse(ActionStatus.Failure);
            //}

            //var functions = JObject.Parse(await response.Content.ReadAsStringAsync());

            //while (IsRunning)
            //{
            //    var existingResponse = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Post, $"providers/Microsoft.Web/sites/{sitename}/functions/LithiumETL", "2016-08-01", "{}");

            //    if (!existingResponse.IsSuccessStatusCode)
            //    {
            //        return new ActionResponse(ActionStatus.Failure);
            //    }
            //    else
            //    {
            //        var appSettingsPayload = JObject.Parse(await existingResponse.Content.ReadAsStringAsync());

            //        //wait for 10 sec to check back the function running status
            //        await Task.Delay(10000);
            //        return new ActionResponse(ActionStatus.Success);
            //    }
            //}



        }

        //private async Task<bool> IsReady(AzureHttpClient client, string sitename)
        //{
        //    bool isReady = false;

        //    for (int i = 0; i < ATTEMPTS && !isReady; i++)
        //    {
        //        FunctionStatusLog statusWrapper = await client.RequestAzure<FunctionStatusLog>(HttpMethod.Get, $"/providers/Microsoft.Web/sites/{sitename}/logs", "2016-08-01");

        //        if (statusWrapper != null && !statusWrapper.Value.IsNullOrEmpty())
        //        {
        //            bool hasFinishedDeployment = true;

        //            for (int j = 0; j < statusWrapper.Value.Count && hasFinishedDeployment; j++)
        //            {
        //                FunctionStatus status = statusWrapper.Value[j];

        //                hasFinishedDeployment = status != null && status.Properties != null && !string.IsNullOrEmpty(status.Properties.LogUrl);

        //                if (hasFinishedDeployment)
        //                {
        //                    List<FunctionStatusLog> logs = await client.Request<List<FunctionStatusLog>>(HttpMethod.Get, status.Properties.LogUrl);

        //                    hasFinishedDeployment = !logs.IsNullOrEmpty();

        //                    if (hasFinishedDeployment)
        //                    {
        //                        bool isDeployed = false;

        //                        for (int k = 0; k < logs.Count && !isDeployed; k++)
        //                        {
        //                            isDeployed = logs[k].Message.Contains(SUCCESS);
        //                        }

        //                        hasFinishedDeployment = isDeployed && status.Properties.Active && status.Properties.Complete &&
        //                            status.Properties.EndTime != null && status.Properties.Status == STATUS;
        //                    }
        //                }
        //            }

        //            if (hasFinishedDeployment)
        //            {
        //                FunctionWrapper functionWrapper = await client.RequestAzure<FunctionWrapper>(HttpMethod.Get, $"/providers/Microsoft.Web/sites/{sitename}/functions", "2016-08-01");

        //                if (functionWrapper != null && !functionWrapper.Value.IsNullOrEmpty())
        //                {
        //                    bool areFunctionsReady = true;

        //                    for (int j = 0; j < functionWrapper.Value.Count && areFunctionsReady; j++)
        //                    {
        //                        Function function = functionWrapper.Value[j];

        //                        areFunctionsReady = function != null && function.Properties != null && function.Properties.Config != null && !function.Properties.Config.Disabled;
        //                    }

        //                    isReady = areFunctionsReady;
        //                }
        //            }
        //        }

        //        if (!isReady)
        //        {
        //            await Task.Delay(WAIT);
        //        }
        //    }

        //    return isReady;
        //}


    }

    public class AzureFunctionRunningStatusLog
    {

        public string DetailsUrl;

        public string Id;

        public DateTime? LogTime;

        public string Message;

        public int Type;
    }



}