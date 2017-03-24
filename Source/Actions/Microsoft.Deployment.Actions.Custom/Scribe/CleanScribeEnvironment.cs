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
    public class CleanScribeEnvironment : BaseAction
    {
        private const string URL_CONNECTION = "/v1/orgs/{0}/connections/{1}";
        private const string URL_CONNECTIONS = "/v1/orgs/{0}/connections";
        private const string URL_SOLUTION = "/v1/orgs/{0}/solutions/{1}";
        private const string URL_SOLUTIONS = "/v1/orgs/{0}/solutions";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetLastValue("ScribeUsername"), request.DataStore.GetLastValue("ScribePassword"));

            string orgId = request.DataStore.GetLastValue("ScribeOrganizationId");

            List<ScribeSolution> solutions = await GetSolutions(rc, orgId);
            if (solutions != null)
            {
                foreach (ScribeSolution solution in solutions)
                {
                    if (solution.Name.Contains(ScribeUtility.BPST_SOLUTION_NAME))
                    {
                        await DeleteSolution(rc, orgId, solution.Id);
                    }
                }
            }

            List<ScribeConnection> connections = await GetConnections(rc, orgId);
            if (connections != null)
            {
                foreach (ScribeConnection connection in connections)
                {
                    if (connection.Name.Contains(ScribeUtility.BPST_SOURCE_NAME) || connection.Name.Contains(ScribeUtility.BPST_TARGET_NAME))
                    {
                        await DeleteConnection(rc, orgId, connection.Id);
                    }
                }
            }

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
        }

        private async Task DeleteConnection(RestClient rc, string orgId, string connectionId)
        {
            await rc.Delete(string.Format(CultureInfo.InvariantCulture, URL_CONNECTION, orgId, connectionId), null, null);
        }

        private async Task DeleteSolution(RestClient rc, string orgId, string solutionId)
        {
            await rc.Delete(string.Format(CultureInfo.InvariantCulture, URL_SOLUTION, orgId, solutionId));
        }

        private async Task<List<ScribeConnection>> GetConnections(RestClient rc, string orgId)
        {
            string response = await rc.Get(string.Format(CultureInfo.InvariantCulture, URL_CONNECTIONS, orgId));
            return JsonConvert.DeserializeObject<List<ScribeConnection>>(response);
        }

        private async Task<List<ScribeSolution>> GetSolutions(RestClient rc, string orgId)
        {
            string response = await rc.Get(string.Format(CultureInfo.InvariantCulture, URL_SOLUTIONS, orgId), null, null);
            return JsonConvert.DeserializeObject<List<ScribeSolution>>(response);
        }
    }
}