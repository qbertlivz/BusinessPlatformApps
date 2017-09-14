namespace Microsoft.Deployment.Common.Model.StreamAnalytics
{
    public class StreamAnalyticsOutputSqlProperties
    {
        public StreamAnalyticsOutputSqlPropertiesDatasource Datasource;

        public StreamAnalyticsOutputSqlProperties(string server, string database, string user, string password, string table)
        {
            Datasource = new StreamAnalyticsOutputSqlPropertiesDatasource(server, database, user, password, table);
        }
    }
}