using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaConnectionParameters
    {
        [JsonProperty("trustStorePassword", NullValueHandling = NullValueHandling.Ignore)]
        public string TrustStorePassword = string.Empty;

        [JsonProperty("CryptoProtocolVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string CryptoProtocolVersion = "TLSv1";

        [JsonProperty("trustStore", NullValueHandling = NullValueHandling.Ignore)]
        public string TrustStore = string.Empty;

        [JsonProperty("HostNameInCertificate", NullValueHandling = NullValueHandling.Ignore)]
        public string HostNameInCertificate = "";

        [JsonProperty("encryptionMethod", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptionMethod = "SSL";

        [JsonProperty("ValidateServerCertificate", NullValueHandling = NullValueHandling.Ignore)]
        public string ValidateServerCertificate = "False";
    }
#pragma warning restore 649
}