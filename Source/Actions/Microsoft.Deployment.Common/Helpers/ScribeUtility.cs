using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Common.Helpers
{
    public class ScribeUtility
    {
        public const string BPST_SOLUTION_NAME = "BPST Solution";
        public const string BPST_SOURCE_NAME = "BPST Source";
        public const string BPST_TARGET_NAME = "BPST Target";
        public const string REPLICATION_SERVICES = "Replication Services (RS)";
        public const string URL_AGENTS = "/v1/orgs/{0}/agents";
        public const string URL_CONNECTION = "/v1/orgs/{0}/connections/{1}";
        public const string URL_CONNECTIONS = "/v1/orgs/{0}/connections";
        public const string URL_CONNECTORS = "/v1/orgs/{0}/connectors";
        public const string URL_CONNECTORSINSTALL = "/v1/orgs/{0}/connectors/{1}/install";
        public const string URL_ENDPOINT = "https://api.scribesoft.com";
        public const string URL_HISTORY = "/v1/orgs/{0}/solutions/{1}/history";
        public const string URL_ORGANIZATIONS = "/v1/orgs";
        public const string URL_PROVISION_CLOUD_AGENT = "/v1/orgs/{0}/agents/provision_cloud_agent";
        public const string URL_PROVISION_ONPREMISE_AGENT = "/v1/orgs/{0}/agents/provision_onpremise_agent";
        public const string URL_SECURITY_RULES = "/v1/orgs/{0}/securityrules";
        public const string URL_SOLUTION = "/v1/orgs/{0}/solutions/{1}";
        public const string URL_SOLUTION_PROCESS = "v1/orgs/{0}/solutions/{1}/start";
        public const string URL_SOLUTION_SCHEDULE = "/v1/orgs/{0}/solutions/{1}/schedule";
        public const string URL_SOLUTIONS = "/v1/orgs/{0}/solutions";
        public const string URL_SUBSCRIPTIONS = "/v1/orgs/{0}/subscriptions";

        public static RestClient Initialize(string username, string password)
        {
            return new RestClient(URL_ENDPOINT, new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Concat(username, ":", password)))));
        }

        public static string AesEncrypt(string apiToken, string message)
        {
            const string salt = "ac103458-fcb6-41d3-94r0-43d25b4f4ff4";
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            string result = null;

            // Setup
            using (var aes = new AesManaged())
            {
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.IV = new byte[aes.BlockSize / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(aes.IV);
                }

                aes.Padding = PaddingMode.PKCS7;

                // PBKDF2 standard with HMACSHA1 for password-based key generation
                using (var rfcDerivative = new Rfc2898DeriveBytes(apiToken, saltBytes))
                {
                    aes.Key = rfcDerivative.GetBytes(aes.KeySize / 8);
                }

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            // Convert the passed string to UTF8 as a byte array
                            var messageAsBytes = Encoding.UTF8.GetBytes(message);

                            // Write the iv + cipherText array to the crypto stream and flush it
                            cryptoStream.Write(messageAsBytes, 0, messageAsBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            // Get an array of bytes from the MemoryStream that holds the encrypted data
                            var encryptedBytes = memoryStream.ToArray();
                            result = Convert.ToBase64String(aes.IV) + Convert.ToBase64String(encryptedBytes);
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<string> GetSolutionId(RestClient rc, string orgId, string name)
        {
            List<ScribeSolution> solutions = await ScribeUtility.GetSolutions(rc, orgId);

            foreach (ScribeSolution solution in solutions)
            {
                if (name == solution.Name)
                {
                    return solution.Id;
                }
            }

            return null;
        }

        public static async Task<List<ScribeSolution>> GetSolutions(RestClient rc, string orgId)
        {
            return JsonUtility.Deserialize<List<ScribeSolution>>(await rc.Get(string.Format(CultureInfo.InvariantCulture, ScribeUtility.URL_SOLUTIONS, orgId), null, null));
        }

        public static async Task InstallConnector(RestClient rc, string orgId, string connectorId)
        {
            List<ScribeConnector> connectors = JsonUtility.Deserialize<List<ScribeConnector>>(await rc.Get(string.Format(URL_CONNECTORS, orgId)));

            if (!connectors.Any(p => p.Id.EqualsIgnoreCase(connectorId)))
            {
                await rc.Post(string.Format(CultureInfo.InvariantCulture, URL_CONNECTORSINSTALL, orgId, connectorId), string.Empty, null, null);
            }
        }
    }
}