using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaActivityLog : InformaticaObject
    {
        [JsonProperty("objectId", NullValueHandling = NullValueHandling.Ignore)]
        public string TaskId;

        [JsonProperty("objectName", NullValueHandling = NullValueHandling.Ignore)]
        public string TaskName;

        [JsonProperty("runId", NullValueHandling = NullValueHandling.Ignore)]
        public string RunId;

        [JsonProperty("agentId")]
        public string AgentId;

        [JsonProperty("runtimeEnvironmentId", NullValueHandling = NullValueHandling.Ignore)]
        public string RuntimeEnvironmentId;

        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public string StartTime;

        [JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore)]
        public string EndTime;

        [JsonProperty("startTimeUtc", NullValueHandling = NullValueHandling.Ignore)]
        public string StartTimeUtc;

        [JsonProperty("endTimeUtc", NullValueHandling = NullValueHandling.Ignore)]
        public string EndTimeUtc;

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State;

        [JsonProperty("executionState", NullValueHandling = NullValueHandling.Ignore)]
        public string ExecutionState;

        [JsonProperty("failedSourceRows", NullValueHandling = NullValueHandling.Ignore)]
        public string FailedSourceRows;

        [JsonProperty("successSourceRows", NullValueHandling = NullValueHandling.Ignore)]
        public string SuccessSourceRows;

        [JsonProperty("failedTargetRows", NullValueHandling = NullValueHandling.Ignore)]
        public string FailedTargetRows;

        [JsonProperty("successTargetRows", NullValueHandling = NullValueHandling.Ignore)]
        public string SuccessTargetRows;

        [JsonProperty("errorMsg", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMsg;

        [JsonProperty("startedBy", NullValueHandling = NullValueHandling.Ignore)]
        public string StartedBy;

        [JsonProperty("runContextType", NullValueHandling = NullValueHandling.Ignore)]
        public string RunContextType;

        [JsonProperty("scheduleName", NullValueHandling = NullValueHandling.Ignore)]
        public string ScheduleName;
    }
#pragma warning restore 649
}