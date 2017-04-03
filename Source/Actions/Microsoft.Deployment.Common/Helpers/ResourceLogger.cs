using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Enums;

namespace Microsoft.Deployment.Common.Helpers
{
    public class ResourceLogger
    {
        public ResourceLogger(DataStore ds)
        {

        }

        public void LogResource(
             string tenantId,
             string subscriptionId,
             string subscriptionName,
             string rgName,
             string resourceName,
             ResourceType type,
             string createdBy,
             string createdAt,
             string resourceId)
        {
        }
    }
}
