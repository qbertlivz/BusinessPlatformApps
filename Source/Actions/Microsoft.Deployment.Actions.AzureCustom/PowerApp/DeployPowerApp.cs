using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

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

            AzureHttpClient ahc = new AzureHttpClient(azureToken);

            string applicationName = "TwitterTemplate" + RandomGenerator.GetDateStamp();
            string environmentId = request.DataStore.GetValue("PowerAppEnvironment");
            string objectId = JsonUtility.GetWebToken(azureToken, "oid");
            string sqlConnectionId = request.DataStore.GetValue("PowerAppSqlConnectionId");

            PowerAppResourceStorage resourceStorage = await ahc.Request<PowerAppResourceStorage>(HttpMethod.Post,
                string.Format(PowerAppUtility.URL_POWERAPPS_GENERATE_RESOURCE_STORAGE, objectId),
                JsonUtility.Serialize<PowerAppEnvironmentWrapper>(new PowerAppEnvironmentWrapper(environmentId)));

            PowerAppSession session = await ahc.Request<PowerAppSession>(HttpMethod.Post,
                string.Format(PowerAppUtility.URL_POWERAPPS_INITIATE_SESSION, objectId));

            AzureHttpClient ahc2 = new AzureHttpClient(azureToken, new Dictionary<string, string>()
            {
                {
                    "AuthoringSessionToken", session.SessionToken
                }
            });

            //List<object> response = await ahc.Request<List<object>>(HttpMethod.Post, PowerAppUtility.URL_POWERAPPS_PUBLISH_TO_BLOB, string.Empty);

            //string backgroundImageUri = sharedAccessSignature.Replace("?", "/logoSmallFile?");
            //string documentUri = sharedAccessSignature.Replace("?", "/document.msapp?");

            //await clientPA.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, $"{CREATE_POWER_APPS_URL}/authoringsession/newinstance?requestedLocation=unitedstates", string.Empty);
            //await clientPA.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Get, $"{CREATE_POWER_APPS_URL}/document/util/createdocumentandloadcontext?apptype=0&locale=en-US", string.Empty);
            //await clientPA.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Get, $"{CREATE_POWER_APPS_URL}/document/2/getpublishinfo", string.Empty);
            //await clientPA.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, $"{CREATE_POWER_APPS_URL}/document/2/setpublishinfo", $"{{\"applicationName\":\"{applicationName}\",\"backgroundColorString\":\"RGBA(0,176,240,1)\",\"logoFile\":{{\"_path\":\"default_icon.png\"}},\"logoFileName\":\"default_icon.png\",\"publishDataLocally\":false,\"publishResourcesLocally\":false,\"publishTarget\":null}}");
            //await clientPA.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, $"{CREATE_POWER_APPS_URL}/document/2/publishtoblobasync", $"{{\"blobURI\":\"{sharedAccessSignature}\",\"docName\":\"{applicationName}\",\"documentSienaUri\":\"/document.msapp\",\"logoSmallUri\":\"/logoSmallFile\"}}");
            //await clientPA.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, $"{BASE_POWER_APPS_URL}/apps?api-version=2016-11-01", $"{{\"properties\":{{\"appUris\":{{\"documentUri\":{{\"value\":\"{documentUri}\"}}}},\"description\":\"\",\"backgroundColor\":\"RGBA(0,176,240,1)\",\"minClientVersion\":{{\"major\":2,\"minor\":0,\"build\":610,\"majorRevision\":0,\"minorRevision\":0,\"revision\":0}},\"createdByClientVersion\":{{\"major\":2,\"minor\":0,\"build\":610,\"majorRevision\":0,\"minorRevision\":0,\"revision\":0}},\"backgroundImageUri\":\"{backgroundImageUri}\",\"displayName\":\"{applicationName}\",\"environment\":{{\"id\":\"/providers/Microsoft.PowerApps/environments/{environmentId}\",\"name\":\"{environmentId}\"}}}},\"tags\":{{\"sienaVersion\":\"{GetNewPowerAppGuid()}\",\"deviceCapabilities\":\"\",\"supportsPortrait\":\"false\",\"supportsLandscape\":\"true\",\"primaryFormFactor\":\"Tablet\",\"primaryDeviceWidth\":\"1366\",\"primaryDeviceHeight\":\"768\",\"publisherVersion\":\"2.0.610\",\"minimumRequiredApiVersion\":\"2.1.0\"}},\"name\":\"{applicationId}\"}}");

            return new ActionResponse(ActionStatus.Success);
        }
    }
}