using Microsoft.Deployment.Common.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;

namespace Microsoft.Deployment.Actions.Custom.Facebook
{
    [Export(typeof(IAction))]
    public class GetPermanantPageToken : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
