using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Model.StreamAnalytics;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System;

namespace Microsoft.Deployment.Actions.AzureCustom.ActivityLogs
{
    [Export(typeof(IAction))]
    public class GetHistoricalData : BaseAction
    {
        public static void BulkInsert(string connString, DataTable table, string tableName)
        {
            try
            {
                using (SqlBulkCopy bulk = new SqlBulkCopy(connString))
                {
                    bulk.BatchSize = 1000;
                    bulk.DestinationTableName = tableName;
                    bulk.WriteToServer(table);
                    bulk.Close();
                }
            }
            catch
            {
                throw new Exception("overflow during batch insert in table " + tableName);
            }
        }

        public static DataTable createHistoricalDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("authorizationAction");
            table.Columns.Add("authorizationScope");
            table.Columns.Add("caller");
            table.Columns.Add("correlationId");
            table.Columns.Add("description");
            table.Columns.Add("eventDataId");
            table.Columns.Add("httpRequestClientId");
            table.Columns.Add("httpRequestClientIpAddr");
            table.Columns.Add("httpRequestMethod");
            table.Columns.Add("level");
            table.Columns.Add("resourceGroupName");
            table.Columns.Add("resourceId");
            table.Columns.Add("resourceProviderName");
            table.Columns.Add("operationId");
            table.Columns.Add("operationName");
            table.Columns.Add("statusCode");
            table.Columns.Add("status");
            table.Columns.Add("subStatus");
            table.Columns.Add("timestamp");
            table.Columns.Add("subscriptionId");
            return table;
        }

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var token = request.DataStore.GetJson("AzureToken", "access_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            System.DateTime now = System.DateTime.UtcNow;
            System.DateTime days90ago = now.Subtract(new System.TimeSpan(2160, 0, 0));
            string nowString = now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string days90agoString = days90ago.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var geturi = $"https://management.azure.com/subscriptions/{subscription}/providers/microsoft.insights/eventtypes/management/values?api-version=2015-04-01&$filter=eventTimestamp ge '{days90agoString}' and eventTimestamp le '{nowString}' and eventChannels eq 'Admin, Operation'";
            // smaller time interval ~500 records below
            //var geturi = $"https://management.azure.com/subscriptions/{subscription}/providers/microsoft.insights/eventtypes/management/values?api-version=2015-04-01&$filter=eventTimestamp ge '2017-06-28T17:56:33.880Z' and eventTimestamp le '2017-06-29T17:56:33.880Z' and eventChannels eq 'Admin, Operation'";
            var sqlConn = request.DataStore.GetValue("SqlConnectionString");
            var historicalTable = createHistoricalDataTable();
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            while (true)
            {
                ActivityLogResponse response = JsonUtility.Deserialize<ActivityLogResponse>(await ahc.Request(HttpMethod.Get, geturi));
                foreach (ActivityLogEntry activity in response.Value)
                {
                    DataRow historicalRow = historicalTable.NewRow();
                    if (activity.Authorization != null)
                    {
                        historicalRow["authorizationAction"] = activity.Authorization.Action;
                        historicalRow["authorizationScope"] = activity.Authorization.Scope;
                    }
                    if (activity.Claims != null)
                    {
                        historicalRow["caller"] = activity.Claims.Upn;
                    }
                    historicalRow["correlationId"] = activity.CorrelationId;
                    historicalRow["description"] = activity.Description;
                    historicalRow["eventDataId"] = activity.EventDataId;
                    if (activity.HttpRequest != null)
                    {
                        historicalRow["httpRequestClientId"] = activity.HttpRequest.ClientRequestId;
                        historicalRow["httpRequestClientIpAddr"] = activity.HttpRequest.ClientIpAddress;
                        historicalRow["httpRequestMethod"] = activity.HttpRequest.Method;
                    }
                    historicalRow["level"] = activity.Level;
                    historicalRow["operationId"] = activity.OperationId;
                    historicalRow["resourceGroupName"] = activity.ResourceGroupName;
                    historicalRow["resourceId"] = activity.Id;
                    if (activity.ResourceProviderName != null)
                    {
                        historicalRow["resourceProviderName"] = activity.ResourceProviderName.LocalizedValue;
                    }
                    if (activity.OperationName != null)
                    {
                        historicalRow["operationName"] = activity.OperationName.LocalizedValue;
                    }
                    if (activity.Properties != null)
                    {
                        historicalRow["statusCode"] = activity.Properties.StatusCode;
                    }
                    if (activity.Status != null)
                    {
                        historicalRow["status"] = activity.Status.LocalizedValue;
                    }
                    if (activity.SubStatus != null)
                    {
                        historicalRow["subStatus"] = activity.SubStatus.LocalizedValue;
                    }
                    historicalRow["subscriptionId"] = activity.SubscriptionId;
                    historicalRow["timestamp"] = activity.EventTimestamp;
                    historicalTable.Rows.Add(historicalRow);
                }
                if ((geturi = response.NextLink) == null)
                {
                    break;
                }
            }
            BulkInsert(sqlConn, historicalTable, "aal.HistoricalData");
            return new ActionResponse(ActionStatus.Success);
        }
    }
}