using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.ActivityLog;
using Microsoft.Deployment.Common.Model.Bpst;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class ExportActivityLogToEventHub : BaseAction
    {
        private const string INSIGHTS_OPERATIONAL_LOGS = "insights-operational-logs";
        private const int ATTEMPTS = 42;
        private const int WAIT = 2500;

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            BpstAzure ba = new BpstAzure(request.DataStore);

            string nameNamespace = request.DataStore.GetValue("nameNamespace");

            ActivityLogProfile logProfile = new ActivityLogProfile(ba.IdSubscription, ba.NameResourceGroup, nameNamespace);

            string url = $"https://management.azure.com/subscriptions/{ba.IdSubscription}/providers/microsoft.insights/logprofiles/default?api-version=2016-03-01";

            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure, ba.IdSubscription);

            bool isSuccess = await ahc.IsSuccess(HttpMethod.Put, url, body);
            
            for (int i = 0; i < ATTEMPTS && !isSuccess; i++) {
                Thread.Sleep(WAIT);
                isSuccess = await ahc.IsSuccess(HttpMethod.Put, url, body);
            }
            
            string logProfileError = string.Empty;
            if (!isSuccess)
            {
                logProfileError = await ahc.Test(HttpMethod.Put, url, body);
            }

            request.DataStore.AddToDataStore("nameEventHub", INSIGHTS_OPERATIONAL_LOGS, DataStoreType.Private);

            return isSuccess
                ? new ActionResponse(ActionStatus.Success)
                : new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("ActivityLogsErrorExportingToEventHub", logProfileError));
        }
    }
}
