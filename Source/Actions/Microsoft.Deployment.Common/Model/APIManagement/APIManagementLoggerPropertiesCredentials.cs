namespace Microsoft.Deployment.Common.Model.APIManagement
{
    public class APIManagementLoggerPropertiesCredentials
    {
        public string ConnectionString;
        public string Name;

        public APIManagementLoggerPropertiesCredentials(string nameNamespace, string connectionString)
        {
            ConnectionString = connectionString;
            Name = nameNamespace;
        }
    }
}