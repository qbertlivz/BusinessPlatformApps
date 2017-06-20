using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

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
        private const string URL_SOLUTION = "/v1/orgs/{0}/solutions/{1}";
        private const string URL_SOLUTION_PROCESS = "v1/orgs/{0}/solutions/{1}/start";
        private const string URL_SOLUTIONS = "/v1/orgs/{0}/solutions";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            Thread.Sleep(SOLUTION_STATUS_WAIT);

            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string orgId = request.DataStore.GetValue("ScribeOrganizationId");

            string solutionId = await GetSolutionId(rc, orgId, ScribeUtility.BPST_SOLUTION_NAME);

            for (int i = 0; i < SOLUTION_STATUS_ATTEMPTS && !(await IsSolutionReady(rc, orgId, solutionId)); i++)
            {
                Thread.Sleep(SOLUTION_STATUS_WAIT);
            }

            await rc.Post(string.Format(CultureInfo.InvariantCulture, URL_SOLUTION_PROCESS, orgId, solutionId), string.Empty);

            return new ActionResponse(ActionStatus.Success);
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

        private async Task<bool> IsSolutionReady(RestClient rc, string orgId, string solutionId)
        {
            string response = await rc.Get(string.Format(CultureInfo.InvariantCulture, URL_SOLUTION, orgId, solutionId));
            var result = JsonConvert.DeserializeObject<ScribeSolution>(response);
            string status = result.status ?? string.Empty;
            return !status.EqualsIgnoreCase("Provisioning") && !status.EqualsIgnoreCase("Preparing");
        }
    }
}