using Hyak.Common.Internals;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureToken
{
    [Export(typeof(IAction))]
    public class CreateSpn : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var refreshToken = request.DataStore.GetJson("AzureToken", "refresh_token");
            var subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");

            JObject graphToken;
            using (HttpClient httpClient = new HttpClient())
            {
                string tokenUrl = string.Format(Constants.AzureTokenUri, "common");
                // ms crm token
                string token = GetAzureToken.GetTokenUri2(refreshToken, "https://graph.windows.net", request.Info.WebsiteRootUrl, Constants.MicrosoftClientId);
                StringContent content = new StringContent(token);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                string response2 = httpClient.PostAsync(new Uri(tokenUrl), content).Result.Content.AsString();
                graphToken =   JsonUtility.GetJsonObjectFromJsonString(response2);
            }


            var tenantId = new JwtSecurityToken(request.DataStore.GetJson("AzureToken")["id_token"].ToString())
                                                    .Claims.First(e => e.Type.ToLowerInvariant() == "tid")
                                                    .Value;


            // Generate new key for ClientSecret
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[44];
            rng.GetBytes(buffer);

            // Return a Base64 string representation of the random number.
            var key = Convert.ToBase64String(buffer);
            string graphUriBase = "https://graph.windows.net/{0}/applications";
            string graphApi = string.Format(graphUriBase, tenantId);

            AzureHttpClient client = new AzureHttpClient(graphToken["access_token"].ToString(), subscription);
            dynamic payload = new ExpandoObject();

            payload.displayName = "solutiontemplate";
            payload.availableToOtherTenants = false;
            payload.homepage = "www.test.com";
            payload.identifierUris = new string[1];
            payload.identifierUris[0] = "https://test.com/" + RandomGenerator.GetRandomLowerCaseCharacters(10);

            payload.passwordCredentials = new ExpandoObject[1];
            payload.passwordCredentials[0] = new ExpandoObject();
            payload.passwordCredentials[0].startDate = DateTime.UtcNow.ToString("o");
            payload.passwordCredentials[0].endDate = DateTime.UtcNow.AddYears(3).ToString("o");
            payload.passwordCredentials[0].keyId = Guid.NewGuid();
            payload.passwordCredentials[0].value = key;

            string body = JsonUtility.GetJsonStringFromObject(payload);

            var response = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, graphApi + "?api-version=1.6", body);
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseBodyObj = JsonUtility.GetJObjectFromJsonString(responseBody);
            if (response.IsSuccessStatusCode)
            {
                string appId = responseBodyObj["appId"].ToString();
                return new ActionResponse(ActionStatus.Success, responseBody);
            }

            // Try to login with Service Principal

            return new ActionResponse(ActionStatus.Failure, responseBody);
        }
    }
}
