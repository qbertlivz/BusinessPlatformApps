using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using System;

namespace Microsoft.Deployment.Actions.SQL
{
    [Export(typeof(IAction))]
    public class CreateSqlUser : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            const string CMD_CREATE_USER    = "CREATE LOGIN {0} WITH password='{1}'; " +
                                              "-- CREATE USER {0} FOR LOGIN {0} WITH DEFAULT_SCHEMA=[dbo];";
            const string CMD_ADD_USER_TO_DB = "CREATE USER {0} FOR LOGIN {0} WITH DEFAULT_SCHEMA=[dbo]; ";
            const string CMD_ADD_USER_TO_READER_ROLE = "EXEC sp_addrolemember 'db_datareader', '{0}'";
            const string CMD_ADD_USER_TO_WRITER_ROLE = "EXEC sp_addrolemember 'db_datawriter', '{0}'";
            const string CMD_GRANT_EXEC_TO_USER      = "GRANT EXECUTE ON SCHEMA ::{1} TO {0}";

            SqlConnectionStringBuilder cnBuilder = new SqlConnectionStringBuilder();
            request.DataStore.GetValueAtIndex("SqlConnectionString", "SqlServerIndex");
            string newUser = request.DataStore.GetValue("NewSqlUser");
            string newPassword = request.DataStore.GetValue("NewSqlPassword");
            cnBuilder.InitialCatalog = "master";

            using (SqlConnection cn = new SqlConnection(cnBuilder.ToString()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand() { Connection = cn, CommandTimeout=0 })
                {
                    cmd.CommandText = string.Format(CMD_CREATE_USER, newUser, newPassword);
                    cmd.ExecuteNonQuery();

                    cn.ChangeDatabase(request.DataStore.GetValue("databasename"));
                    cmd.CommandText = string.Format(CMD_ADD_USER_TO_DB, newUser);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format(CMD_ADD_USER_TO_READER_ROLE, newUser);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format(CMD_ADD_USER_TO_WRITER_ROLE, newUser);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format(CMD_GRANT_EXEC_TO_USER, "dbo");
                    cmd.ExecuteNonQuery();

                    foreach (string s in request.DataStore.GetValue("additionalSchemas").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        cmd.CommandText = string.Format(CMD_GRANT_EXEC_TO_USER, s.Trim());
                        cmd.ExecuteNonQuery();
                    }

                }


            }
            return new ActionResponse(ActionStatus.Success);
        }
    }
}