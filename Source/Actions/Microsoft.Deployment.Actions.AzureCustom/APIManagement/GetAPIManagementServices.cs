using System.Collections.Generic;
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
    public class GetAPIManagementServices : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            BpstAzure ba = new BpstAzure(request.DataStore);

            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure);

            string url = $"https://management.azure.com/subscriptions/{ba.IdSubscription}/providers/Microsoft.ApiManagement/service?api-version=2016-10-10";

            List<APIManagementService> apiManagementServices = await ahc.RequestValue<List<APIManagementService>>(HttpMethod.Get, url);

            return apiManagementServices.IsNullOrEmpty()
                ? new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("ApiManagementErrorNoServicesFound"))
                : new ActionResponse(ActionStatus.Success, JsonUtility.Serialize(apiManagementServices));
        }
    }
}