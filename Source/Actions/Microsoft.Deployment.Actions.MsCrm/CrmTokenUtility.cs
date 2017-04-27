using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Hyak.Common.Internals;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Common.Actions.MsCrm
{
    public static class CrmTokenUtility
    {
        public static JObject RetrieveCrmOnlineToken(string refreshToken, string websiteRootUrl, DataStore dataStore, string resourceUri)
        {
            string tokenUrl = string.Format(Constants.AzureTokenUri, dataStore.GetValue("AADTenant"));

            using (HttpClient httpClient = new HttpClient())
            {
                // CRM Online token
                string token = GetDynamicsResourceUri(refreshToken, resourceUri, websiteRootUrl, Constants.MsCrmClientId);
                StringContent content = new StringContent(token);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                string response = httpClient.PostAsync(new Uri(tokenUrl), content).Result.Content.AsString();

                return JsonUtility.GetJsonObjectFromJsonString(response);
            }
        }

        private static string GetDynamicsResourceUri(string code, string uri, string rootUrl, string clientId)
        {
            return $"refresh_token={code}&" +
                   $"client_id={clientId}&" +
                   $"client_secret={Uri.EscapeDataString(Constants.MicrosoftClientSecret)}&" +
                   $"resource={Uri.EscapeDataString(uri)}&" +
                   $"redirect_uri={Uri.EscapeDataString(rootUrl + Constants.WebsiteRedirectPath)}&" +
                   "grant_type=refresh_token";
        }
    }
}
