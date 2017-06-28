using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.AzureCustom.ActivityLogs
{
    [Export(typeof(IAction))]
    public class GetHistoricalData : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var token = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            System.DateTime now = System.DateTime.UtcNow;
            string nowString = now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            System.DateTime days90ago = now.Subtract(new System.TimeSpan(2160, 0, 0));
            string days90agoString = days90ago.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var geturi = $"https://management.azure.com/subscriptions/{subscription}/providers/microsoft.insights/eventtypes/management/values?api-version=2015-04-01&$filter=eventTimestamp ge '{days90agoString}' and eventTimestamp le '{nowString}' and eventChannels eq 'Admin, Operation'";
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            HttpResponseMessage response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Get, geturi, "{}");
            string responseString = await response.Content.ReadAsStringAsync();
            JObject responseObj = JsonUtility.GetJsonObjectFromJsonString(responseString);
            var responseArr = responseObj.GetValue("value");




            return response.IsSuccessStatusCode ? new ActionResponse(ActionStatus.Success) : new ActionResponse(ActionStatus.Failure);
        }
    }
}