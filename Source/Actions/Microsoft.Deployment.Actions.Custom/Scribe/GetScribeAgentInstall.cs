using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class GetScribeAgentInstall : BaseAction
    {
        private const string URL_PROVISION_ONPREMISE_AGENT = "/v1/orgs/{0}/agents/provision_onpremise_agent";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));
            string response = await rc.Post(string.Format(CultureInfo.InvariantCulture, URL_PROVISION_ONPREMISE_AGENT, request.DataStore.GetValue("ScribeOrganizationId")), string.Empty);
            return new ActionResponse(ActionStatus.Success, JsonUtility.GetJObjectFromStringValue(JsonConvert.SerializeObject(response)));
        }
    }
}