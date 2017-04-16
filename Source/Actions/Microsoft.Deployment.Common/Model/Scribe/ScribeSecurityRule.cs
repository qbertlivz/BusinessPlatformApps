using System;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeSecurityRule : ScribeObject
    {
        public string allowedIpRangeEndAddress = "255.255.255.255";
        public string allowedIpRangeStartAddress = "0.0.0.0";
        public bool apiAccessEnabled = true;
        public bool eventSolutionAccessEnabled = true;
        public string name;

        public ScribeSecurityRule()
        {
            DateTime now = DateTime.Now;
            this.name = $"BPST{now.Year}{now.Month}{now.Day}{now.Hour}{now.Minute}{now.Second}";
        }
    }
}