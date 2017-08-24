using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.EventHub;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class ValidateLogProfile : BaseAction
    {
        private const int ATTEMPTS = 42;
        private const int WAIT = 2500;

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string apiVersion = "2016-03-01";
            string subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            string token = request.DataStore.GetJson("AzureToken", "access_token");

            string uri = $"https://management.azure.com/subscriptions/{subscription}/providers/microsoft.insights/logprofiles/default?api-version={apiVersion}";

            AzureHttpClient ahc = new AzureHttpClient(token, subscription);

            LogProfile logProfile = await ahc.Request<LogProfile>(HttpMethod.Get, uri);

            return logProfile != null
                ? new ActionResponse(ActionStatus.Success) 
                : new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("ActivityLogsInsufficientPermissions"));
        }
    }
}