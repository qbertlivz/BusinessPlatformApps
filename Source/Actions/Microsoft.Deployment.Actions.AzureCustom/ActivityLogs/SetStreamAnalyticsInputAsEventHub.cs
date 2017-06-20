using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.AzureCustom.ActivityLogs
{
    [Export(typeof(IAction))]
    public class SetStreamAnalyticsInputAsEventHub : BaseAction
    {
        public async Task<bool> VerifyInputAlias(string alias, string subscription, string resourceGroup, AzureHttpClient ahc)
        {
            // need to properly pass along streaming job name from create SA job action
            // -hardcoded for now
            var uri = $"https://main.streamanalytics.ext.azure.com/api/Jobs/IsEndpointNameAvailable?subscriptionId={subscription}&resourceGroupName={resourceGroup}&jobName=LancesStreamAnalyticsJob&endpointName={alias}";
            HttpResponseMessage response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, uri, "{}");
            string responseString =  await response.Content.ReadAsStringAsync();
            JObject responseObj = JsonUtility.GetJObjectFromStringValue(responseString);
            if (responseObj.GetValue("value").ToString() == "true")
            {
                return true;
            }
            return false;
        }

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var token = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            var resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            var ehnamespace = request.DataStore.GetValue("namespace");
            // var apiVersion = "2015-10-01";
            var alias = request.DataStore.GetValue("inputAlias");
            string primaryKey = request.DataStore.GetValue("primaryKey");
            string uri = $"https://management.azure.com/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.StreamAnalytics/streamingjobs/LancesStreamAnalyticsJob/inputs/{alias}?api-version=2015-10-01";
            var body = $"{{    \r\n   \"properties\":{{    \r\n      \"type\":\"stream\",  \r\n      \"serialization\":{{    \r\n         \"type\":\"Json\",  \r\n         \"properties\":{{    \r\n            \"fieldDelimiter\":\",\",  \r\n            \"encoding\":\"UTF8\"  \r\n         }}  \r\n      }},  \r\n      \"datasource\":{{    \r\n         \"type\":\"Microsoft.ServiceBus/EventHub\",  \r\n         \"properties\":{{    \r\n            \"serviceBusNamespace\":\"LancesEventHubNameSpace-twq5uxxrAq2aimv\",  \r\n            \"sharedAccessPolicyName\":\"RootManageSharedAccessKey\",  \r\n            \"sharedAccessPolicyKey\":\"{primaryKey}\",  \r\n            \"eventHubName\":\"LancesEventHubName-twq5uxxrAq2aimv\"  \r\n         }}  \r\n      }}  \r\n   }}  \r\n}}";
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            bool isValidAlias = await VerifyInputAlias(alias, subscription, resourceGroup, ahc);
            HttpResponseMessage response;

            if (isValidAlias)
            {
                // API call to set entire input
                response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Put, uri, body);
                if (response.IsSuccessStatusCode)
                {
                    return new ActionResponse(ActionStatus.Success);
                }
            }
            else
            {
                System.Console.WriteLine("Input alias unavailable");
            }

            return new ActionResponse(ActionStatus.Failure);
        }
    }
}
