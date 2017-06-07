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
    public class EventHubTest
    {
        [TestMethod]
        public async Task CreateEventHubNameSpace()
        {
            var dataStore = await TestManager.GetDataStore(true);
            dataStore.AddToDataStore("DeploymentName", "LanceEHDeployment");
            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/EventHub.json");
            var payload = new JObject();
            payload.Add("namespaceName", "LancesEventHubNamespace");
            payload.Add("eventHubName", "Lances-Event-Hub");
            payload.Add("consumerGroupName", "Lances-Consumer-Group");
            dataStore.AddToDataStore("AzureArmParameters", payload);
            var deployArmResult = await TestManager.ExecuteActionAsync(
                "Microsoft-DeployAzureArmTemplate", dataStore, "Microsoft-ActivityLogTemplate");
            Assert.IsTrue(deployArmResult.IsSuccess);
        }
    }
}
