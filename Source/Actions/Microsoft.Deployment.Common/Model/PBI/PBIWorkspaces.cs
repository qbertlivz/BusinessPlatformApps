using System.Collections.Generic;

using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.PBI
{
    public class PBIWorkspaces
    {
        [JsonProperty("@odata.context")]
        public string ODataContext;
        [JsonProperty("value")]
        public List<PBIWorkspace> Workspaces;
    }
}