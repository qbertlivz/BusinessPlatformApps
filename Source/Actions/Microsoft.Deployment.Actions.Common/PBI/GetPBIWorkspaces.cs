using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.PBI;

namespace Microsoft.Deployment.Actions.Common.PBI
{
    [Export(typeof(IAction))]
    public class GetPBIWorkspaces : BaseAction
    {
        private const string PBI_ENDPOINT_GROUPS = "/v1.0/myorg/groups";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            AzureHttpClient client = new AzureHttpClient(request.DataStore.GetJson("PBIToken", "access_token"));
            string pbiClusterUri = request.DataStore.GetValue("PBIClusterUri");

            PBIWorkspaces pbiWorkspaces = JsonUtility.Deserialize<PBIWorkspaces>(await client.Request(HttpMethod.Get, pbiClusterUri + PBI_ENDPOINT_GROUPS));

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetJObjectFromObject(pbiWorkspaces));
        }
    }
}