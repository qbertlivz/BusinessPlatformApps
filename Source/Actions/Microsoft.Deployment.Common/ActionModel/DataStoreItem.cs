using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Common.ActionModel
{
    public class DataStoreItem
    {
        public string Route { get; set; }

        public string Key { get; set; }

        public JToken Value { get; set; }

        public string ValueAsString => Value.ToString();

        public DataStoreType DataStoreType { get; set; }

        public override string ToString()
        {
            return this.ValueAsString;
        }
    }
}