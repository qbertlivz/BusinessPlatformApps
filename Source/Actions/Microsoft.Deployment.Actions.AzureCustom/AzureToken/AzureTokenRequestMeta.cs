using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureToken
{
    public class AzureTokenRequestMeta
    {
        public string Resource { get; set; }
        public string ClientId  {get; set; }

        public AzureTokenRequestMeta(string resource, string clientId)
        {
            this.Resource = resource;
            this.ClientId = clientId;
        }
    }
}
