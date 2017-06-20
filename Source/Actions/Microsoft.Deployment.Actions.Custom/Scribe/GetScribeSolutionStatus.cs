using System;
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

            ScribeSolution solution = JsonUtility.Deserialize<ScribeSolution>(await rc.Get(string.Format(ScribeUtility.URL_SOLUTION, orgId, await ScribeUtility.GetSolutionId(rc, orgId, ScribeUtility.BPST_SOLUTION_NAME))));

            string status = solution.status ?? string.Empty;

            if (status.Equals("IdleLastRunFailed", StringComparison.OrdinalIgnoreCase) || status.Equals("OnDemandLastRunFailed", StringComparison.OrdinalIgnoreCase))
            {
                return new ActionResponse(ActionStatus.Failure);
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
    }
}