using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.PowerApp;

namespace Microsoft.Deployment.Actions.AzureCustom.PowerApp
{
    [Export(typeof(IAction))]
    public class DeployPowerApp : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            string environmentId = request.DataStore.GetValue("PowerAppEnvironment");

            //string fileName = $"TwitterTemplate{RandomGenerator.GetDateStamp()}.msapp";

            ActionResponse wrangledFile = await RequestUtility.CallAction(request, "Microsoft-WranglePowerApp");

            AzureHttpClient ahc = new AzureHttpClient(azureToken);

            PowerAppResourceStorage resourceStorage = await ahc.Request<PowerAppResourceStorage>(HttpMethod.Post,
                string.Format(PowerAppUtility.URL_POWERAPPS_GENERATE_RESOURCE_STORAGE, JsonUtility.GetWebToken(azureToken, "oid")),
                JsonUtility.Serialize<PowerAppEnvironmentWrapper>(new PowerAppEnvironmentWrapper(environmentId)));

            string document = resourceStorage.SharedAccessSignature.Replace("?", "/document.msapp?");

            CloudBlockBlob blob = new CloudBlockBlob(new Uri(document));

            await blob.UploadFromFileAsync(@"C:\git\Microsoft\BusinessPlatformApps\Source\Test\Microsoft.Deployment.Tests.Actions\bin\Debug" + wrangledFile.Body.ToString());



            return new ActionResponse(ActionStatus.Success);
        }
    }
}