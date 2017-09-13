namespace Microsoft.Deployment.Common.Model.APIManagement
{
    public class APIManagementLoggerProperties
    {
        public APIManagementLoggerPropertiesCredentials Credentials;
        public string Description = "BPST Template Logger";
        public bool IsBuffered = true;
        public string LoggerType = "azureEventHub";

        public APIManagementLoggerProperties(string nameEventHub, string connectionString)
        {
            Credentials = new APIManagementLoggerPropertiesCredentials(nameEventHub, connectionString);
        }
    }
}