using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.AzureCustom.PowerApp
{
    [Export(typeof(IAction))]
    public class DeployPowerApp : BaseAction
    {
        private string BASE_POWER_APPS_URL = "https://management.azure.com/providers/Microsoft.PowerApps";
        private string CREATE_POWER_APPS_URL = "https://create.powerapps.com/v2.0.610.0/api/document/2";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");

            AzureHttpClient client = new AzureHttpClient(azureToken);

            string environmentId = request.DataStore.GetValue("PowerAppEnvironment");
            string objectId = JsonUtility.GetWebToken(azureToken, "oid");
            string sqlConnectionId = request.DataStore.GetValue("PowerAppSqlConnectionId");

            JObject resourceStorage = JsonUtility.GetJsonObjectFromJsonString(await client.ExecuteGenericRequestWithHeaderAndReadAsync(HttpMethod.Post, $"{BASE_POWER_APPS_URL}/objectIds/{objectId}/generateResourceStorage?api-version=2016-11-01", $"{{\"environment\":{{\"id\":\"/providers/Microsoft.PowerApps/environments/{environmentId}\",\"name\":\"{environmentId}\"}}}}}}"));

            string sharedAccessSignature = JsonUtility.GetJObjectProperty(resourceStorage, "sharedAccessSignature");

            string applicationName = "TwitterTemplate" + RandomGenerator.GetDateStamp();

            var response = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, $"{CREATE_POWER_APPS_URL}/setpublishinfo", $"{{\"applicationName\":\"{applicationName}\",\"backgroundColorString\":\"RGBA(0, 176, 240, 1)\",\"logoFile\":{{\"_path\":\"https://paawus2blobs.blob.core.windows.net/26f7409b-afa2-4950-9277-5ae82a7473c5/icon/default_icon.png?sv=2015-02-21&sr=b&sig=39JCTgrexHxChB9hIM%2FQTDzYN4mLVDUECbf2Qwmxnwk%3D&se=2017-04-14T21%3A23%3A48Z&sp=rw\"}},\"logoFileName\":\"https://paawus2blobs.blob.core.windows.net/26f7409b-afa2-4950-9277-5ae82a7473c5/icon/default_icon.png?sv=2015-02-21&sr=b&sig=39JCTgrexHxChB9hIM%2FQTDzYN4mLVDUECbf2Qwmxnwk%3D&se=2017-04-14T21%3A23%3A48Z&sp=rw\",\"publishDataLocally\":false,\"publishResourcesLocally\":false,\"publishTarget\":null}}");
            var responseString = await response.Content.ReadAsStringAsync();

            var response2 = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, $"{CREATE_POWER_APPS_URL}/publishtoblobasync", $"{{\"blobURI\":\"{sharedAccessSignature}\",\"docName\":\"{applicationName}\",\"documentSienaUri\":\"/document.msapp\",\"logoSmallUri\":\"/logoSmallFile\"}}");
            var responseString2 = await response2.Content.ReadAsStringAsync();

            var response3 = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, $"{BASE_POWER_APPS_URL}/apps?api-version=2016-11-01", $"{{\"properties\":{{\"appUris\":{{\"documentUri\":{{\"value\":\"{applicationName}\"}},\"description\":\"\",\"backgroundColor\":\"RGBA(0,176,240,1)\",\"minClientVersion\":{{\"major\":2,\"minor\":0,\"build\":610,\"majorRevision\":0,\"minorRevision\":0,\"revision\":0}},\"createdByClientVersion\":{{\"major\":2,\"minor\":0,\"build\":610,\"majorRevision\":0,\"minorRevision\":0,\"revision\":0}},\"backgroundImageUri\":\"https://pafeblobprodby.blob.core.windows.net/20170413t000000z7a82f6b2575e4dea8baad9eb2b0a3591/logoSmallFile?sv=2014-02-14&sr=c&si=SASpolicy&sig=93HQkBuf972LGS0y%2FqyPgXDcAhFbGSSt5y3RlZQE%2B8o%3D\",\"displayName\":\"{applicationName}\",\"environment\":{{\"id\":\"/providers/Microsoft.PowerApps/environments/{environmentId}\",\"name\":\"{environmentId}\"}},\"connectionReferences\":{{\"cbe235ee-eb8b-5329-1c73-812825f07146\":{{\"id\":\"/providers/microsoft.powerapps/apis/shared_sql\",\"parameterHints\":{{}},\"dataSources\":[\"[pbist_twitter].[twitter_query]\",\"[pbist_twitter].[twitter_query_details]\",\"[pbist_twitter].[twitter_query_readable]\"],\"dependents\":[],\"dependencies\":[],\"sharedConnectionId\":\"/providers/microsoft.powerapps/apis/shared_sql/connections/{sqlConnectionId}\",\"isOnPremiseConnection\":false}}}}}},\"tags\":{{\"sienaVersion\":\"7084622c-9b66-30d8-a9d8-afcbf07d5aa3\",\"deviceCapabilities\":\"\",\"supportsPortrait\":\"false\",\"supportsLandscape\":\"true\",\"primaryFormFactor\":\"Tablet\",\"primaryDeviceWidth\":\"1366\",\"primaryDeviceHeight\":\"768\",\"publisherVersion\":\"2.0.610\",\"minimumRequiredApiVersion\":\"2.1.0\"}},\"name\":\"c2f4aa53-4bf7-d196-13c1-170849a95539\"}}");
            var responseString3 = await response3.Content.ReadAsStringAsync();

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
        }
    }
}