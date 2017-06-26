using System;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeHistory
    {
        public string Details;
        public string Id;
        public bool IsReprocessJob;
        public int RecordsFailed;
        public int RecordsProcessed;
        public int ReprocessRecordsRemaining;
        public string Result;
        public DateTime Start;
        public DateTime Stop;
    }
}