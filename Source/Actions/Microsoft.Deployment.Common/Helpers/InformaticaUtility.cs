using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.Model.Informatica;

namespace Microsoft.Deployment.Common.Helpers
{
    public class InformaticaUtility
    {
        public const string BPST_SOLUTION_NAME = "BPST Solution";
        public const string BPST_SOURCE_NAME = "BPST Source";
        public const string BPST_TARGET_NAME = "BPST Target";

        private const string CLOUD_AGENT_NAME = "Informatica Cloud Hosted Agent";
        private const string MSG_MISSING_LICENSES = "You do not have the following required licenses: {0}. Contact Informatica Support to obtain them. Alternatively, you can choose to create a new Informatica account as a 90-day trial account.";

        //private const string ENDPOINT_LOGIN = "https://app.informaticaondemand.com";
        private const string ENDPOINT_LOGIN = "https://expo.informaticaondemand.com";

        private const string URL_AGENT = "api/v2/runtimeEnvironment";
        private const string URL_LICENSE_INFO = "api/v2/licenseInfo";
        private const string URL_LOGIN = "ma/api/v2/user/login";

        public static async Task<RestClient> Initialize(string username, string password)
        {
            RestClient rc = new RestClient(ENDPOINT_LOGIN);

            InformaticaLogin informaticaLogin = new InformaticaLogin
            {
                UserName = username,
                Password = password
            };

            string response = await rc.Post(URL_LOGIN, JsonConvert.SerializeObject(informaticaLogin));
            InformaticaUser u = JsonConvert.DeserializeObject<InformaticaUser>(response);

            rc = new RestClient(u.ServerUrl[u.ServerUrl.Length - 1] == '/' ? u.ServerUrl : u.ServerUrl + '/', null, new Dictionary<string, string> { { "icSessionId", u.IcSessionId } }, u.OrgId);

            InformaticaOrganizationLicenseInformation li = await GetLicensingInformation(rc);

            Dictionary<string, bool> licenses = new Dictionary<string, bool>
            {
                {"Salesforce Connector", false},
                {"Data Replication Service", false},
                {"Cloud Runtime", false},
                {"REST API", false},
            };

            int licenseCount = 0;
            foreach (InformaticaLicense t in li.LicenseInformation.Licenses)
            {
                switch (t.LicenseName.ToUpperInvariant())
                {
                    case "SALESFORCE":
                        licenses["Salesforce Connector"] = true;
                        licenseCount++;
                        break;
                    case "DRS":
                        licenses["Data Replication Service"] = true;
                        licenseCount++;
                        break;
                    case "ENVIRONMENT_CLOUD_AGENT":
                        licenses["Cloud Runtime"] = true;
                        licenseCount++;
                        break;
                    case "REST_API":
                        licenses["REST API"] = true;
                        licenseCount++;
                        break;
                }
            }

            if (licenseCount >= 4) return rc; // All is well, we found the licenses needed!

            string licensesNeeded = null;
            foreach (var k in licenses.Keys)
            {
                if (!licenses[k])
                    licensesNeeded += licensesNeeded == null ? k : ", " + k;
            }

            throw new Exception(string.Format(CultureInfo.InvariantCulture, MSG_MISSING_LICENSES, licensesNeeded));
        }

        public static async Task<string> GetRuntimeEnvironmentId(RestClient rc)
        {
            string response = await rc.Get(URL_AGENT);
            InformaticaRuntimeEnvironment[] environments = JsonConvert.DeserializeObject<InformaticaRuntimeEnvironment[]>(response);

            string id = null;
            for (int i = 0; i < environments.Length; i++)
            {
                if (environments[i].Name.EqualsIgnoreCase(CLOUD_AGENT_NAME))
                {
                    id = environments[i].Id;
                    break;
                }
            }

            return id;
        }

        private static async Task<InformaticaOrganizationLicenseInformation> GetLicensingInformation(RestClient rc)
        {
            string response = await rc.Get(URL_LICENSE_INFO);
            return JsonConvert.DeserializeObject<InformaticaOrganizationLicenseInformation>(response);
        }
    }
}