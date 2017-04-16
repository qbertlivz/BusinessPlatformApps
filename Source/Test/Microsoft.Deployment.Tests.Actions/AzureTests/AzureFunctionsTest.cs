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
    }
}
