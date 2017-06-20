using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Tests.Actions
{
    [TestClass]
    public class SalesforceUnitTests
    {
        [TestMethod]
        public void GetAllObjects()
        {
            var ds = new DataStore();
            ds.AddToDataStore("ObjectTables", "");
            ds.AddToDataStore("SalesforceUser", Credential.Instance.Salesforce.Username);
            ds.AddToDataStore("SalesforcePassword", Credential.Instance.Salesforce.Password);
            ds.AddToDataStore("SalesforceToken", Credential.Instance.Salesforce.Token);
            ds.AddToDataStore("SalesforceUrl", "https://login.salesforce.com");

            var resp = TestManager.ExecuteAction("Microsoft-SalesforceGetEntities", ds);
        }
    }
}
