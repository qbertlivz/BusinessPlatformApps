using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ConfigureStreamAnalyticsTest
    {
        public DataStore dataStore;
        public async void SetupDataStoreWithEventHubKey()
        {
           
            this.dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore("apiVersion", "2014-09-01");
            dataStore.AddToDataStore("namespace", "LancesEventHubNamespace-twq5uxxrAq2aimv");
            var response = await TestManager.ExecuteActionAsync("Microsoft-GetEventHubPrimaryKey", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task SetStreamAnalyticsInputTest()
        {
            SetupDataStoreWithEventHubKey();
            dataStore.AddToDataStore("StreamAnalyticsInputAlias", "eh-input");
            var response = await TestManager.ExecuteActionAsync("Microsoft-SetStreamAnalyticsInputAsEventHub", dataStore);

            Assert.IsTrue(response.IsSuccess);
        }
    }
}
