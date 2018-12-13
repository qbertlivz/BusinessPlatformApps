using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;

namespace Microsoft.Deployment.Common.Helpers
{
    public class RequestUtility
    {
        public static async Task<ActionResponse> CallAction(ActionRequest request, string actionName)
        {
            ActionResponse response = null;

            if (!string.IsNullOrEmpty(actionName))
            {
                var action = request.ControllerModel.AppFactory.Actions[actionName];
                response = await action.ExecuteActionAsync(request);
            }

            return response;
        }
    }
}