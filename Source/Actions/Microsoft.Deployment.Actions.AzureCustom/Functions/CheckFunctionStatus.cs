using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.ErrorCode;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class CheckFunctionStatus : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            var resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            var location = request.DataStore.GetJson("SelectedLocation", "Name");
            var sitename = request.DataStore.GetValue("FunctionName");

            AzureHttpClient client = new AzureHttpClient(azureToken, subscription, resourceGroup);

            // Force sync
            dynamic obj = new ExpandoObject();
            obj.subscriptionId = subscription;
            obj.SiteId = new ExpandoObject();
            obj.SiteId.Name = sitename;
            obj.SiteId.ResourceGroup = resourceGroup;
            HttpResponseMessage result = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, "https://web1.appsvcux.ext.azure.com/websites/api/Websites/KuduSync", JsonUtility.GetJsonStringFromObject(obj));
            var resultBody = await result.Content.ReadAsStringAsync();

           
            while(true)
            {
                result = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Get, $"/providers/Microsoft.Web/sites/{sitename}/deployments", "2016-08-01", "");
                resultBody = await result.Content.ReadAsStringAsync();
                if(result.IsSuccessStatusCode)
                {
                    var jobj = JsonUtility.GetJObjectFromJsonString(resultBody);
                    if(jobj["value"] != null && (jobj["value"] as JArray).Count > 0 )
                    {
                        if (bool.Parse(jobj["value"][0]["properties"]["complete"].ToString()) == true)
                        {
                            break;
                        }
                    }
                    
                }

                await Task.Delay(5000);
            }

            return new ActionResponse(ActionStatus.Success);
        }
    }
}