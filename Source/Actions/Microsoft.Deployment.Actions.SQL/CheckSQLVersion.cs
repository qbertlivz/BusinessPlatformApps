using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Enums;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.SQL
{
    [Export(typeof(IAction))]
    public class CheckSQLVersion : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            // TODO fix hardcoded string as action here
            var allConnectionStrings = request.DataStore.GetAllValues("SqlConnectionString");

            if (allConnectionStrings == null || allConnectionStrings.Count == 0)
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_NoConnectionsInDatastore");

            string connectionString = allConnectionStrings[allConnectionStrings.Count - 1]; // This is always for destination which will be the last entry

            DataTable result = SqlUtility.RunCommand(connectionString, "SELECT @@version AS FullVersion, SERVERPROPERTY('ProductVersion') AS SqlVersion, SERVERPROPERTY('IsLocalDB') AS IsLocalDB, SERVERPROPERTY('Edition') AS SqlEdition", SqlCommandType.ExecuteWithData);
            string version = (string)result.Rows[0]["FullVersion"];
            if (version.IndexOf("Azure SQL Data Warehouse", StringComparison.OrdinalIgnoreCase) > -1)
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_DenyPDW");

            int[] minimumVersion = { 10, 50, 6000, 34 };
            string[] versionParts = ((string)result.Rows[0]["SqlVersion"]).Split('.');
            bool versionOk = true;
            for (int i = 0; i < versionParts.Length; i++)
            {
                int currentVersionPart = int.Parse(versionParts[i]);
                versionOk = versionOk && (currentVersionPart >= minimumVersion[i]);
                if (currentVersionPart > minimumVersion[i])
                    break;
            }

            if (!versionOk)
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_VersionTooLow");

            version = (string)result.Rows[0]["SqlEdition"];
            if (version.IndexOf("Express Edition", StringComparison.OrdinalIgnoreCase) > -1)
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_DenyLocalAndExpress");
            if ((result.Rows[0]["IsLocalDB"] != DBNull.Value) && ((int)result.Rows[0]["IsLocalDB"] == 1))
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_DenyLocalAndExpress");

            result = SqlUtility.RunCommand(connectionString, "SELECT [compatibility_level] FROM sys.databases WHERE [name]=DB_NAME()", SqlCommandType.ExecuteWithData);

            if ( Convert.ToInt32(result.Rows[0]["compatibility_level"])<100)
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_CompatLevelTooLow");

            return new ActionResponse(ActionStatus.Success);
        }
    }
}