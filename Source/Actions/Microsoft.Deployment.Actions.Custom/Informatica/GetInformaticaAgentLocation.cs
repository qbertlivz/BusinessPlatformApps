using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Custom.Informatica
{
    [Export(typeof(IAction))]
    public class GetInformaticaAgentLocation : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            return new ActionResponse(ActionStatus.Success, JsonUtility.GetJObjectFromStringValue(JsonConvert.SerializeObject(InformaticaUtility.GetAgentDownloadLocation())));
        }
    }
}