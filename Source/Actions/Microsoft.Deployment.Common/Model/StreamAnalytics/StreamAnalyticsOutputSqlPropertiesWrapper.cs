namespace Microsoft.Deployment.Common.Model.StreamAnalytics
{
    public class StreamAnalyticsOutputSqlPropertiesWrapper
    {
        public StreamAnalyticsOutputSqlProperties Properties;

        public StreamAnalyticsOutputSqlPropertiesWrapper(string server, string database, string user, string password, string table)
        {
            Properties = new StreamAnalyticsOutputSqlProperties(server, database, user, password, table);
        }
    }
}