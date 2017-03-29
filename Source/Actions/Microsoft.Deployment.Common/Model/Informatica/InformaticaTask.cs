using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaTask : InformaticaObject
    {
        [JsonProperty("orgId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrgId;

        [JsonProperty("sourceConnectionId", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceConnectionId;

        [JsonProperty("targetConnectionId", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetConnectionId;

        [JsonProperty("tablePrefix", NullValueHandling = NullValueHandling.Ignore)]
        public string TablePrefix;

        [JsonProperty("bulkApi")]
        public bool BulkApi = true;

        [JsonProperty("removeDeleted")]
        public bool RemoveDeleted = true;

        [JsonProperty("stopOnError")]
        public bool StopOnError = false;

        [JsonProperty("commitInterval")]
        public int CommitInterval = 5000;

        [JsonProperty("scheduleId", NullValueHandling = NullValueHandling.Ignore)]
        public string ScheduleId;

        [JsonProperty("replicationObjects", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ReplicationObjects = null;

        [JsonProperty("errorTaskEmail", NullValueHandling = NullValueHandling.Ignore)]
        public InformaticaTaskEmail ErrorTaskEmail = null;

        [JsonProperty("successTaskEmail", NullValueHandling = NullValueHandling.Ignore)]
        public InformaticaTaskEmail SuccessTaskEmail = null;

        [JsonProperty("warningTaskEmail", NullValueHandling = NullValueHandling.Ignore)]
        public InformaticaTaskEmail WarningTaskEmail = null;

        public InformaticaTask()
        {
            Type = "drsTask";
        }
    }
#pragma warning restore 649
}