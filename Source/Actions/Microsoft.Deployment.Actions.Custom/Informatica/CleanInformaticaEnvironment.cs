using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Custom.Informatica
{
    [Export(typeof(IAction))]
    public class CleanInformaticaEnvironment : BaseAction
    {
        private const string URL_CONNECTION = "api/v2/connection/{0}";
        private const string URL_REPLICATION_TASK_ID = "api/v2/drstask/{0}";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string username = request.DataStore.GetValue("InformaticaUsername");
            string password = request.DataStore.GetValue("InformaticaPassword");
            RestClient rc = await InformaticaUtility.Initialize(username, password);

            await DeleteTask(rc, InformaticaUtility.BPST_TASK_NAME);
            await DeleteConnection(rc, InformaticaUtility.BPST_SOURCE_NAME);
            await DeleteConnection(rc, InformaticaUtility.BPST_TARGET_NAME);

            await InformaticaUtility.Logout(rc, username, password);

            return new ActionResponse(ActionStatus.Success);
        }

        private async Task DeleteConnection(RestClient rc, string connectionName)
        {
            string connectionId = await InformaticaUtility.GetConnectionId(rc, connectionName);
            if (connectionId != null) await rc.Delete(string.Format(CultureInfo.InvariantCulture, URL_CONNECTION, connectionId));
        }

        private async Task DeleteTask(RestClient rc, string taskName)
        {
            string taskId = await InformaticaUtility.GetTaskId(rc, taskName);
            if (taskId != null) await rc.Delete(string.Format(CultureInfo.InvariantCulture, URL_REPLICATION_TASK_ID, taskId));
        }
    }
}