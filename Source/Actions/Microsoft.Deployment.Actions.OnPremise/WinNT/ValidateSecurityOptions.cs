using System.ComponentModel.Composition;
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
    public class ValidateSecurityOptions : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {

            request.Logger.LogCustomProperty("OS version", Environment.OSVersion.Version.ToString());

            RegistryKey key = null;
            try
            {
                key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Lsa");
                if (key == null)
                    return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());

                object v = key.GetValue("disabledomaincreds"); // If exists, this is a DWORD. Unfortunately, GetValue ignores that in C we have "typedef unsigned long DWORD"
                if (v != null && (int)v != 0)
                {
                    request.Logger.LogCustomProperty("DISABLEDOMAINCREDS", v.ToString());
                    return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "DisabledDomainCredsEnabled");
                }
                return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
            }
            catch (Exception e)
            {
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), e, null);
            }
            finally
            {
                if (key != null)
                {
                    // The key was opened read only
                    key.Dispose();
                }
            }

            
        }
    }
}