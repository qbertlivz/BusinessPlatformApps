using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class ExportActivityLogToEventHub : BaseAction
    {
        private const int ATTEMPTS = 42;
        private const int WAIT = 2500;

        // Exports an Activity Log for the given subscription to Event Hub
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string apiVersion = "2016-03-01";
            string ehnamespace = request.DataStore.GetValue("ActivityLogNamespace");
            string resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            string subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            string token = request.DataStore.GetJson("AzureToken", "access_token");

            string body = $"{{\"id\":null,\"location\":null,\"name\":null,\"properties\":{{\"categories\":[\"Write\",\"Delete\",\"Action\"],\"storageAccountId\":null,\"locations\":[\"australiaeast\",\"australiasoutheast\",\"brazilsouth\",\"canadacentral\",\"canadaeast\",\"centralindia\",\"centralus\",\"eastasia\",\"eastus\",\"eastus2\",\"japaneast\",\"japanwest\",\"koreacentral\",\"koreasouth\",\"northcentralus\",\"northeurope\",\"southcentralus\",\"southindia\",\"southeastasia\",\"uksouth\",\"ukwest\",\"westcentralus\",\"westeurope\",\"westindia\",\"westus\",\"westus2\",\"global\"],\"retentionPolicy\":{{\"enabled\":false,\"days\":0}},\"serviceBusRuleId\":\"/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.EventHub/namespaces/{ehnamespace}/authorizationrules/RootManageSharedAccessKey\"}},\"tags\":null}}";
            string uri = $"https://management.azure.com/subscriptions/{subscription}/providers/microsoft.insights/logprofiles/default?api-version={apiVersion}";

            AzureHttpClient ahc = new AzureHttpClient(token, subscription);

            bool isSuccess = await ahc.IsSuccess(HttpMethod.Put, uri, body);

            for (int i = 0; i < ATTEMPTS && !isSuccess; i++)
            {
                Thread.Sleep(WAIT);
                isSuccess = await ahc.IsSuccess(HttpMethod.Put, uri, body);
            }

            string logProfileError = string.Empty;
            if (!isSuccess)
            {
                logProfileError = await ahc.Test(HttpMethod.Put, uri);
            }

            return isSuccess
                ? new ActionResponse(ActionStatus.Success) 
                : new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("ActivityLogsErrorExportingToEventHub", logProfileError));
        }
    }
}