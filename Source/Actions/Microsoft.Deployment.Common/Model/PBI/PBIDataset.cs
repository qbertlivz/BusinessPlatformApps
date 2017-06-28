using System.Collections.Generic;

using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PBI
{
    public class PBIDataset
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("name")]
        public string Name;
        //[JsonProperty("relationships")]
        //public List<???> Relationships;
        //[JsonProperty("tables")]
        //public List<???> Tables;
        [JsonProperty("webUrl")]
        public string WebUrl;
    }
}