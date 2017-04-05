using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeAgentInstall : ScribeObject
    {
        [JsonProperty("agentInstallationKey")]
        public string AgentInstallationKey;

        [JsonProperty("agentInstallerLocation")]
        public string AgentInstallerLocation;
    }
}