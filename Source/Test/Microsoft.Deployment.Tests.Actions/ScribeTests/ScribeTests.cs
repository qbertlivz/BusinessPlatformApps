using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Tests.Actions.PowerApp
{
    [TestClass]
    public class ScribeTests
    {
        [TestMethod]
        public void ValidateParseEntities()
        {
            string entitiesJson = "{\"account\":\"accountid\",\"lead\":\"leadid\",\"opportunity\":\"opportunityid\",\"opportunityproduct\":\"opportunityproductid\",\"product\":\"productid\",\"systemuser\":\"systemuserid\",\"systemusermanagermap\":\"systemusermanagermapid\",\"territory\":\"territoryid\",\"team\":\"teamid\"}";

            List<string> entities = JsonUtility.DeserializeEntities(entitiesJson);

            Assert.IsFalse(entities.IsNullOrEmpty());
        }
    }
}