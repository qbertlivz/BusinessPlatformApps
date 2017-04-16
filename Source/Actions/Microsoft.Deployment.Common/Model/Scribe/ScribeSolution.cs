namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeSolution : ScribeObject
    {
        public string AgentId;
        public string ConnectionIdForSource;
        public string ConnectionIdForTarget;
        public string SolutionType;
        public string status;
        public string Description;

        public ScribeReplicationSettings ReplicationSettings;
    }
}