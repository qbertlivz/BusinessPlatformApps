using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Deployment.Common.ActionModel;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Tests.Actions.News
{
    [TestClass]
    public class NewsTests
    {
        [TestMethod]
        public void PopulateUserDefinedEntities()
        {
            var dataStore = new DataStore();
            dataStore.AddToDataStore("UserDefinedEntities", @"[{""color"":""#6699ff"",""icon"":""user"",""name"":""New Entity"",""values"":""Value 1\r\n Value 2\r\n Value 3""},{""color"":""#6699ff"",""icon"":""user"",""name"":""New Entity"",""values"":""Value 1\r\n Value 2\r\n Value 3""}]");
            var response = TestManager.ExecuteAction("Microsoft-PopulateUserDefinedEntities", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
