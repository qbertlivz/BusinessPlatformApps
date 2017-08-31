using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FacebookETL
{
    public class SqlUtility
    {
        public static void RunStoredProc(string sqlConnectionString, string storedProc)
        {
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                using (
                    SqlCommand command = new SqlCommand(storedProc, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                {
                    command.CommandTimeout = 180;
                    conn.Open();
                    var obj = command.ExecuteScalar();
                    conn.Close();
                }
            }
        }

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

        public static string[] GetPages(string sqlConnectionString, string schema)
        {
            string pagesCommaSeperated = null;
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"SELECT TOP 1 [value] FROM {schema}.[configuration] WHERE [configuration_group] = 'SolutionTemplate' AND [configuration_subgroup] = 'ETL' AND [name] = 'PagesToFollow'");
                command.Connection = conn;
                var dataReader = command.ExecuteReader();
                dataReader.Read();
                pagesCommaSeperated = dataReader[0].ToString();
                conn.Close();
            }

            return pagesCommaSeperated.Split(',');
        }
    }
}
