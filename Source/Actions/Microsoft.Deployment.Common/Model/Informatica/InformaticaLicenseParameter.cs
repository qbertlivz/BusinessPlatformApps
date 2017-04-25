using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    class InformaticaLicenseParameter : InformaticaObject
    {
        [JsonProperty("paramName")]
        public string ParamName;

        [JsonProperty("paramValue")]
        public string ParamValue;

        public InformaticaLicenseParameter()
        {
            Type = "licenseParam";
        }
    }
#pragma warning restore 649
}