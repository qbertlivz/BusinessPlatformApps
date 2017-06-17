using Newtonsoft.Json.Serialization;

namespace Microsoft.Deployment.Common.Helpers
{
    public class JsonUtilityLowercaseSerializer : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        }
    }
}