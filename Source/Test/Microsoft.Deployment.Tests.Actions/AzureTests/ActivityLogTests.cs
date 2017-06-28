using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ActivityLogTests
    {
        [TestMethod]
        public async Task GetHistoricalDataTest()
        {
            // Tests the Action to create an Event Hub Namespace
            var dataStore = await TestManager.GetDataStore(true);
            var response = await TestManager.ExecuteActionAsync("Microsoft-GetHistoricalData", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
