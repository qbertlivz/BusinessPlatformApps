using System.Collections.Generic;

using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PBI
{
    public class PBIReport
    {
        [JsonProperty("embedUrl")]
        public string EmbedUrl;
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("isFromPbix")]
        public bool IsFromPbix;
        [JsonProperty("isOriginalPbixReport")]
        public bool IsOriginalPbixReport;
        [JsonProperty("modelId")]
        public string ModelId;
        [JsonProperty("name")]
        //[JsonProperty("pages")]
        //public ??? Pages;
        public string Name;
        [JsonProperty("webUrl")]
        public string WebUrl;
    }
}