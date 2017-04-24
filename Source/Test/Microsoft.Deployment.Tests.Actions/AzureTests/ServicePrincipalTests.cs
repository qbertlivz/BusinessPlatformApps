using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Clients.ActiveDirectory.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ServicePrincipalTests
    {
        [TestMethod]
        public async Task CreateServicePrincipal()
        {
            var dataStore = await TestManager.GetDataStore(true);
            var response = TestManager.ExecuteAction("Microsoft-CreateSpn", dataStore);
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
