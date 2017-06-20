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
    public class StartInformaticaTask : BaseAction
    {
        private const string URL_JOB = "api/v2/job";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string username = request.DataStore.GetValue("InformaticaUsername");
            string password = request.DataStore.GetValue("InformaticaPassword");
            RestClient rc = await InformaticaUtility.Initialize(username, password);

            InformaticaJob job = new InformaticaJob
            {
                TaskId = await InformaticaUtility.GetTaskId(rc, InformaticaUtility.BPST_TASK_NAME),
                TaskName = InformaticaUtility.BPST_TASK_NAME,
                TaskType = "DRS"
            };
            await rc.Post(URL_JOB, JsonConvert.SerializeObject(job));

            await InformaticaUtility.Logout(rc, username, password);

            return new ActionResponse(ActionStatus.Success);
        }
    }
}