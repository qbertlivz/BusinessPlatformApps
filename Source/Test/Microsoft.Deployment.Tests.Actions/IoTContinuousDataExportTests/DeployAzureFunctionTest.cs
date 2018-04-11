using System;
using System.Threading.Tasks;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Tests.Actions.IoTContinuousDataExportTests
{
    [TestClass]
    public class DeployAzureFunctionTest
    {
        [TestMethod]
        public async Task CreateAzureFunction()
        {
            var dataStore = await TestManager.GetDataStore(subscriptionId: "479b3e1f-a2af-4b91-b118-02a43df6d293", resourceGroup: "bentestgroup3");

            // Deploy Function
            dataStore.AddToDataStore("DeploymentName", "FunctionDeploymentTest");

            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/functions.json");
            dataStore.AddToDataStore("SqlConnectionString", "test connection string");

            /*
            "siteName": "continuousdataexport",
            "sku": "Standard",
            "workerSize": "Small",
            "storage": "DefaultEndpointsProtocol=https;AccountName=jeffscratch;AccountKey=QRYrElr9AnKXNvP7MXUHA4E0IjkyeBU3n64jKjyZcOjkE8MDNSH6BJ2sOPgTrOpfErSPUIf6xIrrO03I9uD4Lg=="
            */

            JObject functionParameters = new JObject();
            functionParameters.Add("blobStorageConnectionString", "DefaultEndpointsProtocol=https;AccountName=jeffscratch;AccountKey=QRYrElr9AnKXNvP7MXUHA4E0IjkyeBU3n64jKjyZcOjkE8MDNSH6BJ2sOPgTrOpfErSPUIf6xIrrO03I9uD4Lg==");
            functionParameters.Add("sku", "Standard");
            functionParameters.Add("workerSize", "Small");
            dataStore.AddToDataStore("AzureArmParameters", functionParameters);

            JObject functionParametersPrefix = new JObject();
            functionParametersPrefix.Add("siteName", "-continuousdataexport");
            functionParametersPrefix.Add("functionStorage", "function");
            dataStore.AddToDataStore("AzureArmParametersUniquePrefix", functionParametersPrefix);

            var response = TestManager.ExecuteAction("Microsoft-DeployIoTCDEAzureFunction", dataStore, "Microsoft-IoTContinuousDataExportTemplate");
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task DeployFunctionCode()
        {
            var dataStore = await TestManager.GetDataStore(subscriptionId: "479b3e1f-a2af-4b91-b118-02a43df6d293", resourceGroup: "bentestgroup2");

            // Deploy Function
            dataStore.AddToDataStore("DeploymentName", "FunctionDeploymentTest");

            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/functions.json");
            dataStore.AddToDataStore("SqlConnectionString", "test connection string");

            /*
            "siteName": "continuousdataexport",
            "sku": "Standard",
            "workerSize": "Small",
            "storage": "DefaultEndpointsProtocol=https;AccountName=jeffscratch;AccountKey=QRYrElr9AnKXNvP7MXUHA4E0IjkyeBU3n64jKjyZcOjkE8MDNSH6BJ2sOPgTrOpfErSPUIf6xIrrO03I9uD4Lg=="
            */

            JObject functionParameters = new JObject();
            functionParameters.Add("blobStorageConnectionString", "DefaultEndpointsProtocol=https;AccountName=jeffscratch;AccountKey=QRYrElr9AnKXNvP7MXUHA4E0IjkyeBU3n64jKjyZcOjkE8MDNSH6BJ2sOPgTrOpfErSPUIf6xIrrO03I9uD4Lg==");
            functionParameters.Add("sku", "Standard");
            functionParameters.Add("workerSize", "Small");
            dataStore.AddToDataStore("AzureArmParameters", functionParameters);

            JObject functionParametersPrefix = new JObject();
            functionParametersPrefix.Add("siteName", "-continuousdataexport");
            functionParametersPrefix.Add("functionStorage", "function");
            dataStore.AddToDataStore("AzureArmParametersUniquePrefix", functionParametersPrefix);

            var response = TestManager.ExecuteAction("Microsoft-DeployIoTCDEAzureFunction", dataStore, "Microsoft-IoTContinuousDataExportTemplate");
            Assert.IsTrue(response.IsSuccess);

            dataStore.AddToDataStore("ZipFile", "Service/Data/functions.zip");
            response = TestManager.ExecuteAction("Microsoft-DeployIoTCDEFunctionCode", dataStore, "Microsoft-IoTContinuousDataExportTemplate");
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
