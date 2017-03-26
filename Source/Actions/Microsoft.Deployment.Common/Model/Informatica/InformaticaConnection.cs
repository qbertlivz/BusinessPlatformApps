using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaConnection : InformaticaObject
    {
        [JsonProperty("orgId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrgId;

        [JsonProperty("agentId")]
        public string AgentId;

        [JsonProperty("runtimeEnvironmentId", NullValueHandling = NullValueHandling.Ignore)]
        public string RuntimeEnvironmentId;

        [JsonProperty("serviceUrl")]
        public string serviceUrl;

        [JsonProperty("type")]
        public string ConnectionType; // "Salesforce" | SqlServer2012

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public int? port;

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string password;

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string username;

        [JsonProperty("securityToken", NullValueHandling = NullValueHandling.Ignore)]
        public string securityToken;

        [JsonProperty("timeout", NullValueHandling = NullValueHandling.Ignore)]
        public int? timeout;

        [JsonProperty("codepage", NullValueHandling = NullValueHandling.Ignore)]
        public string Codepage;

        [JsonProperty("schema", NullValueHandling = NullValueHandling.Ignore)]
        public string Schema;

        [JsonProperty("host", NullValueHandling = NullValueHandling.Ignore)]
        public string Host;

        [JsonProperty("database", NullValueHandling = NullValueHandling.Ignore)]
        public string Database;

        [JsonProperty("authenticationType", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthenticationType;

        public InformaticaConnection()
        {
            Type = "connection";
        }
    }
#pragma warning restore 649
}