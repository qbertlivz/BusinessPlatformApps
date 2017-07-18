using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model;

namespace Microsoft.Deployment.Actions.OnPremise
{
    [Export(typeof(IAction))]
    public class GetDataPullStatus : BaseAction
    {
        private const int WAIT_INTERVAL = 10000;

        private const string COUNT_NAME = "Count";
        private const string SP_REPLICATION_COUNTS = "sp_get_replication_counts";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            Thread.Sleep(WAIT_INTERVAL);

            DataTable recordCounts = GetRecordCounts(request.DataStore.GetValueAtIndex("SqlConnectionString", "SqlServerIndex"),
                $"[{request.DataStore.GetValue("TargetSchema")}].{SP_REPLICATION_COUNTS}");

            return await ConfirmStatus(request, recordCounts, request.DataStore.GetValue("FinishedActionName"));
        }

        private async Task<ActionResponse> ConfirmStatus(ActionRequest request, DataTable recordCounts, string actionName)
        {
            ActionResponse response = new ActionResponse(IsAtLeastOneRecordComingIn(recordCounts) ? ActionStatus.Success : ActionStatus.InProgress,
                JsonUtility.Serialize<DataPullStatus>(new DataPullStatus()
                {
                    IsFinished = false,
                    Status = JsonUtility.SerializeTable(recordCounts)
                }));

            ActionResponse confirmation = await RequestUtility.CallAction(request, actionName);

            if (confirmation != null)
            {
                switch (confirmation.Status)
                {
                    case ActionStatus.Failure:
                        response = confirmation;
                        break;
                    case ActionStatus.Success:
                        response = new ActionResponse(ActionStatus.Success, JsonUtility.Serialize<DataPullStatus>(new DataPullStatus()
                        {
                            IsFinished = true,
                            Slices = JObject.FromObject(confirmation.Body)["value"]?.ToString(),
                            Status = JsonUtility.SerializeTable(recordCounts)
                        }));
                        break;
                }
            }

            return response;
        }

        private DataTable GetRecordCounts(string connectionString, string query)
        {
            DataTable recordCounts = null;

            try
            {
                recordCounts = SqlUtility.InvokeStoredProcedure(connectionString, query, null);
            }
            catch
            {
                // It's ok for this to fail, we'll just return an empty table
                recordCounts = new DataTable();
            }

            return recordCounts;
        }

        private bool IsAtLeastOneRecordComingIn(DataTable recordCounts)
        {
            bool isAtLeastOneRecordComingIn = false;

            for (int i = 0; i < recordCounts.Rows.Count && !isAtLeastOneRecordComingIn; i++)
            {
                isAtLeastOneRecordComingIn = Convert.ToInt64(recordCounts.Rows[i][COUNT_NAME]) > 0;
            }

            return isAtLeastOneRecordComingIn;
        }
    }
}