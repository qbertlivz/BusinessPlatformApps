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
            apiVersion = "2015-10-01";
            var alias = request.DataStore.GetValue("StreamAnalyticsInputAlias");
            string primaryKey = request.DataStore.GetValue("primaryKey");
            // var body = $"{{\"properties\":{{\"dataSource\":{{\"inputIotHubSource\":{{}},\"inputEventHubSource\":{{\"eventHubName\":\"LancesEventHubName-twq5uxxrAq2aimv\",\"serviceBusNamespace\":\"{ehnamespace}\",\"sharedAccessPolicyName\":\"RootManageSharedAccessKey\",\"sharedAccessPolicyKey\":\"{primaryKey}\",\"sourcePartitionCount\":null,\"consumerGroupName\":null,\"consumerGroupNameDefaultValue\":null}},\"inputBlobSource\":{{}},\"inputIotGatewaySource\":{{}},\"inputReferenceFileSource\":{{}},\"type\":\"Microsoft.ServiceBus/EventHub\"}},\"serialization\":{{\"properties\":{{\"fieldDelimiter\":\",\",\"encoding\":\"UTF8\",\"format\":\"LineSeparated\"}},\"type\":\"Json\"}},\"type\":\"Stream\"}},\"createType\":\"None\",\"id\":null,\"location\":null,\"name\":\"eh-input\",\"type\":\"Microsoft.ServiceBus/EventHub\"}}";
            //string uri = $"https://main.streamanalytics.ext.azure.com/api/Inputs/PutInput?fullResourceId=%2Fsubscriptions%2F{subscription}%2FresourceGroups%2F{resourceGroup}%2Fproviders%2FMicrosoft.StreamAnalytics%2Fstreamingjobs%2FLancesStreamAnalyticsJob&subscriptionId={subscription}&resourceGroupName={resourceGroup}&jobName=LancesStreamAnalyticsJob&componentType=&componentName=";
            // var body = "{\"properties\":{\"dataSource\":{\"inputIotHubSource\":{},\"inputEventHubSource\":{\"eventHubName\":\"LancesEventHubName-twq5uxxrAq2aimv\",\"serviceBusNamespace\":\"LancesEventHubNameSpace-twq5uxxrAq2aimv\",\"sharedAccessPolicyName\":\"RootManageSharedAccessKey\",\"sharedAccessPolicyKey\":\"2XZ1zWfhr7G1W9yi3xAVU3xmYYsRMM3b6FRE4wLhw40=\",\"sourcePartitionCount\":null,\"consumerGroupName\":null,\"consumerGroupNameDefaultValue\":null},\"inputBlobSource\":{},\"inputIotGatewaySource\":{},\"inputReferenceFileSource\":{},\"type\":\"Microsoft.ServiceBus/EventHub\"},\"serialization\":{\"properties\":{\"fieldDelimiter\":\",\",\"encoding\":\"UTF8\",\"format\":\"LineSeparated\"},\"type\":\"Json\"},\"type\":\"Stream\"},\"createType\":\"None\",\"id\":null,\"location\":null,\"name\":\"input2\",\"type\":\"Microsoft.ServiceBus/EventHub\"}";
            //string uri = $"https://main.streamanalytics.ext.azure.com/api/Inputs/PutInput?fullResourceId=%2Fsubscriptions%2F657eb4a4-2e7c-485c-aee6-2816aef905c5%2FresourceGroups%2Fminint-cebcjkutest%2Fproviders%2FMicrosoft.StreamAnalytics%2Fstreamingjobs%2FLancesStreamAnalyticsJob&subscriptionId=657eb4a4-2e7c-485c-aee6-2816aef905c5&resourceGroupName=minint-cebcjkutest&jobName=LancesStreamAnalyticsJob&componentType=&componentName=";
            string uri = $"https://management.azure.com/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.StreamAnalytics/streamingjobs/LancesStreamAnalyticsJob/inputs/{alias}?api-version={apiVersion}";
            var lancestring = $"{{    \r\n   \"properties\":{{    \r\n      \"type\":\"stream\",  \r\n      \"serialization\":{{    \r\n         \"type\":\"Json\",  \r\n         \"properties\":{{    \r\n            \"fieldDelimiter\":\",\",  \r\n            \"encoding\":\"UTF8\"  \r\n         }}  \r\n      }},  \r\n      \"datasource\":{{    \r\n         \"type\":\"Microsoft.ServiceBus/EventHub\",  \r\n         \"properties\":{{    \r\n            \"serviceBusNamespace\":\"LancesEventHubNameSpace-twq5uxxrAq2aimv\",  \r\n            \"sharedAccessPolicyName\":\"RootManageSharedAccessKey\",  \r\n            \"sharedAccessPolicyKey\":\"{primaryKey}\",  \r\n            \"eventHubName\":\"LancesEventHubName-twq5uxxrAq2aimv\"  \r\n         }}  \r\n      }}  \r\n   }}  \r\n}}";


            var body = $"{{\"properties\":{{\"type\":\"stream\",\"serialization\":{{\"type\":\"Json\",\"properties\":{{\"fieldDelimiter\":\",\",\"encoding\":\"UTF8\"}},\"datasource\":{{\"type\":\"Microsoft.ServiceBus/EventHub\",\"properties\":{{\"serviceBusNamespace\":\"LancesEventHubNameSpace-twq5uxxrAq2aimv\",\"sharedAccessPolicyName\":\"RootManageSharedAccessKey\",\"sharedAccessPolicyKey\":\"{primaryKey}\",\"eventHubName\":\"LancesEventHubName-twq5uxxrAq2aimv\"}}}}}}}}}}";
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            bool isValidAlias = await VerifyInputAlias(alias, subscription, resourceGroup, ahc);
            HttpResponseMessage response;

            if (isValidAlias)
            {
                // API call to set entire input
                response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Put, uri, lancestring);
                if (response.IsSuccessStatusCode)
                {
                    return new ActionResponse(ActionStatus.Success);
                }
            }

            return new ActionResponse(ActionStatus.Failure);
        }
    }
}
