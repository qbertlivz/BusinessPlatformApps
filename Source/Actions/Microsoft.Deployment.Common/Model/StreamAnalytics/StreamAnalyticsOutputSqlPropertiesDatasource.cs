namespace Microsoft.Deployment.Common.Model.StreamAnalytics
{
    public class StreamAnalyticsOutputSqlPropertiesDatasource
    {
        public StreamAnalyticsOutputSqlPropertiesDatasourceProperties Properties;
        public string Type = "Microsoft.Sql/Server/Database";

        public StreamAnalyticsOutputSqlPropertiesDatasource(string server, string database, string user, string password, string table)
        {
            Properties = new StreamAnalyticsOutputSqlPropertiesDatasourceProperties(server, database, user, password, table);
        }
    }
}