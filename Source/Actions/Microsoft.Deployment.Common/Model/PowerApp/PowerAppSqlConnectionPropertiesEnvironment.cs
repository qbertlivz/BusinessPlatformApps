using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PowerApp
{
    public class PowerAppSqlConnectionPropertiesEnvironment
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("name")]
        public string Name = "powerAppEnvironment";

        public PowerAppSqlConnectionPropertiesEnvironment(string environmentId)
        {
            Id = $"/providers/Microsoft.PowerApps/environments/{environmentId}";
        }
    }
}