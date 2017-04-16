using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    class InformaticaLicenseInformation : InformaticaObject
    {
        [JsonProperty("licenses")]
        public InformaticaLicense[] Licenses;

        public InformaticaLicenseInformation()
        {
            Type = "licenseInfo";
        }
    }
#pragma warning restore 649
}