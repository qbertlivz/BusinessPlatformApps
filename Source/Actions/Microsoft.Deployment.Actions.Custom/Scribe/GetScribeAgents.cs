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
    public class GetScribeAgents : BaseAction
    {
        private const string URL_AGENTS = "/v1/orgs/{0}/agents";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string response = await rc.Get(string.Format(CultureInfo.InvariantCulture, URL_AGENTS, request.DataStore.GetValue("ScribeOrganizationId")));
            List<ScribeAgent> agents = JsonConvert.DeserializeObject<List<ScribeAgent>>(response);

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetJObjectFromStringValue(JsonConvert.SerializeObject(agents)));
        }
    }
}