namespace Microsoft.Deployment.Common.Model.StreamAnalytics
{
    public class StreamAnalyticsInputPropertiesWrapper
    {
        public StreamAnalyticsInputProperties Properties;

        public StreamAnalyticsInputPropertiesWrapper(string nameEventHub, string nameNamespace, string key)
        {
            Properties = new StreamAnalyticsInputProperties(nameEventHub, nameNamespace, key);
        }
    }
}