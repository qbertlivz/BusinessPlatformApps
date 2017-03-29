using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    class InformaticaLicense : InformaticaObject
    {
        [JsonProperty("licenseName")]
        public string LicenseName;

        [JsonProperty("licenseCategory")]
        public string LicenseCategory;

        [JsonProperty("licenseType")]
        public string LicenseType;

        [JsonProperty("expirationDate")]
        public string ExpirationDate;

        [JsonProperty("childLicenses")]
        public InformaticaLicense[] ChildLicenses;

        [JsonProperty("licenseParams")]
        public InformaticaLicenseParameter[] LicenseParams;

        public InformaticaLicense()
        {
            Type = "license";
        }
    }
#pragma warning restore 649
}