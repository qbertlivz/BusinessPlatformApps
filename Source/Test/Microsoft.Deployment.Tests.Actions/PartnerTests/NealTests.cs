using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Deployment.Tests.Actions.PartnerTests
{
    [TestClass]
    public class NealTests
    {
        [TestMethod]
        public async Task DeployAuditTemplate()
        {
            var dataStore =  await TestManager.GetDataStore();
            dataStore.AddToDataStore("SqlConnectionString", "Server=tcp:modb1.database.windows.net,1433;Initial Catalog=test;Persist Security Info=False;User ID=pbiadmin;Password=Corp123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            dataStore.AddToDataStore("Schema", "ak");
            dataStore.AddToDataStore("SqlServerIndex", "0");
            dataStore.AddToDataStore("SqlScriptsFolder", "Database");
            ActionResponse response = null;

            response = await TestManager.ExecuteActionAsync("Microsoft-DeploySQLScripts", dataStore, "NealAnalytics-OfficeAudit");
            Assert.IsTrue(response.IsSuccess);

            // Testing to see if the tear down works
            response = await TestManager.ExecuteActionAsync("Microsoft-DeploySQLScripts", dataStore, "NealAnalytics-OfficeAudit");
            Assert.IsTrue(response.IsSuccess);
        }
    }
}