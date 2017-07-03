using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PowerApp
{
    public class PowerAppRuleWrapper
    {
        [JsonProperty("dcall:irule/8001/subscribetoinvalidated")]
        public PowerAppRule Rule;
    }
}