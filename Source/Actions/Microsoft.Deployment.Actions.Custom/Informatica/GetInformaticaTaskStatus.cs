using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Informatica;

namespace Microsoft.Deployment.Actions.Custom.Informatica
{
    [Export(typeof(IAction))]
    public class GetInformaticaTaskStatus : BaseAction
    {
        private const string URL_ACTIVITY = "api/v2/activity/activityLog";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string username = request.DataStore.GetValue("InformaticaUsername");
            string password = request.DataStore.GetValue("InformaticaPassword");
            RestClient rc = await InformaticaUtility.Initialize(username, password);

            string response = await rc.Get(URL_ACTIVITY);
            List<InformaticaActivityLog> tasks = JsonConvert.DeserializeObject<List<InformaticaActivityLog>>(response);
            string taskId = await InformaticaUtility.GetTaskId(rc, InformaticaUtility.BPST_TASK_NAME);

            ActionResponse result = null;
            for (var i = 0; i < tasks.Count && result == null; i++)
            {
                if (tasks[i].TaskId.EqualsIgnoreCase(taskId))
                {
                    if (tasks[i].State.EqualsIgnoreCase("1") || tasks[i].State.EqualsIgnoreCase("2"))
                    {
                        result = new ActionResponse(ActionStatus.Success);
                    }
                    else if (tasks[i].ExecutionState.EqualsIgnoreCase("3"))
                    {
                        result = new ActionResponse(ActionStatus.Failure, JsonUtility.GetJObjectFromStringValue(tasks[i].ErrorMsg));
                    }
                    else
                    {
                        result = new ActionResponse(ActionStatus.InProgress);
                    }
                }
            }

            if (result == null) result = new ActionResponse(ActionStatus.InProgress);

            await InformaticaUtility.Logout(rc, username, password);

            return result;
        }
    }
}