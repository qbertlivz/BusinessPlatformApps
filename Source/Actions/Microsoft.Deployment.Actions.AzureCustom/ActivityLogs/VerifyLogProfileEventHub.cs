using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.EventHub;

namespace Microsoft.Deployment.Actions.AzureCustom.ActivityLogs
{
    [Export(typeof(IAction))]
    public class VerifyLogProfileEventHub : BaseAction
    {
        private const string INSIGHTS = "insights-operational-logs";
        private const int RETRIES = 20;
        private const int SLEEP = 10000;
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string azure_access_token = request.DataStore.GetJson("AzureToken", "access_token");
            string subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            string resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            string apiVersion = "2015-08-01";
            string ehnamespace = request.DataStore.GetValue("ActivityLogNamespace");
            string uri = $"https://management.azure.com/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.EventHub/namespaces/{ehnamespace}/eventhubs?api-version={apiVersion}";
            string body = $"\"parameters\": {{\"namespaceName\":\"{ehnamespace}\",\"resourceGroupName\":\"{resourceGroup}\",\"api-version\":\"2015-08-01\", \"subscriptionId\": \"{subscription}\"}}";
            AzureHttpClient ahc = new AzureHttpClient(azure_access_token, subscription);
            bool areHubsPresent = false;
            for (int i = 0; i < RETRIES && !areHubsPresent; i++)
            {
                if (!areHubsPresent)
                {
                    Thread.Sleep(SLEEP);
                }
                List<EventHub> hubs = await ahc.RequestValue<List<EventHub>>(HttpMethod.Get, uri, body);

                if (!hubs.IsNullOrEmpty())
                {
                    foreach(EventHub hub in hubs)
                    {
                        if (hub.Name.EqualsIgnoreCase(INSIGHTS))
                        {
                            areHubsPresent = true;
                            break;
                        }
                    }
                }
            }
            return areHubsPresent ? new ActionResponse(ActionStatus.Success) : new ActionResponse(ActionStatus.Failure);
        }
    }
}