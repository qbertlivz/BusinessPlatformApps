using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Informatica;

namespace Microsoft.Deployment.Actions.Custom.Informatica
{
    [Export(typeof(IAction))]
    public class CleanInformaticaEnvironment : BaseAction
    {
        private const string URL_CONNECTION = "api/v2/connection/{0}";
        private const string URL_CONNECTIONS = "api/v2/connection";
        private const string URL_REPLICATION_TASK_ID = "api/v2/drstask/{0}";
        private const string URL_TASKS = "api/v2/task?type=DRS";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = await InformaticaUtility.Initialize(request.DataStore.GetValue("InformaticaUsername"), request.DataStore.GetValue("InformaticaPassword"));

            await DeleteTask(rc, InformaticaUtility.BPST_SOLUTION_NAME);
            await DeleteConnection(rc, InformaticaUtility.BPST_SOURCE_NAME);
            await DeleteConnection(rc, InformaticaUtility.BPST_TARGET_NAME);

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
        }

        private async Task DeleteConnection(RestClient rc, string connectionName)
        {
            string response = await rc.Get(URL_CONNECTIONS);
            InformaticaConnection[] connections = JsonConvert.DeserializeObject<InformaticaConnection[]>(response);

            if (connections == null) return;

            string connectionId = null;
            for (int i = 0; i < connections.Length; i++)
            {
                if (!connections[i].Name.EqualsIgnoreCase(connectionName)) continue;
                connectionId = connections[i].Id;
                break;
            }

            if (connectionId != null) await rc.Delete(string.Format(CultureInfo.InvariantCulture, URL_CONNECTION, connectionId));
        }

        private async Task DeleteTask(RestClient rc, string taskName)
        {
            string response = await rc.Get(URL_TASKS);
            InformaticaTask[] tasks = JsonConvert.DeserializeObject<InformaticaTask[]>(response);

            if (tasks == null) return;

            string taskId = null;
            for (int i = 0; i < tasks.Length; i++)
            {
                if (!tasks[i].Name.EqualsIgnoreCase(taskName)) continue;
                taskId = tasks[i].Id;
                break;
            }

            if (taskId != null) await rc.Delete(string.Format(CultureInfo.InvariantCulture, URL_REPLICATION_TASK_ID, taskId));
        }
    }
}