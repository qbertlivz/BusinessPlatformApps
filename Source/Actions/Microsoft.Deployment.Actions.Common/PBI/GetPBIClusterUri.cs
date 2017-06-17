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
    public class GetPBIClusterUri : BaseAction
    {
        private const string PBI_CLUSTER_URIS_URL = "https://api.powerbi.com/spglobalservice/GetOrInsertClusterUrisByTenantlocation";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            AzureHttpClient client = new AzureHttpClient(request.DataStore.GetJson("PBIToken", "access_token"));

            PBIClusterUri pbiClusterUri = JsonUtility.Deserialize<PBIClusterUri>(await client.Request(HttpMethod.Put, PBI_CLUSTER_URIS_URL));

            request.DataStore.AddToDataStore("PBIClusterUri", pbiClusterUri.FixedClusterUri);

            return new ActionResponse(ActionStatus.Success);
        }
    }
}