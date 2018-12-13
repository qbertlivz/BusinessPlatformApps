using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Custom.Informatica
{
    [Export(typeof(IAction))]
    public class VerifyInformaticaCredentials : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string username = request.DataStore.GetValue("InformaticaUsername");
            string password = request.DataStore.GetValue("InformaticaPassword");
            RestClient rc = await InformaticaUtility.Initialize(username, password);
            await InformaticaUtility.Logout(rc, username, password);
            return new ActionResponse(ActionStatus.Success);
        }
    }
}