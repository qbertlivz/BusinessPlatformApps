using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    class InformaticaOrganizationEdition
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("expirationDate")]
        public string ExpirationDate;
        [JsonProperty("overwriteCustomLicenses")]
        public bool OverwriteCustomLicenses;
    }
#pragma warning restore 649
}