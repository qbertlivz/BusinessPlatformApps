using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Enums;
using Microsoft.Deployment.Common.ErrorCode;
using Microsoft.Deployment.Common.Helpers;
using System.Net.Http;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class ExportActivityLogToEventHub : BaseAction
    {
        // Exports an Activity Log for the given subscription to Event Hub
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string token = request.DataStore.GetJson("AzureToken", "access_token");
            string subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            string resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            string ehnamespace = request.DataStore.GetValue("ActivityLogNamespace");
            string apiVersion = "2016-03-01";
            string uri = $"https://management.azure.com/subscriptions/{subscription}/providers/microsoft.insights/logprofiles/default?api-version={apiVersion}";
            string body = $"{{\"id\":null,\"location\":null,\"name\":null,\"properties\":{{\"categories\":[\"Write\",\"Delete\",\"Action\"],\"storageAccountId\":null,\"locations\":[\"australiaeast\",\"australiasoutheast\",\"brazilsouth\",\"canadacentral\",\"canadaeast\",\"centralindia\",\"centralus\",\"eastasia\",\"eastus\",\"eastus2\",\"japaneast\",\"japanwest\",\"koreacentral\",\"koreasouth\",\"northcentralus\",\"northeurope\",\"southcentralus\",\"southindia\",\"southeastasia\",\"uksouth\",\"ukwest\",\"westcentralus\",\"westeurope\",\"westindia\",\"westus\",\"westus2\",\"global\"],\"retentionPolicy\":{{\"enabled\":false,\"days\":0}},\"serviceBusRuleId\":\"/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.EventHub/namespaces/{ehnamespace}/authorizationrules/RootManageSharedAccessKey\"}},\"tags\":null}}";
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            HttpResponseMessage response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Put, uri, body);
            if (!response.IsSuccessStatusCode)
            {
                for (int i = 0; i < 5; i++) {
                    response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Put, uri, body);
                    if (response.IsSuccessStatusCode)
                    {
                        return new ActionResponse(ActionStatus.Success);
                    }
                    Thread.Sleep(4000);
                }
            }
            return response.IsSuccessStatusCode ? new ActionResponse(ActionStatus.Success) : new ActionResponse(ActionStatus.Failure);
        }
    }
}