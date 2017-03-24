using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class GetScribeSolutionStatus : BaseAction
    {
        private const string URL_SOLUTION = "/v1/orgs/{0}/solutions/{1}";
        private const string URL_SOLUTION_PROCESS = "v1/orgs/{0}/solutions/{1}/start";
        private const string URL_SOLUTIONS = "/v1/orgs/{0}/solutions";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string orgId = request.DataStore.GetValue("ScribeOrganizationId");

            string response = await rc.Get(string.Format(CultureInfo.InvariantCulture, URL_SOLUTION, orgId, GetSolutionId(rc, orgId, ScribeUtility.BPST_SOLUTION_NAME));
            var result = JsonConvert.DeserializeObject<ScribeSolution>(response);

            string status = result.status ?? string.Empty;

            if (status.Equals("IdleLastRunFailed", StringComparison.OrdinalIgnoreCase) || status.Equals("OnDemandLastRunFailed", StringComparison.OrdinalIgnoreCase))
            {
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject());
            }
            else if (status.Equals("Idle", StringComparison.OrdinalIgnoreCase) || status.Equals("OnDemand", StringComparison.OrdinalIgnoreCase))
            {
                return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
            }
            else
            {
                return new ActionResponse(ActionStatus.BatchNoState, JsonUtility.GetEmptyJObject());
            }
        }

        private async Task<string> GetSolutionId(RestClient rc, string orgId, string name)
        {
            List<ScribeSolution> solutions = await GetSolutions(rc, orgId);

            foreach (ScribeSolution solution in solutions)
            {
                if (name == solution.Name)
                {
                    return solution.Id;
                }
            }

            return null;
        }

        private async Task<List<ScribeSolution>> GetSolutions(RestClient rc, string orgId)
        {
            string response = await rc.Get(string.Format(CultureInfo.InvariantCulture, URL_SOLUTIONS, orgId), null, null);
            return JsonConvert.DeserializeObject<List<ScribeSolution>>(response);
        }
    }
}