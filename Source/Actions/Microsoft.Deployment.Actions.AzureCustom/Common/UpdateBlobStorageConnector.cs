﻿using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Actions.AzureCustom.LogicApp;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.ErrorCode;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class UpdateBlobStorageConnector : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            var resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            var location = request.DataStore.GetJson("SelectedLocation", "Name");
            var connectorName = request.DataStore.GetValue("ConnectorName");
            var connectorDisplayName = request.DataStore.GetValue("ConnectorDisplayName");

            JToken connectorPayload = request.DataStore.GetJson("ConnectorPayload");

            LogicAppConnector connector = new LogicAppConnector()
            {
                id = $"subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.Web/connections/{connectorName}",
                location = location,
                name = connectorName,
                properties = new Properties()
                {
                    api = new Api()
                    {
                        id = $"subscriptions/{subscription}/providers/Microsoft.Web/locations/{location}/managedApis/{connectorName}",
                        location = location,
                        name = connectorName,
                        type = "Microsoft.Web/locations/managedApis"
                    },
                    displayName = connectorDisplayName,
                    parametervalues = connectorPayload

                }
            };

            JObject finalPayload = JObject.FromObject(connector);
            

            HttpResponseMessage connection = await new AzureHttpClient(azureToken, subscription, resourceGroup).ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Put,
                $"/providers/Microsoft.Web/connections/{connectorName}", "2015-08-01-preview", JsonUtility.GetJsonStringFromObject(finalPayload));


            if (!connection.IsSuccessStatusCode)
            {
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetJObjectFromJsonString(await connection.Content.ReadAsStringAsync()), null,
                    DefaultErrorCodes.DefaultErrorCode, "Failed to create connection");
            }

            var connectionData = JsonUtility.GetJObjectFromJsonString(await connection.Content.ReadAsStringAsync());
            if (connectionData["properties"]["statuses"][0]["status"].ToString() != "Connected")
            {
                return new ActionResponse(ActionStatus.Failure, connectionData);
            }

            return new ActionResponse(ActionStatus.Success, connectionData);
        }
    }
}