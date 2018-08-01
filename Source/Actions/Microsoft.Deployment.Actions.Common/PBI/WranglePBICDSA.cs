using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model;

namespace Microsoft.Deployment.Actions.Common.PBI
{
    [Export(typeof(IAction))]
    public class WranglePBICDSA : BaseAction
    {
        private const int RETRY_ATTEMPTS = 10;

        private FileStream AVAwareOpen(string path, FileMode mode, FileAccess access, FileShare share)
        {
            FileStream result = null;
            List<Exception> exceptionList = new List<Exception>();

            for (int i = 0; i < RETRY_ATTEMPTS; i++)
            {
                try
                {
                    result = new FileStream(path, mode, access, share);
                    break;
                }
                catch (Exception e)
                {
                    exceptionList.Add(e);
                    Thread.Sleep(250);
                    continue;
                }
            }

            if (result == null)
                throw new AggregateException($"Could not open file: {path}", exceptionList);

            return result;
        }

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string connectionString = request.DataStore.GetValueAtIndex("SqlConnectionString", "SqlServerIndex");

            string[] originalFiles = request.DataStore.GetValue("FileName").Split('|');
            var tempFolders = new string[originalFiles.Length];
            var serverPaths = new string[originalFiles.Length];

            for (int i = 0; i < originalFiles.Length; i++)
            {
                string templateFullPath = request.Info.App.AppFilePath + $"/service/PowerBI/{originalFiles[i]}";
                tempFolders[i] = Path.GetRandomFileName();
                Directory.CreateDirectory(request.Info.App.AppFilePath + $"/Temp/{tempFolders[i]}");

                SqlCredentials creds = SqlUtility.GetSqlCredentialsFromConnectionString(connectionString);

                using (PBIXUtils wrangler = new PBIXUtils(templateFullPath, request.Info.App.AppFilePath + $"/Temp/{tempFolders[i]}/{originalFiles[i]}"))
                {
                    wrangler.ReplaceKnownVariableinMashup("STSqlServer", creds.Server);
                    wrangler.ReplaceKnownVariableinMashup("STSqlDatabase", creds.Database);
                }
            }

            for (int i = 0; i < originalFiles.Length; i++)
            {
                serverPaths[i] = request.Info.ServiceRootUrl + request.Info.ServiceRelativePath + request.Info.App.AppRelativeFilePath + $"/Temp/{tempFolders[i]}/{originalFiles[i]}";
            }

            return new ActionResponse(ActionStatus.Success, JsonUtility.Serialize<string[]>(serverPaths));
        }
    }
}