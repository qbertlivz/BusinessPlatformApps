using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeAgent : ScribeObject
    {
        [JsonProperty("isCloudAgent")]
        public bool IsCloudAgent;

        [JsonProperty("status")]
        public string Status;
    }
}