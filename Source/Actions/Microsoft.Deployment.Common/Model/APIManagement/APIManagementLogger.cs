using Microsoft.Deployment.Common.Model.Bpst;

namespace Microsoft.Deployment.Common.Model.APIManagement
{
    public class APIManagementLogger
    {
        public string Id;
        public string Name;
        public APIManagementLoggerProperties Properties;
        public string Type = "Microsoft.ApiManagement/service/loggers";

        public APIManagementLogger(string idApimService, string id, string nameEventHub, string connectionString)
        {
            Id = $"{idApimService}/loggers/{id}";
            Name = id;
            Properties = new APIManagementLoggerProperties(nameEventHub, connectionString);
        }
    }
}