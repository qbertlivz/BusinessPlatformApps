using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Custom;

namespace Microsoft.Deployment.Actions.AzureCustom.Reddit
{
    [Export(typeof(IAction))]
    class PopulateRedditUserDefinedEntities : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            List<NewsEntity> userEntities = JsonUtility.Deserialize<List<NewsEntity>>(request.DataStore.GetValue("UserDefinedEntities"));

            if (userEntities != null)
            {
                var sqlConn = request.DataStore.GetValue("SqlConnectionString");
                
                var userEntityTable = UserEntitiesTable();

                foreach (NewsEntity userEntity in userEntities)
                {
                    var values = new List<string>(
                               userEntity.Values.Split(new string[] { "\n" },
                               System.StringSplitOptions.RemoveEmptyEntries));

                    foreach (var value in values)
                    {
                        var userEntityRow = userEntityTable.NewRow();
                        userEntityRow["regex"] = value ;
                        userEntityRow["entityValue"] = value;
                        userEntityRow["entityType"] = userEntity.Name;
                        userEntityTable.Rows.Add(userEntityRow);
                    }
                }
                
                BulkInsert(sqlConn, userEntityTable, "reddit.UserDefinedEntityDefinitions");
            }

            return new ActionResponse(ActionStatus.Success);
        }


        public static void BulkInsert(string connString, DataTable table, string tableName)
        {
            try
            {
                using (var bulk = new SqlBulkCopy(connString))
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

        public static DataTable UserEntitiesTable()
        {
            var table = new DataTable();
            table.Columns.Add("regex");
            table.Columns.Add("entityType");
            table.Columns.Add("entityValue");
            return table;
        }
    }
}