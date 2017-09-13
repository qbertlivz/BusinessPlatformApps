namespace Microsoft.Deployment.Common.Model.APIManagement
{
    public class APIManagementLoggerPropertiesCredentials
    {
        public string ConnectionString;
        public string Name;

        public APIManagementLoggerPropertiesCredentials(string nameEventHub, string connectionString)
        {
            ConnectionString = connectionString;
            Name = nameEventHub;
        }
    }
}