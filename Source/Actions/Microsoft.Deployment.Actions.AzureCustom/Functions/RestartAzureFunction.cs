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
using Microsoft.Deployment.Common.Model.Azure;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class RestartAzureFunction : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            var resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            var location = request.DataStore.GetJson("SelectedLocation", "Name");
            var sitename = request.DataStore.GetValue("FunctionName");
            //bool IsRunning = true;

            AzureHttpClient client = new AzureHttpClient(azureToken, subscription, resourceGroup);
            var responseMessage = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Post, $"providers/Microsoft.Web/sites/{sitename}/start", "2016-08-01", "{}");

            //if restart is successful, check the function running status.
            if (responseMessage.IsSuccessStatusCode)
            {
                await Task.Delay(60000);
                return new ActionResponse(ActionStatus.Success);
                //var functionResponse = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Get, $"providers/Microsoft.Web/sites/{sitename}/functions/LithiumETL", "2016-08-01", "{}");
            }
            else
            {
                return new ActionResponse(ActionStatus.Failure);
            }

            //var functions = JObject.Parse(await response.Content.ReadAsStringAsync());

            //while (IsRunning)
            //{
            //    var existingResponse = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Post, $"providers/Microsoft.Web/sites/{sitename}/functions/LithiumETL", "2016-08-01", "{}");

            //    if (!existingResponse.IsSuccessStatusCode)
            //    {
            //        return new ActionResponse(ActionStatus.Failure);
            //    }
            //    else
            //    {
            //        var appSettingsPayload = JObject.Parse(await existingResponse.Content.ReadAsStringAsync());

            //        //wait for 10 sec to check back the function running status
            //        await Task.Delay(10000);
            //        return new ActionResponse(ActionStatus.Success);
            //    }
            //}



        }

        
    }
}