using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.APIManagement
{
    public class APIManagementServiceSku
    {
        [JsonProperty("capacity")]
        public int Capacity;
        [JsonProperty("name")]
        public string Name;
    }
}