using System.Net;

namespace Microsoft.Deployment.Actions.Salesforce.Helpers
{
    public static class SecurityHelper
    {
        public static void SetTls12()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = true;
        }
    }
}