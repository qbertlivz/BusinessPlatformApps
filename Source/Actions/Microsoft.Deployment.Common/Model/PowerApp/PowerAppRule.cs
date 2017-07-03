using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PowerApp
{
    public class PowerAppRule
    {
        [JsonProperty("handler")]
        public PowerAppRuleHandler Handler;
    }
}