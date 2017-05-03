using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class AzureAnalysisServicesTest
    {
        [TestMethod]
        public async Task ErrorMessageValidation()
        {
            // Deploy AS Model based of the following pramaters
            var dataStore = await TestManager.GetDataStore();

            dataStore.AddToDataStore("ASServerName", "Test123");
            var response = await TestManager.ExecuteActionAsync("Microsoft-CheckASServerNameAvailability", dataStore, "Microsoft-TwitterTemplate");
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task DeployASModelTest()
        {
            // Deploy AS Model based of the following pramaters
            var dataStore = await TestManager.GetDataStore();

            // Deploy Twitter Database Scripts
            dataStore.AddToDataStore("SqlConnectionString", SqlCreds.GetSqlPagePayload("modb1"));
            dataStore.AddToDataStore("SqlServerIndex", "0");
            dataStore.AddToDataStore("SqlScriptsFolder", "Database/");

            var response = await TestManager.ExecuteActionAsync("Microsoft-DeploySQLScripts", dataStore, "Microsoft-CRMSalesManagement");
            Assert.IsTrue(response.Status == ActionStatus.Success);

            dataStore.AddToDataStore("ASServerName", "asservermo2345");
            dataStore.AddToDataStore("ASLocation", "westcentralus");
            dataStore.AddToDataStore("ASSku", "D1");

            dataStore.AddToDataStore("xmlaFilePath", "Service/AzureAS/SalesManagement.xmla");
            dataStore.AddToDataStore("ASDatabase", "testdb");

            response = await TestManager.ExecuteActionAsync("Microsoft-DeployAzureAnalysisServices", dataStore, "Microsoft-CRMSalesManagement");
            Assert.IsTrue(response.IsSuccess);

            response = await TestManager.ExecuteActionAsync("Microsoft-ValidateConnectionToAS", dataStore, "Microsoft-CRMSalesManagement");
            Assert.IsTrue(response.IsSuccess);

            //response = await TestManager.ExecuteActionAsync("Microsoft-DeployAzureASModel", dataStore, "Microsoft-CRMSalesManagement");
            //Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task CheckASConnection()
        {
            // Deploy AS Model based of the following pramaters
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore("ASServerUrl", "asazure://westus.asazure.windows.net/test");

            var response = await TestManager.ExecuteActionAsync("Microsoft-ValidateConnectionToAS", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task CheckPermissionCreation()
        {
            // Deploy AS Model based of the following pramaters
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore("ASServerUrl", "asazure://westus.asazure.windows.net/testmo");

            dataStore.AddToDataStore("SqlConnectionString", SqlCreds.GetSqlPagePayload("modb1"));
            dataStore.AddToDataStore("SqlServerIndex", "0");
            dataStore.AddToDataStore("SqlScriptsFolder", "Database/");
            dataStore.AddToDataStore("ASServerName", "testmo");
            dataStore.AddToDataStore("ASLocation", "westcentralus");
            dataStore.AddToDataStore("ASSku", "D1");
            dataStore.AddToDataStore("xmlaFilePath", "Service/AzureAS/SalesManagement.xmla");
            dataStore.AddToDataStore("ASDatabase", "testdb");
            dataStore.AddToDataStore("UserToAdd", "mohaali@microsoft.com");

            var response = await TestManager.ExecuteActionAsync("Microsoft-ValidateConnectionToAS", dataStore);
            Assert.IsTrue(response.IsSuccess);

            response = await TestManager.ExecuteActionAsync("Microsoft-DeployAzureASModel", dataStore, "Microsoft-CRMSalesManagement");
            Assert.IsTrue(response.IsSuccess);

            response = await TestManager.ExecuteActionAsync("Microsoft-AssignPermissionsForUser", dataStore, "Microsoft-CRMSalesManagement");
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
