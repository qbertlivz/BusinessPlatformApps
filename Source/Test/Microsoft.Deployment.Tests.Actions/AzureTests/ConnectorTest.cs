using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Deployment.Common.ActionModel;
using System.Dynamic;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ConnectorTest
    {
        [TestMethod]
        public async Task CreateConnector()
        {
            var dataStore = await TestManager.GetDataStore();

            var response = TestManager.ExecuteAction("Microsoft-CreateTwitterConnectionToLogicApp", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }
    }
}