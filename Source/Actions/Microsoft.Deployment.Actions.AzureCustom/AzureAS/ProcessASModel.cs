using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AnalysisServices;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using RefreshType = Microsoft.AnalysisServices.Tabular.RefreshType;
using Server = Microsoft.AnalysisServices.Tabular.Server;

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

            Server server = new Server();
            server.Connect(connectionString);

            // Process
            var db = server.Databases.Find(asDatabase);
            db.Model.RequestRefresh(RefreshType.Full);
            db.Model.SaveChanges();
            return new ActionResponse(ActionStatus.Success);
        }
    }
}