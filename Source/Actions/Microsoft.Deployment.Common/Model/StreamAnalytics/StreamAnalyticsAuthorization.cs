using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.StreamAnalytics
{
    public class StreamAnalyticsAuthorization
    {
        [JsonProperty("action")]
        public string Action;

        [JsonProperty("role")]
        public string Role;

        [JsonProperty("scope")]
        public string Scope;
    }
}
