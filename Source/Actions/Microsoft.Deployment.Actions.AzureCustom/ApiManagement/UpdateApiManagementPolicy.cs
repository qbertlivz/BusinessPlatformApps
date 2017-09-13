using System.ComponentModel.Composition;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.ApiManagement;
using Microsoft.Deployment.Common.Model.Bpst;

namespace Microsoft.Deployment.Actions.AzureCustom.APIManagement
{
    [Export(typeof(IAction))]
    public class UpdateApiManagementPolicy : BaseAction
    {
        private const int SIZE_PADDING = 5;

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            BpstAzure ba = new BpstAzure(request.DataStore);

            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure);

            string idApimLogger = request.DataStore.GetValue("IdApimLogger");
            string idApimService = request.DataStore.GetValue("IdApimService");

            string policyContent = File.ReadAllText(Path.Combine(request.Info.App.AppFilePath, "Service/Policy.txt"));
            policyContent.Replace("$(idLogger)", idApimLogger);

            ApiManagementPolicy policy = new ApiManagementPolicy(idApimService, policyContent);

            string url = $"https://management.azure.com{idApimService}/policies/policy?api-version=2017-03-01";

            string error = await ahc.Test(HttpMethod.Put, url, JsonUtility.Serialize(policy));

            return error == null
                ? new ActionResponse(ActionStatus.Success)
                : new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("ApiManagementFailedToUpdatePolicy"));
        }
    }
}