namespace Microsoft.Deployment.Common.Model.StreamAnalytics
{
    public class StreamAnalyticsOutputSqlPropertiesDatasourceProperties
    {
        public string Database;
        public string Password;
        public string Server;
        public string Table;
        public string User;

        public StreamAnalyticsOutputSqlPropertiesDatasourceProperties(string server, string database, string user, string password, string table)
        {
            Database = database;
            Password = password;
            Server = server;
            Table = table;
            User = user;
        }
    }
}