using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaAgent : InformaticaObject
    {
        // Organization ID
        [JsonProperty("orgId")]
        public string OrgId;

        [JsonProperty("description")]
        public string Description;

        // Whether the Secure Agent is active.Returns one of the following values: {true Active} {false Inactive}
        [JsonProperty("active")]
        public bool Active;

        // Platform of the Secure Agent machine.Returns one of the following values: win32, linux32, win64, linux64
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public string Platform;

        // Host name of the Secure Agent machine.
        [JsonProperty("agentHost", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentHost;

        // Password of the Secure Agent machine.
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password;

        // Agent version.
        [JsonProperty("agentVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentVersion;

        // Upgrade status.
        [JsonProperty("upgradeStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string UpgradeStatus;

        // Upgrade status.
        [JsonProperty("runtimeEnvironmentId", NullValueHandling = NullValueHandling.Ignore)]
        public string RuntimeEnvironmentId;

        public InformaticaAgent()
        {
            Type = "agent";
        }

    }
#pragma warning restore 649
}