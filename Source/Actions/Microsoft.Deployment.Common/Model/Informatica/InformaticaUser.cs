using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaUser : InformaticaObject
    {
        [JsonProperty("orgId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrgId;

        [JsonProperty("icSessionId", NullValueHandling = NullValueHandling.Ignore)]
        public string IcSessionId;

        [JsonProperty("serverUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerUrl;

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password;

        [JsonProperty("emails", NullValueHandling = NullValueHandling.Ignore)]
        public string Emails;

        public InformaticaUser()
        {
            Type = "user";
        }
    }
#pragma warning restore 649
}