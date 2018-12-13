using System.Collections.Generic;

using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PBI
{
    public class PBIImportStatus
    {
        [JsonProperty("createdDateTime")]
        public string CreatedDateTime;
        [JsonProperty("datasets")]
        public List<PBIDataset> Datasets;
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("importState")]
        public string ImportState;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("reports")]
        public List<PBIReport> Reports;
        [JsonProperty("updatedDateTime")]
        public string UpdatedDateTime;
    }
}