using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Scribe;
using System.Linq;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class CreateScribeSolution : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string orgId = request.DataStore.GetValue("ScribeOrganizationId");

            var sfObjects = request.DataStore.GetValue("Entities").SplitByCommaSpaceTabReturnList();
            var additionalObjects = request.DataStore.GetValue("AdditionalObjects");

            if (!string.IsNullOrEmpty(additionalObjects))
            {
                var add = additionalObjects.Split(',').ToList();
                sfObjects.AddRange(add);
            }

            ScribeSolution solution = new ScribeSolution
            {
                Name = ScribeUtility.BPST_SOLUTION_NAME,
                Description = string.Empty,
                SolutionType = "Replication",
                ReplicationSettings = new ScribeReplicationSettings(sfObjects.ToArray()),
                ConnectionIdForSource = await GetConnectionId(rc, orgId, ScribeUtility.BPST_SOURCE_NAME),
                ConnectionIdForTarget = await GetConnectionId(rc, orgId, ScribeUtility.BPST_TARGET_NAME),
                AgentId = await GetAgentId(rc, orgId, request.DataStore.GetValue("ScribeAgentName"))
            };

            ScribeSolution result = JsonUtility.Deserialize<ScribeSolution>(await rc.Post(string.Format(ScribeUtility.URL_SOLUTIONS, orgId), JsonUtility.Serialize(solution)));

            ScribeSolutionSchedule schedule = new ScribeSolutionSchedule(request.DataStore.GetValue("RefreshSchedule"));
            await rc.Put(string.Format(ScribeUtility.URL_SOLUTION_SCHEDULE, orgId, result.Id), JsonUtility.Serialize(schedule));

            return new ActionResponse(ActionStatus.Success);
        }

        private async Task<string> GetAgentId(RestClient rc, string orgId, string agentName)
        {
            List<ScribeAgent> agents = JsonUtility.Deserialize<List<ScribeAgent>>(await rc.Get(string.Format(ScribeUtility.URL_AGENTS, orgId)));

            foreach (ScribeAgent agent in agents)
            {
                if (agent.Name.EqualsIgnoreCase(agentName))
                {
                    return agent.Id;
                }
            }

            return null;
        }

        private async Task<string> GetConnectionId(RestClient rc, string orgId, string connectionName)
        {
            List<ScribeConnection> connection = JsonUtility.Deserialize<List<ScribeConnection>>(await rc.Get(string.Format(ScribeUtility.URL_CONNECTIONS, orgId),
                string.Format("name={0}&limit=1&expand=false", connectionName), null));
            return connection.Count == 1 ? connection[0].Id : null;
        }
    }
}