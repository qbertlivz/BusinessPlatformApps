using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class GetScribeAgents : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            return new ActionResponse(ActionStatus.Success, await rc.Get(string.Format(ScribeUtility.URL_AGENTS, request.DataStore.GetValue("ScribeOrganizationId"))));
        }
    }
}