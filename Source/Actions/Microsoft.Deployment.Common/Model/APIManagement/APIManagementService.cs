using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.APIManagement
{
    public class APIManagementService
    {
        [JsonProperty("etag")]
        public string ETag;
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("location")]
        public string Location;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("properties")]
        public APIManagementServiceProperties Properties;
        [JsonProperty("sku")]
        public APIManagementServiceSku Sku;
        [JsonProperty("type")]
        public string Type;
    }
}