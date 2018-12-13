using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class CleanScribeEnvironment : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string orgId = request.DataStore.GetLastValue("ScribeOrganizationId");

            List<ScribeSolution> solutions = await ScribeUtility.GetSolutions(rc, orgId);
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

            return new ActionResponse(ActionStatus.Success);
        }

        private async Task DeleteConnection(RestClient rc, string orgId, string connectionId)
        {
            await rc.Delete(string.Format(ScribeUtility.URL_CONNECTION, orgId, connectionId), null, null);
        }

        private async Task DeleteSolution(RestClient rc, string orgId, string solutionId)
        {
            await rc.Delete(string.Format(ScribeUtility.URL_SOLUTION, orgId, solutionId));
        }

        private async Task<List<ScribeConnection>> GetConnections(RestClient rc, string orgId)
        {
            return JsonUtility.Deserialize<List<ScribeConnection>>(await rc.Get(string.Format(ScribeUtility.URL_CONNECTIONS, orgId)));
        }
    }
}