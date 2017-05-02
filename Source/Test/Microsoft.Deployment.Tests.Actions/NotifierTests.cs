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

        [TestMethod]
        public void SccmGetInitialCounts()
        {
            var dataStore = new DataStore();
            dataStore.AddToDataStore("SccmEntities", "computer,site,user,usercomputer,computerprogram,program,collection,computercollection,malware,computermalware,update,computerupdate,scanhistory,computer_staging,site_staging,user_staging,usercomputer_staging,computerprogram_staging,program_staging,collection_staging,computercollection_staging,malware_staging,computermalware_staging,update_staging,computerupdate_staging,scanhistory_staging");
            

            var resp = TestManager.ExecuteAction("Microsoft-SccmGetEntityInitialCounts", dataStore);
        }
    }
}
