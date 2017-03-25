using System.ComponentModel.Composition;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Win32;
using System;

namespace Microsoft.Deployment.Actions.OnPremise.WinNT
{
    // Should not run impersonated
    [Export(typeof(IAction))]
    public class ValidateAdminPrivileges : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string osVersion = null;
            string productName = null;
            string installationType = null;

            try
            {
                osVersion = Environment.OSVersion.Version.ToString();

                using (RegistryKey k = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion"))
                {
                    object v = k.GetValue("CurrentMajorVersionNumber");
                    if (v != null)
                    {
                        osVersion = Convert.ToString(v);
                        v = k.GetValue("CurrentMinorVersionNumber");
                        if (v != null)
                        {
                            osVersion += '.' + Convert.ToString(v);
                            v = k.GetValue("CurrentBuildNumber") ?? k.GetValue("CurrentBuild");
                            if (v != null)
                            {
                                osVersion += '.' + Convert.ToString(v);
                            }
                        }
                    }

                    productName = (string)k.GetValue("ProductName");
                    installationType = (string)k.GetValue("InstallationType");
                }
            }
            catch 
            {
                // Do nothing, I could not read the OS version and will rely on what .Net says
            }

            request.Logger.LogEvent("OSVersion", new System.Collections.Generic.Dictionary<string, string> { { nameof(osVersion), osVersion },
                                                                                                             { nameof(productName), productName },
                                                                                                             { nameof(installationType), installationType }
                                                                                                           });

            WindowsPrincipal current = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return current.IsInRole(WindowsBuiltInRole.Administrator)
                ? new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject())
                : new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "NotAdmin");
        }
    }
}