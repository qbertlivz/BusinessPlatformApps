﻿using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;

namespace Microsoft.Deployment.Actions.Common.Test
{
    [Export(typeof(IAction))]
    public class Test : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            return new ActionResponse(ActionStatus.Success, "Test");
        }
    }
}