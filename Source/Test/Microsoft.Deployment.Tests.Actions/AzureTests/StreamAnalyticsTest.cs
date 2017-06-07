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
    public class StreamAnalyticsTest
    {
        [TestMethod]
        public async Task CreateStreamAnalyticsJob()
        {
            var dataStore = await TestManager.GetDataStore(true);
            dataStore.AddToDataStore("DeploymentName", "LanceDeployment");
            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/StreamAnalytics.json");
            var payload = new JObject();
            payload.Add("name", "LancesStreamAnalyticsJob");
            dataStore.AddToDataStore("AzureArmParameters", payload);

            var deployArmResult = await TestManager.ExecuteActionAsync(
                "Microsoft-DeployAzureArmTemplate", dataStore, "Microsoft-ActivityLogTemplate");
            Assert.IsTrue(deployArmResult.IsSuccess);
     
        }
    }
}
