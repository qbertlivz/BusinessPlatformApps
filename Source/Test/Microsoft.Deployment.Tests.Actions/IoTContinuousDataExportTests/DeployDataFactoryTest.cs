using System;
using System.Threading.Tasks;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Tests.Actions.IoTContinuousDataExportTests
{
    [TestClass]
    public class DeployDataFactoryTest
    {
        [TestMethod]
        public async Task CreateDataFactory()
        {
            var dataStore = await TestManager.GetDataStore(
                force: true,
                subscriptionId: "479b3e1f-a2af-4b91-b118-02a43df6d293", resourceGroup: "bentestgroup2", region: "eastus");

            // Deploy Data Factory
            dataStore.AddToDataStore("DeploymentName", "DataFactoryDeploymentTest");

            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/datafactory.json");
            dataStore.AddToDataStore("SqlConnectionString", "test connection string");

            JObject functionParametersPrefix = new JObject();
            functionParametersPrefix.Add("factoryName", "test");
            dataStore.AddToDataStore("AzureArmParametersUniquePrefix", functionParametersPrefix);

            var response = TestManager.ExecuteAction("Microsoft-DeployIoTCDEDataFactory", dataStore, "Microsoft-IoTContinuousDataExportTemplate");
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
