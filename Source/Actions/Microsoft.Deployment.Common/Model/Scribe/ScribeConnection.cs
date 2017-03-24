using System.Collections.Generic;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeConnection : ScribeObject
    {
        public string Alias = "";
        public string Color;
        public string ConnectorId;

        public List<ScribeKeyValue> Properties;
    }
}