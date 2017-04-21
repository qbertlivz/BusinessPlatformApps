using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Deployment.Common.ActionModel;
using System.Dynamic;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ConnectorTest
    {
        [TestMethod]
        public async Task CreateConnector()
        {
            var dataStore = await TestManager.GetDataStore();

            var response = TestManager.ExecuteAction("Microsoft-CreateTwitterConnectionToLogicApp", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public async Task CreateSqlConnector()
        {
            var dataStore = await TestManager.GetDataStore();

            var response = TestManager.ExecuteAction("Microsoft-CreateSqlConnector", dataStore);

            dataStore.AddToDataStore("AzureArmFile", "Service/Notifier/notifierLogicApp.json");

            dynamic AzureArmParameters = new ExpandoObject();
            AzureArmParameters.notifierLogicApp = "notifierLogicApp";
            AzureArmParameters.sqlConnection = dataStore.GetValue("sqlConnectionName");
            AzureArmParameters.resourcegroup = dataStore.GetValue("SelectedResourceGroup");
            AzureArmParameters.subscription = dataStore.GetJson("SelectedSubscription")["SubscriptionId"];

            dataStore.AddToDataStore("AzureArmParameters", JsonUtility.GetJsonStringFromObject(AzureArmParameters));

            response = TestManager.ExecuteAction("Microsoft-DeployAzureArmTemplate", dataStore);

            Assert.IsTrue(response.IsSuccess);
        }

    }
}