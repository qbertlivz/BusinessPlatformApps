using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Tests.Actions.AzureTests
{
    [TestClass]
    public class ActivityLogTemplateTests
    {
        [TestInitialize]
        public void testInit()
        { }

        [ClassInitialize]
        public void classInit()
        {

        }

        public string id = RandomGenerator.GetRandomCharacters();
        public string token;
        [TestMethod]
        public async Task main()
        {
            System.Diagnostics.Debug.WriteLine($"id: {id}");
            Common.ActionModel.DataStore dataStore = await TestManager.GetDataStore(true);
            Common.ActionModel.DataStore dataStoreCopy = dataStore;
            await CreateStreamAnalyticsJob(dataStore);
            await CreateEventHubNameSpace(dataStoreCopy);
            token = dataStoreCopy.GetJson("AzureToken", "access_token");
            ExportActivityLogToEventHub(dataStoreCopy);


        }

        public async Task CreateEventHubNameSpace(Common.ActionModel.DataStore dataStore)
        {
            dataStore.AddToDataStore("DeploymentName", $"LanceEHDeployment-{id}");
            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/EventHub.json");
            var payload = new JObject();
            payload.Add("namespaceName", $"LancesEventHubNamespace-{id}");
            payload.Add("eventHubName", $"LancesEventHubName-{id}");
            payload.Add("consumerGroupName", $"LancesConsumerGroup-{id}");
            dataStore.AddToDataStore("AzureArmParameters", payload);
            var deployArmResult = await TestManager.ExecuteActionAsync(
                "Microsoft-DeployAzureArmTemplate", dataStore, "Microsoft-ActivityLogTemplate");
            Assert.IsTrue(deployArmResult.IsSuccess);
        }

        
        public async Task CreateStreamAnalyticsJob(Common.ActionModel.DataStore dataStore)
        {
            //var dataStore = await TestManager.GetDataStore(true);
            dataStore.AddToDataStore("DeploymentName", $"LanceSADeployment-{id}");
            dataStore.AddToDataStore("AzureArmFile", "Service/ARM/StreamAnalytics.json");
            var payload = new JObject();
            payload.Add("name", $"LancesStreamAnalyticsJob-{id}");
            dataStore.AddToDataStore("AzureArmParameters", payload);

            var deployArmResult = await TestManager.ExecuteActionAsync(
                "Microsoft-DeployAzureArmTemplate", dataStore, "Microsoft-ActivityLogTemplate");
            Assert.IsTrue(deployArmResult.IsSuccess);

        }

        public async Task ExportActivityLogToEventHub(Common.ActionModel.DataStore dataStore)
        {
            //string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlGWERwYmZNRlQyU3ZRdVhoODQ2WVR3RUlCdyIsImtpZCI6IjlGWERwYmZNRlQyU3ZRdVhoODQ2WVR3RUlCdyJ9.eyJhdWQiOiJodHRwczovL21hbmFnZW1lbnQuY29yZS53aW5kb3dzLm5ldC8iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC83MmY5ODhiZi04NmYxLTQxYWYtOTFhYi0yZDdjZDAxMWRiNDcvIiwiaWF0IjoxNDk3NDU5NDUxLCJuYmYiOjE0OTc0NTk0NTEsImV4cCI6MTQ5NzQ2MzM1MSwiX2NsYWltX25hbWVzIjp7Imdyb3VwcyI6InNyYzEifSwiX2NsYWltX3NvdXJjZXMiOnsic3JjMSI6eyJlbmRwb2ludCI6Imh0dHBzOi8vZ3JhcGgud2luZG93cy5uZXQvNzJmOTg4YmYtODZmMS00MWFmLTkxYWItMmQ3Y2QwMTFkYjQ3L3VzZXJzLzU4ZmRlMDdmLTZjMzMtNGI4Yi04NGUwLWNhOGIyNjBlOWQxMS9nZXRNZW1iZXJPYmplY3RzIn19LCJhY3IiOiIxIiwiYWlvIjoiQVNRQTIvOERBQUFBNDhQOXRacXMvZkxkRVRiVGhJRkN6aXJRalN0OHNrcmxTZVhmQ0crZ2s0az0iLCJhbXIiOlsicHdkIiwibWZhIl0sImFwcGlkIjoiYzQ0YjQwODMtM2JiMC00OWMxLWI0N2QtOTc0ZTUzY2JkZjNjIiwiYXBwaWRhY3IiOiIyIiwiZV9leHAiOjI2MjgwMCwiZmFtaWx5X25hbWUiOiJMYU1vdHRlIiwiZ2l2ZW5fbmFtZSI6IkxhbmNlIiwiaW5fY29ycCI6InRydWUiLCJpcGFkZHIiOiIxMzEuMTA3LjE1OS4yMDAiLCJuYW1lIjoiTGFuY2UgTGFNb3R0ZSIsIm9pZCI6IjU4ZmRlMDdmLTZjMzMtNGI4Yi04NGUwLWNhOGIyNjBlOWQxMSIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS0yMTI3NTIxMTg0LTE2MDQwMTI5MjAtMTg4NzkyNzUyNy0yNjc3NDk1MSIsInBsYXRmIjoiMyIsInB1aWQiOiIxMDAzMDAwMEExQTMxRDhFIiwic2NwIjoidXNlcl9pbXBlcnNvbmF0aW9uIiwic3ViIjoiMEptNDhDRVRBR2ZiQTRSMkhpdkVLWlBuTllPR2piTXMtV1lGdGd4T2xYYyIsInRpZCI6IjcyZjk4OGJmLTg2ZjEtNDFhZi05MWFiLTJkN2NkMDExZGI0NyIsInVuaXF1ZV9uYW1lIjoidC1sYWxhbW9AbWljcm9zb2Z0LmNvbSIsInVwbiI6InQtbGFsYW1vQG1pY3Jvc29mdC5jb20iLCJ2ZXIiOiIxLjAifQ.ekGgUBNZkDWsWe4JN0Mi8diqok12bXS-mOiAG0KWweseDFQPHAvdzIa9rmh0AS9hYQjqokz2tK4MjBeV2hUDSQ6uhBPEVqNh9UP9GVK12j6FumNU_OSlWls0HvPVAPzjqtvUjj4KdSTWPWsEMCFp3wv8JITPoxvLZJ87pIIF0H4U5tg8oIaNO4SdSS11x7hb0Bd1rnnciVNPUFXMnKUZFNbS-vlRRgtlSmjoiLPiMSuLhUG6KAkJUf6CjizGEhL6PWT1cm7xzYiRFCGtxIm97xBLGZEQycm8YdDKI0flRK3wY_14DwMkxhPybm2DofWOQ5Vjp1cqmr-WhAUNeeEubA";
            string subscription = "657eb4a4-2e7c-485c-aee6-2816aef905c5";
            string requestURI = "https://management.azure.com/subscriptions/657eb4a4-2e7c-485c-aee6-2816aef905c5/providers/microsoft.insights/logprofiles/default?api-version=2016-03-01";
            string relativeUrl = "providers/microsoft.insights/logprofiles/default";
            string apiVersion = "2016-03-01";
            string body = $"{{\"id\":null,\"location\":null,\"name\":null,\"properties\":{{\"categories\":[\"Write\",\"Delete\",\"Action\"],\"storageAccountId\":null,\"locations\":[\"australiaeast\",\"australiasoutheast\",\"brazilsouth\",\"canadacentral\",\"canadaeast\",\"centralindia\",\"centralus\",\"eastasia\",\"eastus\",\"eastus2\",\"japaneast\",\"japanwest\",\"koreacentral\",\"koreasouth\",\"northcentralus\",\"northeurope\",\"southcentralus\",\"southindia\",\"southeastasia\",\"uksouth\",\"ukwest\",\"westcentralus\",\"westeurope\",\"westindia\",\"westus\",\"westus2\",\"global\"],\"retentionPolicy\":{{\"enabled\":false,\"days\":0}},\"serviceBusRuleId\":\"/subscriptions/{subscription}/resourceGroups/minint-cebcjkutest/providers/Microsoft.EventHub/namespaces/LancesEventHubNamespace/authorizationrules/RootManageSharedAccessKey\"}},\"tags\":null}}";
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            HttpResponseMessage response = await ahc.ExecuteWithSubscriptionAsync(HttpMethod.Put, relativeUrl, apiVersion, body);
            if (response.ReasonPhrase == "Unauthorized")
            {
                System.Diagnostics.Debug.WriteLine("Bad token");
            }
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
