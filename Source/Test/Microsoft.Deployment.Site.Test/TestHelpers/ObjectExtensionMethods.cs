using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Site.Test.TestHelpers
{
    public static class ObjectExtensionMethods
    {
        public static JObject GetJObject(this object val)
        {
            return JObject.FromObject(val);
        }
    }
}
