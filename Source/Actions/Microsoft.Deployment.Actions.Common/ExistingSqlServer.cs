using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Enums;

namespace Microsoft.Deployment.Actions.Common
{
    [Export(typeof(IAction))]
    class ExistingSqlServer : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            request.Logger.LogResource(request.DataStore, request.DataStore.GetValue("Server"), DeployedResourceType.SqlServer, CreatedBy.User, DateTime.UtcNow.ToString("o"));
            request.Logger.LogResource(request.DataStore, request.DataStore.GetValue("Database"), DeployedResourceType.SqlServer, CreatedBy.User, DateTime.UtcNow.ToString("o"));

            return new ActionResponse(ActionStatus.Success);
        }
    }
}