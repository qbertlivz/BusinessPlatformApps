using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaJob : InformaticaObject
    {
        [JsonProperty("taskId", NullValueHandling = NullValueHandling.Ignore)]
        public string TaskId;

        [JsonProperty("taskName", NullValueHandling = NullValueHandling.Ignore)]
        public string TaskName;

        [JsonProperty("taskType", NullValueHandling = NullValueHandling.Ignore)]
        public string TaskType;

        [JsonProperty("runId", NullValueHandling = NullValueHandling.Ignore)]
        public string RunId;

        [JsonProperty("callbackURL", NullValueHandling = NullValueHandling.Ignore)]
        public string CallbackURL;

        public InformaticaJob()
        {
            Type = "job";
        }
    }
#pragma warning restore 649
}