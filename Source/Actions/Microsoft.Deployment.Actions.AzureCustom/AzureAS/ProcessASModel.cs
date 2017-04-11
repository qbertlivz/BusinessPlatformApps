using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.AnalysisServices.Tabular;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureAS
{
    [Export(typeof(IAction))]
    public class ProcessASModel : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string connectionString = request.DataStore.GetValue("ASConnectionString");
            AzureUtility.GetEmailFromToken(request.DataStore.GetJson("AzureToken"));
            string asDatabase = request.DataStore.GetValue("ASDatabase");

            using (Server server = new Server())
            {
                server.Connect(connectionString);

                // Process
                Database db = server.Databases.FindByName(asDatabase);
                db.Model.RequestRefresh(RefreshType.Full);

                server.Disconnect(true);
                return new ActionResponse(ActionStatus.Success);
            }
        }
    }
}