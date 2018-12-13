using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Custom
{
    [Export(typeof(IAction))]
    public class RemoveFiles : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string targetPath = request.DataStore.GetValue("TargetPath") ?? FileUtility.GetLocalTemplatePath(request.Info.AppName);

            if (Directory.Exists(targetPath))
            {
                try
                {
                    RetryUtility.Retry(5, () =>
                    {
                        Directory.Delete(targetPath, true);
                        Thread.Sleep(500);
                    });
                }
                catch (DirectoryNotFoundException)
                {
                    // If the directory is not found return success. Either it's been deleted manually or customer installed in a different directory.
                }
                catch
                {
                    // Do nothing
                }
            }

            return new ActionResponse(ActionStatus.Success);
        }
    }
}