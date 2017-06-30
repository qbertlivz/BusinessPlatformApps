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

            Assert.IsTrue((await TestManager.ExecuteActionAsync("Microsoft-DeploySQLScripts", ds, "Microsoft-TwitterTemplate")).IsSuccess);

            ds.AddToDataStore("SearchQuery", "cricket rights OR english cricket OR \"Sky AND cricket\"");

            Assert.IsTrue((await TestManager.ExecuteActionAsync("Microsoft-ConfigurePowerAppTables", ds)).IsSuccess);
        }
    }
}