using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Bpst;
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
            BpstAzure ba = new BpstAzure(request.DataStore);

            string nameNamespace = request.DataStore.GetValue("nameNamespace");
            string uri = $"https://management.azure.com/subscriptions/{ba.IdSubscription}/resourceGroups/{ba.NameResourceGroup}/providers/Microsoft.EventHub/namespaces/{nameNamespace}/eventhubs?api-version=2015-08-01";
            string body = $"\"parameters\": {{\"namespaceName\":\"{nameNamespace}\",\"resourceGroupName\":\"{ba.NameResourceGroup}\",\"api-version\":\"2015-08-01\", \"subscriptionId\": \"{ba.IdSubscription}\"}}";
            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure, ba.IdSubscription);
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