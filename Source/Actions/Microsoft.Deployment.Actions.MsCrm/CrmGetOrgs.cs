namespace Microsoft.Deployment.Common.Actions.MsCrm
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Model;

    using Microsoft.Deployment.Common.ActionModel;
    using Microsoft.Deployment.Common.Actions;
    using Microsoft.Deployment.Common.Helpers;

    [Export(typeof(IAction))]
    public class CrmGetOrgs : BaseAction
    {

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string token = request.DataStore.GetJson("MsCrmToken", "access_token");

            AuthenticationHeaderValue bearer = new AuthenticationHeaderValue("Bearer", token);

            RestClient rc = new RestClient(MsCrmEndpoints.ENDPOINT, bearer);

            List<MsCrmOrganization> orgs = JsonUtility.Deserialize<List<MsCrmOrganization>>(await rc.Get(MsCrmEndpoints.URL_ORGANIZATIONS));

            return orgs.IsNullOrEmpty()
                ? new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("MsCrm_NoOrgs"))
                : new ActionResponse(ActionStatus.Success, JsonUtility.Serialize<List<MsCrmOrganization>>(orgs));
        }
    }
}