using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class ProcessScribeSolution : BaseAction
    {
        private const int SOLUTION_STATUS_ATTEMPTS = 50;
        private const int SOLUTION_STATUS_WAIT = 5000;

        private const string SOLUTION_STATUS_PREPARING = "Preparing";
        private const string SOLUTION_STATUS_PROVISIONING = "Provisioning";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            Thread.Sleep(SOLUTION_STATUS_WAIT);

            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string orgId = request.DataStore.GetValue("ScribeOrganizationId");

            string solutionId = await ScribeUtility.GetSolutionId(rc, orgId, ScribeUtility.BPST_SOLUTION_NAME);

            for (int i = 0; i < SOLUTION_STATUS_ATTEMPTS && !(await IsSolutionReady(rc, orgId, solutionId)); i++)
            {
                Thread.Sleep(SOLUTION_STATUS_WAIT);
            }

            await rc.Post(string.Format(ScribeUtility.URL_SOLUTION_PROCESS, orgId, solutionId), string.Empty);

            return new ActionResponse(ActionStatus.Success);
        }

        private async Task<bool> IsSolutionReady(RestClient rc, string orgId, string solutionId)
        {
            ScribeSolution solution = JsonUtility.Deserialize<ScribeSolution>(await rc.Get(string.Format(ScribeUtility.URL_SOLUTION, orgId, solutionId)));
            string status = solution.status ?? string.Empty;
            return !status.EqualsIgnoreCase(SOLUTION_STATUS_PREPARING) && !status.EqualsIgnoreCase(SOLUTION_STATUS_PROVISIONING);
        }
    }
}