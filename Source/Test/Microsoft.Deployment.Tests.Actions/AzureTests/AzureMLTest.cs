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
    public class AzureMLTest
    {
        [TestMethod]
        public async Task CreateAMLWebService()
        {
            var dataStore = await TestManager.GetDataStore();

            //// Deploy AML WebService
            dataStore.AddToDataStore("DeploymentName", "AMLDeploymentTest");
            dataStore.AddToDataStore("WebServiceFile", "Service/AzureML/Experiments/TopicImagesWebService.json");
            dataStore.AddToDataStore("WebServiceName", "$run('TopicsImagesWebService' + this.MS.DataStore.getValue('uniqueId'))");
            dataStore.AddToDataStore("CommitmentPlan", "commitmentplan");
            dataStore.AddToDataStore("SelectedResourceGroup", "desktop-424ud00test");
            dataStore.AddToDataStore("StorageAccountName", "storage8kxdl9wil");
            dataStore.AddToDataStore("StorageAccountKey", "h1N4bYA7oQazUMF0PXiE1KjKboR0LoC4l9pOge7UTWGkAQee3sW4Jx3RHc+oMSz60n92UcyCD8VjvGreKeJ7CQ==");
            dataStore.AddToDataStore("IsRequestResponse", true);

            var response = TestManager.ExecuteAction("Microsoft-DeployAzureMLWebServiceFromFile", dataStore);
            Assert.IsTrue(response.IsSuccess);
            response = TestManager.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

    }
}
