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
            string sqlConnectionId = request.DataStore.GetValue("PowerAppSqlConnectionId");

            ActionResponse wrangledFile = await RequestUtility.CallAction(request, "Microsoft-WranglePowerApp");

            AzureHttpClient ahc = new AzureHttpClient(azureToken);

            PowerAppResourceStorage resourceStorage = await ahc.Request<PowerAppResourceStorage>(HttpMethod.Post,
                string.Format(PowerAppUtility.URL_POWERAPPS_GENERATE_RESOURCE_STORAGE, JsonUtility.GetWebToken(azureToken, "oid")),
                JsonUtility.Serialize<PowerAppEnvironmentWrapper>(new PowerAppEnvironmentWrapper(environmentId)));

            string uri = resourceStorage.SharedAccessSignature.Replace("?", "/document.msapp?");

            CloudBlockBlob blob = new CloudBlockBlob(new Uri(uri));

            await blob.UploadFromFileAsync(wrangledFile.Body.ToString());

            PowerAppMetadata metadata = await ahc.Request<PowerAppMetadata>(HttpMethod.Post, PowerAppUtility.URL_POWERAPPS_PUBLISH_APP,
                JsonUtility.Serialize<PowerAppPublish>(new PowerAppPublish(uri, $"TwitterTemplate{RandomGenerator.GetDateStamp()}", environmentId, sqlConnectionId)));

            await ahc.Request(HttpMethod.Post, string.Format(PowerAppUtility.URL_POWERAPPS_SQL_CONNECTION_UPDATE, sqlConnectionId, environmentId),
                JsonUtility.Serialize<PowerAppSqlConnectionUpdate>(new PowerAppSqlConnectionUpdate(metadata.Name)));

            request.DataStore.AddToDataStore("PowerAppUri", string.Format(PowerAppUtility.URL_POWERAPPS_WEB, metadata.Name));

            return new ActionResponse(ActionStatus.Success);
        }
    }
}