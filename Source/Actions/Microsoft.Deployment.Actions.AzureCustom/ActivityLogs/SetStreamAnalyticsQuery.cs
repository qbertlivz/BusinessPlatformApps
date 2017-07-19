using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.AzureCustom.ActivityLogs
{
    [Export(typeof(IAction))]
    public class SetStreamAnalyticsQuery : BaseAction
    {
        // Updates the default query provided by a Stream Analytics job
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var token = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            var resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            var jobName = request.DataStore.GetValue("SAJob");
            var transformationName = "Transformation";
            var apiVersion = "2015-10-01";
            string uri = $"https://management.azure.com/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.StreamAnalytics/streamingjobs/{jobName}/transformations/{transformationName}?api-version={apiVersion}";
            string input = request.DataStore.GetValue("inputAlias");
            string output = request.DataStore.GetValue("outputAlias");
            var body = $"{{\"properties\":{{\"streamingUnits\":1,\"query\":\"SELECT arrayElement.ArrayValue.[identity].claims.[http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn] as caller, arrayElement.ArrayValue.correlationId, arrayElement.ArrayValue.resultDescription as description, arrayElement.ArrayValue.Properties.eventCategory, arrayElement.ArrayValue.Properties.eventProperties.communication as impact, arrayElement.ArrayValue.Properties.eventProperties.region as impactedRegions, arrayElement.ArrayValue.Properties.eventProperties.JobFailedMessage, CASE WHEN arrayElement.ArrayValue.level = 'Information' THEN 'Informational' ELSE arrayElement.ArrayValue.level END as level, arrayElement.ArrayValue.category as operationCategory, arrayElement.ArrayValue.Properties.operationId, arrayElement.ArrayValue.operationName, CASE WHEN CHARINDEX('/', SUBSTRING(arrayElement.ArrayValue.resourceId, 68, 1000)) = 0 THEN SUBSTRING(arrayElement.ArrayValue.resourceId, 68, 1000) ELSE SUBSTRING(arrayElement.ArrayValue.resourceId, 68, CHARINDEX('/', SUBSTRING(arrayElement.ArrayValue.resourceId, 68, 1000)) - 1) END as resourceGroup, arrayElement.ArrayValue.resourceId, CASE WHEN arrayElement.ArrayValue.resultType = 'Start' THEN 'Started' WHEN arrayElement.ArrayValue.resultType = 'Accept' THEN 'Accepted' ELSE arrayElement.ArrayValue.resultType END as status, arrayElement.ArrayValue.Properties.statusCode, SUBSTRING(arrayElement.ArrayValue.resourceId, 16, 36) as subscriptionId, arrayElement.ArrayValue.time as [timestamp] INTO [{output}] FROM [{input}] as event CROSS APPLY GetArrayElements(event.records) AS arrayElement\"}}}}";
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            HttpResponseMessage response = await ahc.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Put, uri, body);
            return response.IsSuccessStatusCode ? new ActionResponse(ActionStatus.Success) : new ActionResponse(ActionStatus.Failure);
        }
    }
}
