using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PowerApp
{
    public class PowerAppSession
    {
        [JsonProperty("sessionToken")]
        public string SessionToken;
    }
}