using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ExportActivityLogToEventHubTest
    {
        [TestMethod]
        public async Task LinkActivityLogToEventHubTest()
        {
            string id = RandomGenerator.GetRandomCharacters();
            var dataStore = await TestManager.GetDataStore();

            dataStore.AddToDataStore("relativeUrl", "providers/microsoft.insights/logprofiles/default");
            dataStore.AddToDataStore("apiVersion", "2016-03-01");
            // dataStore.AddToDataStore("namespace", $"LancesEventHubNamespace-{id}");
            dataStore.AddToDataStore("namespace", "LancesEventHubNamespace-twq5uxxrAq2aimv");
            var response = TestManager.ExecuteAction("Microsoft-ExportActivityLogToEventHub", dataStore);

            Assert.IsTrue(response.IsSuccess);
        }
    }
}
