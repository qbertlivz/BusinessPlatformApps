using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeOrganization : ScribeObject
    {
        [JsonProperty("apiToken")]
        public string ApiToken;
    }
}