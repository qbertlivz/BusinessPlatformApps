using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PBI
{
    public class PBIWorkspace
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("isReadOnly")]
        public string IsReadOnly;
        [JsonProperty("name")]
        public string Name;

        public PBIWorkspace(string name)
        {
            this.Id = string.Empty;
            this.Name = name;
        }
    }
}