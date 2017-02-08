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
            string connectionString = request.DataStore.GetAllValues("SqlConnectionString")[1];

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

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
        }
    }
}