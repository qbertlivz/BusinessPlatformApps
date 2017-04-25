using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class AzureFunctionsTest
    {
        [TestMethod]
        public async Task CreateAzureFunction()
        {
            var dataStore = await TestManager.GetDataStore();

            //// Deploy Function
            dataStore.AddToDataStore("DeploymentName", "FunctionDeploymentTest");
            dataStore.AddToDataStore("FunctionName", "unittestfunction6");
            dataStore.AddToDataStore("RepoUrl", "https://github.com/MohaaliMicrosoft/AnalysisServicesRefresh");
            dataStore.AddToDataStore("sku", "Standard");

            var response = TestManager.ExecuteAction("Microsoft-DeployAzureFunction", dataStore);
            Assert.IsTrue(response.IsSuccess);
            response = TestManager.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }


        [TestMethod]
        public async Task CreateAzureFunctionForNews()
        {
            var dataStore = await TestManager.GetDataStore();

            //// Deploy Function
            dataStore.AddToDataStore("DeploymentName", "FunctionDeploymentTest");
            dataStore.AddToDataStore("FunctionName", "unittestfunction8");
            dataStore.AddToDataStore("RepoUrl", "https://github.com/juluczni/AzureFunctionsNewsTemplate");

            var response = TestManager.ExecuteAction("Microsoft-DeployAzureFunction", dataStore);
            Assert.IsTrue(response.IsSuccess);
            response = TestManager.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);

            response = TestManager.ExecuteAction("Microsoft-DeployPrivateAssemblyToFunction", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task DeployFunctionAssetsNews()
        {
            var dataStore = await TestManager.GetDataStore();

            //// Deploy Function
            dataStore.AddToDataStore("DeploymentName", "FunctionDeploymentTest");
            dataStore.AddToDataStore("FunctionName", "testfunctionappe");
            dataStore.AddToDataStore("apiKey", "655c7b05d8a04bf6a88647c54fd042e2");

            var response = TestManager.ExecuteAction("Microsoft-DeployNewsFunctionAsset", dataStore);
            Assert.IsTrue(response.IsSuccess);
            response = TestManager.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task HistoricalLogicAppTest()
        {
            var dataStore = await TestManager.GetDataStore();

            //// Deploy Function
            dataStore.AddToDataStore("DeploymentName", "HistoricalAppTest");
            dataStore.AddToDataStore("LogicAppNameHistorical", "LogicAppNameHistorical");
            dataStore.AddToDataStore("SearchQuery", "Microsoft OR Azure");
            dataStore.AddToDataStore("FunctionName", "function1737e9shqv");

            var response = TestManager.ExecuteAction("Microsoft-DeployNewsTemplateHistorical", dataStore);
            Assert.IsTrue(response.IsSuccess);
            response = TestManager.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);

            response = TestManager.ExecuteAction("Microsoft-RunNewsHistorical", dataStore);
            Assert.IsTrue(response.IsSuccess);
            response = TestManager.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

    }
}
