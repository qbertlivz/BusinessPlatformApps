using System;
using System.IO;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Win32.TaskScheduler;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

using Task = Microsoft.Win32.TaskScheduler.Task;
using System.IO.Compression;

namespace Microsoft.Deployment.Actions.OnPremise.TaskScheduler
{
    [Export(typeof(IAction))]
    public class GetTaskStatus : BaseAction
    {
        static String userLogDir = "C:\\ProgramData\\Business Platform Solution Templates\\Microsoft-SCCMTemplate\\Logs";
        static String exportLogDir = "C:\\ProgramData\\Business Platform Solution Templates\\Microsoft-SCCMTemplate\\Logs\\Export";


        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string taskName = request.DataStore.GetValue("TaskName");

            TaskCollection tasks = null;

            using (TaskService ts = new TaskService())
            {
                tasks = ts.RootFolder.GetTasks(new Regex(taskName));

                // We expect only one task to match
                foreach (Task task in tasks)
                {
                    switch (task.LastTaskResult)
                    {
                        case 0: return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
                        case 267014:
                            //zipLogs();
                            return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(),
                                                                new Exception("The scheduled task was terminated by the user."),
                                                                "TaskSchedulerRunFailed");
                        case 411:
                            return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(),
                                                      new Exception("PowerShell version too low - please upgrade to latest version https://msdn.microsoft.com/en-us/powershell/wmf/5.0/requirements"),
                                                      "TaskSchedulerRunFailed");
                    }

                    if (task.State == TaskState.Running)
                        return new ActionResponse(ActionStatus.BatchNoState, JsonUtility.GetEmptyJObject());

                    if (NTHelper.IsCredentialGuardEnabled())
                        return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "CredentialGuardEnabled");

                    //If we've encountered an error, copy the logs, zip them up, and send to blob
                    //zipLogs();

                    ActionResponse response = new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(),
                        new Exception($"Scheduled task exited with code {task.LastTaskResult}"), "TaskSchedulerRunFailed");
                    response.ExceptionDetail.LogLocation = FileUtility.GetLocalTemplatePath(request.Info.AppName);

                    return response;
                }
            }

            // We should never return this
            return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SccmTaskNotFound");
        }

        private void zipLogs()
        {

            if (Directory.Exists(userLogDir))
            {
                if (File.Exists(exportLogDir))
                {
                    File.Delete(exportLogDir);
                }
                Directory.CreateDirectory(exportLogDir);
                
                //copy the logs first.  (If you try to do a read on the original log file, you get an error saying another process is using it.)
                System.Collections.Generic.IEnumerable<string> logFiles = Directory.EnumerateFiles(userLogDir);
                foreach (string filePath in logFiles)
                {
                    string[] pathBits = filePath.Split('\\');
                    string fileName = pathBits[pathBits.Length - 1];
                    string newFilePath = Path.Combine(exportLogDir, fileName);
                    File.Copy(filePath, newFilePath, true);
                }

                foreach (FileInfo fileToCompress in new DirectoryInfo(exportLogDir).GetFiles()) { 
                    using (FileStream originalFileStream = fileToCompress.OpenRead())
                    {
                        if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                        {
                            string newFileName = Path.Combine(exportLogDir, fileToCompress.Name) + ".gz";
                            using (FileStream compressedFileStream = File.Create(newFileName))
                            {
                                using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                                {
                                    originalFileStream.CopyTo(compressionStream);
                                }
                            }
                        }
                    }
                    fileToCompress.Delete();
                }
            }
        }

    }
}