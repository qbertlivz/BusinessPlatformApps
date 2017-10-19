using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Bpst;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class ExportActivityLogToEventHub : BaseAction
    {
        private const int ATTEMPTS = 42;
        private const int WAIT = 2500;

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            BpstAzure ba = new BpstAzure(request.DataStore);

            string nameNamespace = request.DataStore.GetValue("nameNamespace");

            string body = $"{{\"id\":null,\"location\":null,\"name\":null,\"properties\":{{\"categories\":[\"Write\",\"Delete\",\"Action\"],\"storageAccountId\":null,\"locations\":[\"australiaeast\",\"australiasoutheast\",\"brazilsouth\",\"canadacentral\",\"canadaeast\",\"centralindia\",\"centralus\",\"eastasia\",\"eastus\",\"eastus2\",\"japaneast\",\"japanwest\",\"koreacentral\",\"koreasouth\",\"northcentralus\",\"northeurope\",\"southcentralus\",\"southindia\",\"southeastasia\",\"uksouth\",\"ukwest\",\"westcentralus\",\"westeurope\",\"westindia\",\"westus\",\"westus2\",\"global\"],\"retentionPolicy\":{{\"enabled\":false,\"days\":0}},\"serviceBusRuleId\":\"/subscriptions/{ba.IdSubscription}/resourceGroups/{ba.NameResourceGroup}/providers/Microsoft.EventHub/namespaces/{nameNamespace}/authorizationrules/RootManageSharedAccessKey\"}},\"tags\":null}}";
            string uri = $"https://management.azure.com/subscriptions/{ba.IdSubscription}/providers/microsoft.insights/logprofiles/default?api-version=2016-03-01";

            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure, ba.IdSubscription);

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