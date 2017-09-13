using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.APIManagement;
using Microsoft.Deployment.Common.Model.Bpst;

namespace Microsoft.Deployment.Actions.AzureCustom.APIManagement
{
    [Export(typeof(IAction))]
    public class CreateAPIManagementLogger : BaseAction
    {
        private const int SIZE_PADDING = 5;

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            BpstAzure ba = new BpstAzure(request.DataStore);

            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure);

            string connectionString = request.DataStore.GetValue("EventHubPrimaryConnectionString");
            string idApimLogger = RandomGenerator.GetRandomHexadecimal(SIZE_PADDING, "bpst-apim-l-");
            string nameApimService = request.DataStore.GetValue("NameApimService");
            string nameNamespace = request.DataStore.GetValue("nameNamespace");

            request.DataStore.AddToDataStore("IdApimLogger", idApimLogger, DataStoreType.Private);

            APIManagementLogger logger = new APIManagementLogger(ba, nameApimService, idApimLogger, nameNamespace, connectionString);

            string url = $"https://management.azure.com/subscriptions/{ba.IdSubscription}/resourceGroups/{ba.NameResourceGroup}/providers/Microsoft.ApiManagement/service/{nameApimService}/loggers/{idApimLogger}?api-version=2017-03-01";

            bool isSuccess = await ahc.IsSuccess(HttpMethod.Put, url, JsonUtility.Serialize(logger));

            return isSuccess
                ? new ActionResponse(ActionStatus.Success)
                : new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("ApiManagementFailedToCreateLogger"));
        }
    }
}