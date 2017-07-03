using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PowerApp
{
    public class PowerAppInvoke
    {
        [JsonProperty("blobURI")]
        public string BlobUri;
        [JsonProperty("docName")]
        public string DocName;
        [JsonProperty("documentSienaUri")]
        public string DocumentSienaUri = "/document.msapp";
        [JsonProperty("smallLogoUri")]
        public string SmallLogoUri = "/logoSmallFile";

        public PowerAppInvoke(string blobUri, string docName)
        {
            BlobUri = blobUri;
            DocName = docName;
        }
    }
}