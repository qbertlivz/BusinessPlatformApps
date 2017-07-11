using System;
using System.ComponentModel.Composition;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.Win32;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.OnPremise.WinNT
{
    // Should not run impersonated
    [Export(typeof(IAction))]
    public class ValidateAdminPrivileges : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string osVersion = NTHelper.OsVersion;
            string productName = null;
            string installationType = null;

            using (RegistryKey k = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion"))
            {
                productName = (string)k.GetValue("ProductName");
                installationType = (string)k.GetValue("InstallationType");
            }

            request.Logger.LogEvent("OSVersion", new System.Collections.Generic.Dictionary<string, string> { { nameof(osVersion), osVersion },
                                                                                                             { nameof(productName), productName },
                                                                                                             { nameof(installationType), installationType }
                                                                                                           });

            WindowsPrincipal current = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return current.IsInRole(WindowsBuiltInRole.Administrator)
                ? new ActionResponse(ActionStatus.Success)
                : new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "NotAdmin");
        }
    }
}