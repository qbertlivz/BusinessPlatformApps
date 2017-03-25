using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    class InformaticaOrganizationLicenseInformation : InformaticaObject
    {
        [JsonProperty("orgId")]
        public string OrgId;

        [JsonProperty("orgEdition")]
        public InformaticaOrganizationEdition OrganizationEdition;

        [JsonProperty("licenseInfo")]
        public InformaticaLicenseInformation LicenseInformation;

        public InformaticaOrganizationLicenseInformation()
        {
            Type = "orgLicenseInfo";
        }
    }
#pragma warning restore 649
}