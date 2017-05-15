using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaTaskEmail : InformaticaObject
    {
        [JsonProperty("emails", NullValueHandling = NullValueHandling.Ignore)]
        public string Emails;

        [JsonProperty("emailType", NullValueHandling = NullValueHandling.Ignore)]
        public string EmailType;

        [JsonProperty("id")]
        public new int Id;

        public InformaticaTaskEmail()
        {
            Type = "taskEmail";
        }
    }
#pragma warning restore 649
}