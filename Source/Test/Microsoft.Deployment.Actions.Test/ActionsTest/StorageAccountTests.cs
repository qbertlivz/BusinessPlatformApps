using Microsoft.Deployment.Actions.Test.TestHelpers;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.Helpers;
namespace Microsoft.Deployment.Actions.Test.ActionsTest
{
    [TestClass]
    public class StorageAccountTests
    {
        public static string randomString = RandomGenerator.GetRandomLowerCaseCharacters(5);

        [TestMethod]
        public async Task DeployAzureStorageAccount()
        {
            var dataStore = await TestHarness.GetCommonDataStoreWithUserToken();

            dataStore.AddToDataStore("DeploymentName", "StorageDeploymentTest");
            dataStore.AddToDataStore("StorageAccountName", "unittesttrialbpst" + randomString);
            dataStore.AddToDataStore("StorageAccountType", "Standard_LRS");
            dataStore.AddToDataStore("StorageAccountEncryptionEnabled", "false");

            var response = TestHarness.ExecuteAction("Microsoft-CreateAzureStorageAccount", dataStore);
            Assert.IsTrue(response.IsSuccess);

            response = TestHarness.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task GetStorageAccountKey()
        {
            await this.DeployAzureStorageAccount();
            var dataStore = await TestHarness.GetCommonDataStoreWithUserToken();

            dataStore.AddToDataStore("DeploymentName", "StorageDeploymentTest");
            dataStore.AddToDataStore("StorageAccountName", "unittesttrialbpst" + randomString);

            var response = TestHarness.ExecuteAction("Microsoft-GetStorageAccountKey", dataStore);
            Assert.IsTrue(response.IsSuccess);

            response = TestHarness.ExecuteAction("Microsoft-WaitForArmDeploymentStatus", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task DeployStorageAccountBlob()
        {

            var dataStore = await TestHarness.GetCommonDataStoreWithUserToken();

            dataStore.AddToDataStore("StorageAccountKey", "1zwf5K6duNcivYmIs+xjKp1Pqdd/uR3ZslO/H2SUreY/v275VnuPATeKOX9NqbfMGMeROyFx+Xl9vBsQDWuKlw==");
            dataStore.AddToDataStore("StorageAccountName", "solutiontemplateazgyzfds");
            dataStore.AddToDataStore("StorageAccountContainer", "mycontainer" + randomString);
            dataStore.AddToDataStore("DeploymentName", "mydeployment");


            var response = TestHarness.ExecuteAction("Microsoft-DeployStorageAccountContainer", dataStore);
            Assert.IsTrue(response.IsSuccess);

        }

    }
}
