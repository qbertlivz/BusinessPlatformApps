using System;
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

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string orgId = request.DataStore.GetValue("ScribeOrganizationId");

            Thread.Sleep(SOLUTION_STATUS_WAIT);

            string solutionId = await ScribeUtility.GetSolutionId(rc, orgId, ScribeUtility.BPST_SOLUTION_NAME);

            ScribeSolution solution = JsonUtility.Deserialize<ScribeSolution>(await rc.Get(string.Format(ScribeUtility.URL_SOLUTION, orgId, solutionId)));

            string status = solution.status ?? string.Empty;

            if (status.Equals("IdleLastRunFailed", StringComparison.OrdinalIgnoreCase) || status.Equals("OnDemandLastRunFailed", StringComparison.OrdinalIgnoreCase))
            {
                return new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail(await GetHistory(rc, orgId, solutionId)));
            }
            else if (status.Equals("Idle", StringComparison.OrdinalIgnoreCase) || status.Equals("OnDemand", StringComparison.OrdinalIgnoreCase))
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

            if (history != null && history.Count > 0)
            {
                result = history[0].Details;
            }

            return result;
        }
    }
}