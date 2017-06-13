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
    public class ExportActivityLogToEventHubTest
    {
        [TestMethod]
        public async Task ExportActivityLogToEventHub()
        {
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlGWERwYmZNRlQyU3ZRdVhoODQ2WVR3RUlCdyIsImtpZCI6IjlGWERwYmZNRlQyU3ZRdVhoODQ2WVR3RUlCdyJ9.eyJhdWQiOiJodHRwczovL21hbmFnZW1lbnQuY29yZS53aW5kb3dzLm5ldC8iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC83MmY5ODhiZi04NmYxLTQxYWYtOTFhYi0yZDdjZDAxMWRiNDcvIiwiaWF0IjoxNDk3MzkyMzEyLCJuYmYiOjE0OTczOTIzMTIsImV4cCI6MTQ5NzM5NjIxMiwiX2NsYWltX25hbWVzIjp7Imdyb3VwcyI6InNyYzEifSwiX2NsYWltX3NvdXJjZXMiOnsic3JjMSI6eyJlbmRwb2ludCI6Imh0dHBzOi8vZ3JhcGgud2luZG93cy5uZXQvNzJmOTg4YmYtODZmMS00MWFmLTkxYWItMmQ3Y2QwMTFkYjQ3L3VzZXJzLzU4ZmRlMDdmLTZjMzMtNGI4Yi04NGUwLWNhOGIyNjBlOWQxMS9nZXRNZW1iZXJPYmplY3RzIn19LCJhY3IiOiIxIiwiYWlvIjoiWTJaZ1lGRE05bWY3OW4zQ2tSdC9YblRLcExHZHRyNHFrWDNlWVlacFlPNmNyS0NsMGxzQSIsImFtciI6WyJwd2QiLCJtZmEiXSwiYXBwaWQiOiJjNDRiNDA4My0zYmIwLTQ5YzEtYjQ3ZC05NzRlNTNjYmRmM2MiLCJhcHBpZGFjciI6IjIiLCJlX2V4cCI6MjYyODAwLCJmYW1pbHlfbmFtZSI6IkxhTW90dGUiLCJnaXZlbl9uYW1lIjoiTGFuY2UiLCJpbl9jb3JwIjoidHJ1ZSIsImlwYWRkciI6IjEzMS4xMDcuMTU5LjIwMCIsIm5hbWUiOiJMYW5jZSBMYU1vdHRlIiwib2lkIjoiNThmZGUwN2YtNmMzMy00YjhiLTg0ZTAtY2E4YjI2MGU5ZDExIiwib25wcmVtX3NpZCI6IlMtMS01LTIxLTIxMjc1MjExODQtMTYwNDAxMjkyMC0xODg3OTI3NTI3LTI2Nzc0OTUxIiwicGxhdGYiOiIzIiwicHVpZCI6IjEwMDMwMDAwQTFBMzFEOEUiLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiIwSm00OENFVEFHZmJBNFIySGl2RUtaUG5OWU9HamJNcy1XWUZ0Z3hPbFhjIiwidGlkIjoiNzJmOTg4YmYtODZmMS00MWFmLTkxYWItMmQ3Y2QwMTFkYjQ3IiwidW5pcXVlX25hbWUiOiJ0LWxhbGFtb0BtaWNyb3NvZnQuY29tIiwidXBuIjoidC1sYWxhbW9AbWljcm9zb2Z0LmNvbSIsInZlciI6IjEuMCJ9.fSQ4agNRRKPKptpxqpN9yrLziNKxRAhWkG_1RlrmYlbvn0NB43q7w14V30yuKQERfURNBuX9ik7rxAvmgI_0MggyGsF5EH9OZjrWcradnP3H9RgZ78zMVh2ZVHto1kBq_A1oK6-6PghktxF0Ka1Y0nSAHsaqamwpnHgV2F-lMAH9jTEG4zrFo4DBYeoaG2rG8dNyL2oTcHK-DsGdSXXyAVyKrYD17JgxpbS-Vd0EG9L_E8sATppq4HxebbcEaHdicpxV2mGJGk2oHU17yngmrFvsHMHaXnP5w3BSxFdJ7wUSw39l5k_VCVa01Fl226V_uRQDI73TVHci6taaA78dng";
            string subscription = "657eb4a4-2e7c-485c-aee6-2816aef905c5";
            string requestURI = "https://management.azure.com/subscriptions/657eb4a4-2e7c-485c-aee6-2816aef905c5/providers/microsoft.insights/logprofiles/default?api-version=2016-03-01";
            string relativeUrl = "providers/microsoft.insights/logprofiles/default";
            string apiVersion = "2016-03-01";
            string body = "{\"id\":null,\"location\":null,\"name\":null,\"properties\":{\"categories\":[\"Write\",\"Delete\",\"Action\"],\"storageAccountId\":null,\"locations\":[\"australiaeast\",\"australiasoutheast\",\"brazilsouth\",\"canadacentral\",\"canadaeast\",\"centralindia\",\"centralus\",\"eastasia\",\"eastus\",\"eastus2\",\"japaneast\",\"japanwest\",\"koreacentral\",\"koreasouth\",\"northcentralus\",\"northeurope\",\"southcentralus\",\"southindia\",\"southeastasia\",\"uksouth\",\"ukwest\",\"westcentralus\",\"westeurope\",\"westindia\",\"westus\",\"westus2\",\"global\"],\"retentionPolicy\":{\"enabled\":false,\"days\":0},\"serviceBusRuleId\":\"/subscriptions/657eb4a4-2e7c-485c-aee6-2816aef905c5/resourceGroups/LancesRG/providers/Microsoft.EventHub/namespaces/Lances-Namespace-Event-Hub/authorizationrules/RootManageSharedAccessKey\"},\"tags\":null}";
            AzureHttpClient ahc = new AzureHttpClient(token, subscription);
            HttpResponseMessage response = await ahc.ExecuteWithSubscriptionAsync(HttpMethod.Put, relativeUrl, apiVersion, body);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
