using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.PBI;

namespace Microsoft.Deployment.Actions.Common.PBI
{
    [Export(typeof(IAction))]
    public class PublishPBIReport : BaseAction
    {
        private const string PBI_IMPORT_STATUS_URI = "beta/myorg/{0}imports/{1}";
        private const string PBI_IMPORT_URI = "beta/myorg/{0}imports/?datasetDisplayName={1}&nameConflict=Abort";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            AzureHttpClient client = new AzureHttpClient(request.DataStore.GetJson("PBIToken", "access_token"));
            string pbiClusterUri = request.DataStore.GetValue("PBIClusterUri");
            string pbiWorkspaceId = request.DataStore.GetValue("PBIWorkspaceId");
            string pbixLocation = request.DataStore.GetValue("PBIXLocation");

            pbiWorkspaceId = string.IsNullOrEmpty(pbiWorkspaceId) ? string.Empty : "groups/" + pbiWorkspaceId + "/";

            byte[] file = null;
            using (WebClient webClient = new WebClient())
            {
                file = webClient.DownloadData(pbixLocation);
            }

            string filename = request.Info.AppName + RandomGenerator.GetDateStamp() + ".pbix";

            PBIImport pbiImport = JsonUtility.Deserialize<PBIImport>(await client.Request(pbiClusterUri + string.Format(PBI_IMPORT_URI, pbiWorkspaceId, filename), file, filename));

            return new ActionResponse(ActionStatus.Success, string.Empty);
        }
    }
}