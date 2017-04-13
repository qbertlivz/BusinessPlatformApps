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

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");

            AzureHttpClient client = new AzureHttpClient(azureToken);

            string objectId = JsonUtility.GetWebToken(azureToken, "oid");

            JObject generateResourceStorageResponse = JsonUtility.GetJsonObjectFromJsonString(await client.ExecuteGenericRequestWithHeaderAndReadAsync(HttpMethod.Post, $"{BASE_POWER_APPS_URL}/objectIds/{objectId}/generateResourceStorage?api-version=2016-11-01", "{}"));

            string sharedAccessSignature = JsonUtility.GetJObjectProperty(generateResourceStorageResponse, "sharedAccessSignature");

            //if (environments["value"] == null)
            //{
            //    var skipPowerApp = request.DataStore.GetValue("SkipPowerApp");
            //    if (skipPowerApp == null)
            //    {
            //        request.DataStore.AddToDataStore("SkipPowerApp", "true", DataStoreType.Public);
            //        //return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "PowerAppNoEnvironment");
            //    }
            //    
            //}

            //foreach (var environment in environments["value"])
            //{
            //    bool isDefault = false;
            //    bool.TryParse(environment["properties"]["isDefault"].ToString(), out isDefault);
            //    if (isDefault && environment["properties"]["permissions"]["CreatePowerApp"] != null)
            //    {
            //        request.DataStore.AddToDataStore("PowerAppEnvironment", environment["name"].ToString(), DataStoreType.Private);
            //        return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
            //    };
            //}

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
        }
    }
}