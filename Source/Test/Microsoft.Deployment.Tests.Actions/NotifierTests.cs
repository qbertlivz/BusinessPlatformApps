using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Deployment.Tests.Actions
{
    [TestClass]
    public class NotifierTests
    {
        [TestMethod]
        public void SalesforceGetInitialCounts()
        {
            var dataStore = new DataStore();
            var json = "{\"account\":1000, \"opportunity\":1000}";

            dataStore.AddToDataStore("NotificationThresholds", JsonUtility.GetJsonObjectFromJsonString(json));

            var resp = TestManager.ExecuteAction("Microsoft-SalesforceGetEntityInitialCounts", dataStore);
        }
    }
}
