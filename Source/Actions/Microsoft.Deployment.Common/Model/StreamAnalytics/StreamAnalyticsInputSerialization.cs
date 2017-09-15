namespace Microsoft.Deployment.Common.Model.StreamAnalytics
{
    public class StreamAnalyticsInputSerialization
    {
        public StreamAnalyticsInputSerializationProperties Properties;
        public string Type = "CSV";

        public StreamAnalyticsInputSerialization()
        {
            Properties = new StreamAnalyticsInputSerializationProperties();
        }
    }
}