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

        public static DataTable createNonServiceHealthTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("eventId");
            table.Columns.Add("caller");
            table.Columns.Add("correlationId");
            table.Columns.Add("description");
            table.Columns.Add("eventCategory");
            table.Columns.Add("level");
            table.Columns.Add("operationCategory");
            table.Columns.Add("operationId");
            table.Columns.Add("operationName");
            table.Columns.Add("resourceGroup");
            table.Columns.Add("resourceId");
            table.Columns.Add("status");
            table.Columns.Add("statusCode");
            table.Columns.Add("timestamp");
            return table;
        }

        public static DataTable createServiceHealthTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("serviceHealthId");
            table.Columns.Add("correlationId");
            table.Columns.Add("description");
            table.Columns.Add("impact");
            table.Columns.Add("impactedRegions");
            table.Columns.Add("impactedServices");
            table.Columns.Add("incidentType");
            table.Columns.Add("level");
            table.Columns.Add("operationId");
            table.Columns.Add("status");
            table.Columns.Add("timestamp");
            table.Columns.Add("title");
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
            var sqlConn = request.DataStore.GetValue("SqlConnectionString");
            var shTable = createServiceHealthTable();
            var nonShTable = createNonServiceHealthTable();
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            while (true)
            {
                string raw = await ahc.Request(HttpMethod.Get, geturi);
                ActivityLogResponse response = JsonUtility.Deserialize<ActivityLogResponse>(raw);
                foreach (ActivityLogEntry activity in response.Value)
                {
                    if (activity.Category != null)
                    {
                        if (activity.Category.Value == "ServiceHealth")
                        {
                            DataRow shRow = shTable.NewRow();
                            shRow["serviceHealthId"] = activity.EventDataId;
                            shRow["correlationId"] = activity.CorrelationId;
                            shRow["description"] = activity.Description;
                            if (activity.Properties != null)
                            {
                                shRow["impact"] = activity.Properties.Impact;
                                shRow["impactedRegions"] = activity.Properties.ImpactedRegions;
                                shRow["impactedServices"] = activity.Properties.ImpactedServices;
                                shRow["incidentType"] = activity.Properties.IncidentType;
                            }
                            shRow["level"] = activity.Level;
                            shRow["operationId"] = activity.OperationId;
                            if (activity.Status != null)
                            {
                                shRow["status"] = activity.Status.LocalizedValue;
                            }
                            shRow["timestamp"] = activity.EventTimestamp;
                            if (activity.Properties != null)
                            {
                                shRow["title"] = activity.Properties.Title;
                            }
                            shTable.Rows.Add(shRow);
                        }
                    }
                    DataRow nonShRow = nonShTable.NewRow();
                    if (activity.Claims != null)
                    {
                        nonShRow["caller"] = activity.Claims.Upn;
                    }
                    nonShRow["correlationId"] = activity.CorrelationId;
                    nonShRow["description"] = activity.Description;
                    if (activity.Category != null)
                    {
                        nonShRow["eventCategory"] = activity.Category.Value;
                    }
                    nonShRow["level"] = activity.Level;
                    if (activity.OperationName != null)
                    {
                        string opName = activity.OperationName.Value;
                        if (opName.Length > 5)
                        {
                            int startidx;
                            if ((startidx = opName.LastIndexOf("/")) != -1)
                            {
                                string opCategory = opName.Substring(startidx + 1).ToLower();
                                if (opCategory == "write")
                                {
                                    nonShRow["operationCategory"] = "Write";
                                }
                                else if (opCategory == "delete")
                                {
                                    nonShRow["operationCategory"] = "Delete";
                                }
                                else if (opCategory == "action")
                                {
                                    nonShRow["operationCategory"] = "Action";
                                }
                            }
                        }
                    }
                    nonShRow["operationId"] = activity.OperationId;
                    if (activity.OperationName != null)
                    {
                        nonShRow["operationName"] = activity.OperationName.LocalizedValue;
                    }
                    nonShRow["resourceGroup"] = activity.ResourceGroupName;
                    nonShRow["resourceId"] = activity.Id;
                    if (activity.Status != null)
                    {
                        nonShRow["status"] = activity.Status.LocalizedValue;
                    }
                    if (activity.Properties != null)
                    {
                        nonShRow["statusCode"] = activity.Properties.StatusCode;
                    }
                    nonShRow["timestamp"] = activity.EventTimestamp;
                    nonShTable.Rows.Add(nonShRow);
                }
                if ((geturi = response.NextLink) == null)
                {
                    break;
                }
            }
            BulkInsert(sqlConn, shTable, "bpst_aal.ServiceHealthData");
            BulkInsert(sqlConn, nonShTable, "bpst_aal.NonServiceHealthData");
            return new ActionResponse(ActionStatus.Success);
        }
    }
}