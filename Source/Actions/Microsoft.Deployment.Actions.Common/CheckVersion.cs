using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;

namespace Microsoft.Deployment.Actions.Common
{
    [Export(typeof(IAction))]
    public class CheckVersion : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            bool upgrade = false;
            Version localVersion = Assembly.GetExecutingAssembly().GetName().Version;
            WebClient client = new WebClient();
            ActionResponse result = new ActionResponse(ActionStatus.Success);

            try
            {
                Version webVersion = new Version();
                Version.TryParse(client.DownloadString("https://bpstservice.azurewebsites.net/" + "api/version"), out webVersion);

                if (localVersion < webVersion)
                {
                    upgrade = true;
                    result = new ActionResponse(ActionStatus.Success, upgrade);
                }

                result = new ActionResponse(ActionStatus.Success, upgrade);
            }
            catch
            {
                result = new ActionResponse(ActionStatus.Success, upgrade);
            }

            return result;
        }
    }
}
