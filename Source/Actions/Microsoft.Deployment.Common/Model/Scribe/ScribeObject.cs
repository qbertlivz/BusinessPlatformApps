using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeObject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id;

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name;
    }
}