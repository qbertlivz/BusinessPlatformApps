using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
    public class InformaticaObject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id;

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name;

        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type;
    }
}