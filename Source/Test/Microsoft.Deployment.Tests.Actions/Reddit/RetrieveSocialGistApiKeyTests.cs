using Microsoft.Deployment.Actions.AzureCustom.Reddit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Tests.Actions.Reddit
{
    [TestClass]
    class RetrieveSocialGistApiKeyTests
    {

        [TestMethod]
        public void TestKeyFromJson_works()
        {
            var sample = "{\"response\":{\"Request\":{\"Actual\":{\"rt\":\"json\"}},\"Keys\":{\"Key\":[{\"Key\":\"THISISNOTAKEYTHISISATRIBUTE\",\"Description\":\"Microsoft PowerBI - DwayneTest1\",\"Enabled\":1,\"RequestsPerDay\":\"1000\",\"DataCounters\":{\"MaxMatches\":\"10000\",\"MaxDataSet\":\"100\",\"MaxResultsDays\":\"No Limit\"},\"DataSources\":{\"SelectedSources\":\"\",\"DedicatedContentProviderId\":245}}]},\"Timer\":[]}}";
            var retriever = new RetrieveSocialGistApiKey();
            var key = retriever.KeyFromJson(sample);
            Assert.AreEqual("THISISNOTAKEYTHISISATRIBUTE", key);
        }

        [TestMethod]
        public void TestKeyFromJson_malformed_json()
        {

        }

    }
}
