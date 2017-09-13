using Microsoft.Deployment.Common.Model.Bpst;

namespace Microsoft.Deployment.Common.Model.APIManagement
{
    public class APIManagementLogger
    {
        public string Id;
        public string Name;
        public APIManagementLoggerProperties Properties;
        public string Type = "Microsoft.ApiManagement/service/loggers";

        public APIManagementLogger(BpstAzure ba, string nameService, string id, string nameNamespace, string connectionString)
        {
            Id = $"/subscriptions/{ba.IdSubscription}/resourceGroups/{ba.NameResourceGroup}/providers/Microsoft.ApiManagement/service/{nameService}/loggers/{id}";
            Name = id;
            Properties = new APIManagementLoggerProperties(nameNamespace, connectionString);
        }
    }
}