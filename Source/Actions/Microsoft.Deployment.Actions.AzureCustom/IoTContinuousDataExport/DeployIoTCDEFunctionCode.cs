using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.ErrorCode;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.AzureCustom.IoTContinuousDataExport
{
    [Export(typeof(IAction))]
    public class DeployIoTCDEFunctionCode : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string tokenAzure = request.DataStore.GetJson("AzureToken", "access_token");

            var zipFileBinary = File.ReadAllBytes(Path.Combine(request.Info.App.AppFilePath, request.DataStore.GetValue("ZipFile")));

            var functionAppName = request.DataStore.GetValue("siteName");
            var url = $"https://{functionAppName}.scm.azurewebsites.net/api/zipdeploy";

            var client = new AzureHttpClient(tokenAzure);
            var result = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, url, zipFileBinary);

            if (!result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                return new ActionResponse(
                    ActionStatus.Failure, 
                    JsonUtility.GetJObjectFromJsonString(response),
                    null, 
                    DefaultErrorCodes.DefaultErrorCode, 
                    "Error uploading function code");
            }

            return new ActionResponse(ActionStatus.Success);
        }
    }
}