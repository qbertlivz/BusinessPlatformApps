using System;
using System.Threading.Tasks;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Tests.Actions.IoTContinuousDataExportTests
{
    [TestClass]
    public class GetAzureStoragesTest
    {
        [TestMethod]
        public async Task GetAzureStorages()
        {
            var dataStore = await TestManager.GetDataStore(subscriptionId: "479b3e1f-a2af-4b91-b118-02a43df6d293", resourceGroup: "bentestgroup3");

            var response = TestManager.ExecuteAction("Microsoft-GetAzureStorages", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
