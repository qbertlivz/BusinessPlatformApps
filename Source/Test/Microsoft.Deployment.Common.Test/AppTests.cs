using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.AppLoad;
using Microsoft.Deployment.Common.Controller;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Common.Test
{
    [TestClass]
    public class AppTests
    {
        [TestMethod]
        public void GetApps()
        {

            AppFactory appFactory = new AppFactory(true);
            Assert.IsTrue(appFactory.Apps.Count > 0);
        }

        [TestMethod]
        public void DataStoreRetrievalTest()
        {
            DataStore store = new DataStore();
            store.PrivateDataStore = new DictionaryIgnoreCase<DictionaryIgnoreCase<JToken>>();
            store.PublicDataStore = new DictionaryIgnoreCase<DictionaryIgnoreCase<JToken>>();

            store.PrivateDataStore.Add("TestRoute", new DictionaryIgnoreCase<JToken>()
            {
                {"TestObject", "TestValue"},
                {"TestObject2", "TestValue2"},
                {"password", "secret"}
            });

            store.PrivateDataStore.Add("TestRoute2", new DictionaryIgnoreCase<JToken>()
            {
                {"TestObject", "TestValue"},
                {"TestObject2", "TestValue2"}
            });

            Assert.IsTrue(store.GetAllValues("TestObject").Count() == 2);
            Assert.IsTrue(store.GetAllValues("TestObject2").Count() == 2);
            Assert.IsTrue(store.GetAllValues("password").Count() == 1);
            Assert.IsTrue(store.GetAllDataStoreItems("password").First().DataStoreType == DataStoreType.Private);
            Assert.IsTrue(store.GetAllDataStoreItems("password").First().ValueAsString == "secret");
            Assert.IsTrue(store.GetAllDataStoreItems("password").First().ToString() == "secret");

            var valueNotFound = store.GetValue("valuenothere");
            var valueNotFoundWithRoute = store.GetValue("routethere", "valuenothere");
            Assert.IsNull(valueNotFoundWithRoute);
            Assert.IsNull(valueNotFound);

            store.AddToDataStore("routethere", "valuenothere", "TestValue");
            valueNotFoundWithRoute = store.GetValue("routethere", "valuenothere");
            Assert.IsNotNull(valueNotFoundWithRoute);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(store.PrivateDataStore);

        }

        [TestMethod]
        public void TestActionsWithObjectTypes()
        {
            AppFactory appFactory = new AppFactory(true);
            Assert.IsTrue(appFactory.Apps.Count > 0);

            var result = appFactory.Actions["Microsoft-MockAction"].ExecuteActionAsync(null).Result;
            Assert.IsTrue(result.Status == ActionStatus.Success);

            var jobject = JObject.FromObject(result);
            Assert.IsNotNull(jobject);
            Assert.IsNotNull(jobject["Body"]["Value"].ToString());
        }

        [TestMethod]
        public void TestActionWithCommonController()
        {
            AppFactory factory = new AppFactory(true);
            CommonControllerModel model = new CommonControllerModel()
            {
                AppFactory = factory
            };
            CommonController commonController = new CommonController(model);
            UserInfo info = new UserInfo();
            info.ActionName = "Microsoft-MockAction";
            info.AppName = "TestApp";
            var result = commonController.ExecuteAction(info, new ActionRequest() { DataStore = new DataStore() }).Result;
            Assert.IsTrue(result.Status == ActionStatus.Success);
        }

        [Ignore]
        [TestMethod]
        public void TestExceptionActionWithCommonController()
        {
            AppFactory factory = new AppFactory(true);
            CommonControllerModel model = new CommonControllerModel()
            {
                AppFactory = factory
            };
            CommonController commonController = new CommonController(model);
            UserInfo info = new UserInfo();
            info.ActionName = "Microsoft-ExceptionAction";
            info.AppName = "TestApp";
            var result = commonController.ExecuteAction(info, new ActionRequest() { DataStore = new DataStore() }).Result;
            Assert.IsTrue(result.Status == ActionStatus.Failure);
        }

        [TestMethod]
        public void TestAzureParam()
        {
            AzureArmParameterGenerator param = new AzureArmParameterGenerator();
            param.AddStringParam("test", "test2");
            param.AddStringParam("test2", "test2");
            var val = param.GetDynamicObject();
        }


        [TestMethod]
        public void DataStoreCaseSensitivityTest()
        {
            try
            {
                DataStore store = new DataStore();
                store.PrivateDataStore = new DictionaryIgnoreCase<DictionaryIgnoreCase<JToken>>();
                store.PublicDataStore = new DictionaryIgnoreCase<DictionaryIgnoreCase<JToken>>();

                store.PrivateDataStore.Add("TestRoute", new DictionaryIgnoreCase<JToken>()
            {
                {"TestObject","TestValue" },
                {"Testobject","TestValue2" },
                {"password","secret" }
            });

                store.PrivateDataStore.Add("TestRoute2", new DictionaryIgnoreCase<JToken>()
            {
                {"TestObject","TestValue" },
                {"TestObject2","TestValue2" }
            });
            }
            catch (ArgumentException)
            {

                return;
            }

            Assert.Fail();

        }

        [TestMethod]
        public void TestSSASConnectionChange()
        {
            using (PBIXUtils pu = new PBIXUtils("..\\..\\Resources\\A1.pbix", "..\\..\\Resources\\A2.pbix"))
            {
                pu.ReplaceSSASConnectionString("s1", "c1", "cc1");
            }

        }

        [TestMethod]
        public void RemoveEntryTest()
        {
            var ds = new DataStore();

            ds.AddToDataStore("Key1", "Value1");
            ds.UpdateValue(DataStoreType.Any, "", "Key1", "Value2");

            var entries = ds.GetAllValues("Key1");

            Assert.IsTrue(entries.Count == 2);

            ds.RemoveFirst("Key1");

            Assert.IsTrue(ds.GetAllValues("Key1").Count() == 1 && ds.GetValue("Key1") == "Value2");

        }
    }
}
