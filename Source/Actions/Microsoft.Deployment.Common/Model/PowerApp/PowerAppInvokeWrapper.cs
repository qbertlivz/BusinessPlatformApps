using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PowerApp
{
    public class PowerAppInvokeWrapper
    {
        [JsonProperty("invoke")]
        public PowerAppInvoke Invoke;

        public PowerAppInvokeWrapper(string blobUri, string docName)
        {
            Invoke = new PowerAppInvoke(blobUri, docName);
        }
    }
}