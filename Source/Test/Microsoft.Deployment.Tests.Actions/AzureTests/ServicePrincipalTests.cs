using Microsoft.AnalysisServices;
using Microsoft.AnalysisServices.Tabular;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Clients.ActiveDirectory.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ServicePrincipalTests
    {
        [TestMethod]
        public async Task CreateServicePrincipal()
        {
            var dataStore = await TestManager.GetDataStore();
            var response = TestManager.ExecuteAction("Microsoft-CreateSpn", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task LoginServicePrincipalAndConnectToASTest()
        {
            var dataStore = await TestManager.GetDataStore();
            var azureToken = dataStore.GetJson("AzureToken", "access_token");
            AuthenticationContext context = new AuthenticationContext("https://login.windows.net/" + Credential.Instance.AAD.TenantId);
            ClientCredential credential = new ClientCredential("d80958f5-525e-4172-8b57-1a8a71a969b6", "vKAXBuSooqYGbuao+BdqRCxe18vNJz8aynj94y5tc+Xm7Woo6g28QXoySAE=");
            var token = await context.AcquireTokenAsync("cf710c6e-dfcc-4fa8-a093-d47294e44c66", credential);

            string serverName = "asazure://westus.asazure.windows.net/motestdbsdasd";

            Uri test = new Uri(serverName);
            string username = ""; 
            string password = token.AccessToken;

            string connectionString = string.Empty;

            if (serverName.ToLowerInvariant().StartsWith("asazure"))
            {
                connectionString += "Provider=MSOLAP;";
            }

            connectionString += $"Data Source={serverName};";

            //if (!string.IsNullOrEmpty(password))
            //{
                connectionString +=
                    $"Password={password};";
            //}

            try
            {
                ConnectionInfo info = new ConnectionInfo(connectionString);
                
                Server server = new Server();
                server.Connect(connectionString);
                
            }
            catch (Exception ex)
            {
            }

        }
    }
}
