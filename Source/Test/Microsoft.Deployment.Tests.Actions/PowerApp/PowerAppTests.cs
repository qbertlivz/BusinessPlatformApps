using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Deployment.Tests.Actions.PowerApp
{
    [TestClass]
    public class PowerAppTests
    {
        [TestMethod]
        public async Task ValidateConfigurePowerAppTables()
        {
            var ds = new DataStore();

            ds.AddToDataStore("SqlConnectionString", SqlCreds.GetSqlPagePayload("ssas"));
            ds.AddToDataStore("SqlScriptsFolder", "Service/Database/LogicApps");

            Assert.IsTrue(await TestManager.IsSuccessAsync("Microsoft-DeploySQLScripts", ds, "Microsoft-TwitterTemplate"));

            ds.AddToDataStore("SearchQuery", "cricket rights OR english cricket OR \"Sky AND cricket\"");

            Assert.IsTrue(await TestManager.IsSuccessAsync("Microsoft-ConfigurePowerAppTables", ds));
        }

        [TestMethod]
        public async Task ValidateGetPowerAppEnvironment()
        {
            var ds =  await TestManager.GetDataStore();

            Assert.IsTrue(await TestManager.IsSuccessAsync("Microsoft-GetPowerAppEnvironment", ds));
        }
    }
}