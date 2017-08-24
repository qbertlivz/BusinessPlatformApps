using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Common
{
    [Export(typeof(IAction))]
    public class EmailSubscription : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string azureToken = request.DataStore.GetJson("AzureToken", "access_token");

            request.Logger.LogEmailSubscription(request.DataStore.GetValue("EmailAddress"),
                JsonUtility.GetWebToken(azureToken, "given_name"),
                JsonUtility.GetWebToken(azureToken, "family_name"));

            return new ActionResponse(ActionStatus.Invisible);
        }
    }
}