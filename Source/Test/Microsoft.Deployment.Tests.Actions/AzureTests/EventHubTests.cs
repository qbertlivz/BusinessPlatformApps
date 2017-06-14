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
    public class EventHubTests
    {
        [TestMethod]
        public async Task CreateEventHubNameSpaceTest()
        {
            string id = RandomGenerator.GetRandomCharacters();
            System.Diagnostics.Debug.WriteLine($"id: {id}");
            var dataStore = await TestManager.GetDataStore(true);
            dataStore.AddToDataStore("DeploymentName", $"LanceEHDeployment-{id}");
            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/EventHub.json");
            var payload = new JObject();
            payload.Add("namespaceName", $"LancesEventHubNamespace-{id}");
            payload.Add("eventHubName", $"LancesEventHub-{id}");
            payload.Add("consumerGroupName", "LancesConsumerGroup");
            dataStore.AddToDataStore("AzureArmParameters", payload);
            var deployArmResult = await TestManager.ExecuteActionAsync("Microsoft-CreateEventHubNameSpace", dataStore, "Microsoft-ActivityLogTemplate");
            Assert.IsTrue(deployArmResult.IsSuccess);
        }
    }
}
