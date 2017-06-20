using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ConfigureStreamAnalyticsTest
    {
        public DataStore dataStore;

        [TestMethod]
        public async Task CreateStreamAnalyticsJobTest()
        {
            string id = RandomGenerator.GetRandomCharacters();
            System.Diagnostics.Debug.WriteLine($"id: {id}");
            var dataStore = await TestManager.GetDataStore(true);
            dataStore.AddToDataStore("DeploymentName", $"LanceSADeployment-{id}");
            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/StreamAnalytics.json");
            var payload = new JObject();
            payload.Add("name", $"LancesStreamAnalyticsJob-{id}");
            dataStore.AddToDataStore("AzureArmParameters", payload);

            var deployArmResult = await TestManager.ExecuteActionAsync(
                "Microsoft-CreateStreamAnalyticsJob", dataStore, "Microsoft-ActivityLogTemplate");
            Assert.IsTrue(deployArmResult.IsSuccess);

        }
        public void SetupDataStoreWithEventHubKey()
        {

            dataStore = TestManager.GetDataStore().Result;
            dataStore.AddToDataStore("apiVersion", "2014-09-01");
            dataStore.AddToDataStore("namespace", "LancesEventHubNamespace-twq5uxxrAq2aimv");
            var response = TestManager.ExecuteActionAsync("Microsoft-GetEventHubPrimaryKey", dataStore).Result;
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task SetStreamAnalyticsInputTest()
        {
            SetupDataStoreWithEventHubKey();
            dataStore.AddToDataStore("inputAlias", "new-input");
            var response = await TestManager.ExecuteActionAsync("Microsoft-SetStreamAnalyticsInputAsEventHub", dataStore);

            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task SetStreamAnalyticsQueryTest()
        {
            var dataStore = await TestManager.GetDataStore(true);
            dataStore.AddToDataStore("inputAlias", "new-input17");
            dataStore.AddToDataStore("outputAlias", "PBI-Output");
            dataStore.AddToDataStore("transformationName", "testQuery");
            dataStore.AddToDataStore("jobName", "LancesStreamAnalyticsJob");
            var response = await TestManager.ExecuteActionAsync("Microsoft-SetStreamAnalyticsQuery", dataStore);

            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task StartStreamAnalyticsJobTest()
        {
            var dataStore = await TestManager.GetDataStore(true);
            dataStore.AddToDataStore("jobName", "LancesStreamAnalyticsJob");
            var response = await TestManager.ExecuteActionAsync("Microsoft-StartStreamAnalyticsJob", dataStore);
            Assert.IsTrue(response.IsSuccess);

        }
    }
}
