using System.Collections.Generic;
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
    public class GetScribeSolutionStatus : BaseAction
    {
        private const int SOLUTION_STATUS_WAIT = 5000;

        private const string SOLUTION_STATUS_IDLE = "Idle";
        private const string SOLUTION_STATUS_IDLE_LAST_RUN_FAILED = "IdleLastRunFailed";
        private const string SOLUTION_STATUS_ON_DEMAND = "OnDemand";
        private const string SOLUTION_STATUS_ON_DEMAND_LAST_RUN_FAILED = "OnDemandLastRunFailed";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string orgId = request.DataStore.GetValue("ScribeOrganizationId");

            Thread.Sleep(SOLUTION_STATUS_WAIT);

            string solutionId = await ScribeUtility.GetSolutionId(rc, orgId, ScribeUtility.BPST_SOLUTION_NAME);

            ScribeSolution solution = JsonUtility.Deserialize<ScribeSolution>(await rc.Get(string.Format(ScribeUtility.URL_SOLUTION, orgId, solutionId)));

            string status = solution.status ?? string.Empty;

            if (status.EqualsIgnoreCase(SOLUTION_STATUS_IDLE_LAST_RUN_FAILED) || status.EqualsIgnoreCase(SOLUTION_STATUS_ON_DEMAND_LAST_RUN_FAILED))
            {
                return new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail(string.Empty, await GetHistory(rc, orgId, solutionId)));
            }
            else if (status.EqualsIgnoreCase(SOLUTION_STATUS_IDLE) || status.EqualsIgnoreCase(SOLUTION_STATUS_ON_DEMAND))
            {
                return new ActionResponse(ActionStatus.Success);
            }
            else
            {
                return new ActionResponse(ActionStatus.InProgress);
            }
        }

        private async Task<string> GetHistory(RestClient rc, string orgId, string solutionId)
        {
            string result = string.Empty;

            List<ScribeHistory> history = JsonUtility.Deserialize<List<ScribeHistory>>(await rc.Get(string.Format(ScribeUtility.URL_HISTORY, orgId, solutionId)));

            if (!history.IsNullOrEmpty())
            {
                result = history[0].Details;
            }

            return result;
        }
    }
}