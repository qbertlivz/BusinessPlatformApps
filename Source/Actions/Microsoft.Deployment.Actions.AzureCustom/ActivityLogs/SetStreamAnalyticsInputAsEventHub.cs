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
            var apiVersion = request.DataStore.GetValue("apiVersion");
            var alias = request.DataStore.GetValue("StreamAnalyticsInputAlias");
            var body = $"{{\"properties\":{{\"dataSource\":{{\"inputIotHubSource\":{{}},\"inputEventHubSource\":{{\"eventHubName\":\"LancesEventHubName-twq5uxxrAq2aimv\",\"serviceBusNamespace\":\"LancesEventHubNamespace-twq5uxxrAq2aimv\",\"sharedAccessPolicyName\":\"RootManageSharedAccessKey\",\"sharedAccessPolicyKey\":\"EVIN5Ue24j5klcawtkNcbl3LKBpJqalZdkHM1go501Q=\",\"sourcePartitionCount\":null,\"consumerGroupName\":null,\"consumerGroupNameDefaultValue\":null}},\"inputBlobSource\":{{}},\"inputIotGatewaySource\":{{}},\"inputReferenceFileSource\":{{}},\"type\":\"Microsoft.ServiceBus/EventHub\"}},\"serialization\":{{\"properties\":{{\"fieldDelimiter\":\",\",\"encoding\":\"UTF8\",\"format\":\"LineSeparated\"}},\"type\":\"Json\"}},\"type\":\"Stream\"}},\"createType\":\"None\",\"id\":null,\"location\":null,\"name\":\"eh-input\",\"type\":\"Microsoft.ServiceBus/EventHub\"}}";
            string uri = $"https://management.azure.com/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.EventHub/namespaces/{ehnamespace}/AuthorizationRules/RootManageSharedAccessKey/listkeys?api-version={apiVersion}";

            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            bool isValidAlias = await VerifyInputAlias(alias, subscription, resourceGroup, ahc);
            if (isValidAlias)
            {
                // API call to set entire input
            }
            HttpResponseMessage response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, uri, "{}");
            if (response.IsSuccessStatusCode)
            {
                string keyString = await response.Content.ReadAsStringAsync();
                JObject keys = JsonUtility.GetJObjectFromJsonString(keyString);
                request.DataStore.AddToDataStore("policyKeys", keys);
            }
            return new ActionResponse(ActionStatus.Success);
        }
    }
}
