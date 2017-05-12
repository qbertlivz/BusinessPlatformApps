using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureToken
{
    [Export(typeof(IAction))]
    [Export(typeof(IActionRequestInterceptor))]
    public class RefreshAzureToken : BaseAction, IActionRequestInterceptor
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string tokenUrl = string.Format(Constants.AzureTokenUri, request.DataStore.GetValue("AADTenant"));

            JObject azureToken = new JObject();
            JObject crmToken = new JObject();
            JObject keyvaultToken = new JObject();

            var token = request.DataStore.GetJson("AzureToken");
            if (token != null)
            {
                azureToken = await GetToken(request.DataStore.GetJson("AzureToken", "refresh_token"), tokenUrl, Constants.MicrosoftClientId);
                azureToken.Add("id_token", token["id_token"]);
            }

            var tokenMsCrm = request.DataStore.GetJson("MsCrmToken");
            if (token != null && tokenMsCrm != null)
            {
                crmToken = GetAzureToken.RetrieveCrmToken(azureToken["refresh_token"].ToString(), request.Info.WebsiteRootUrl, request.DataStore);
            }

            var tokenKV = request.DataStore.GetJson("AzureTokenKV");
            if (tokenKV != null)
            {
                keyvaultToken = await GetToken(request.DataStore.GetJson("AzureTokenKV", "refresh_token"), tokenUrl, Constants.MicrosoftClientIdCrm);
                keyvaultToken.Add("id_token", tokenKV["id_token"]);
            }

            return new ActionResponse(ActionStatus.Success,
                new JObject(new JProperty("AzureToken", azureToken), new JProperty("MsCrmToken", crmToken), new JProperty("AzureTokenKV", keyvaultToken)),
                true);
        }

        private static async Task<JObject> GetToken(string refreshToken, string tokenUrl, string clientId)
        {
            HttpClient client = new HttpClient();

            var builder = GetTokenUri(refreshToken, Constants.AzureManagementCoreApi, clientId);
            var content = new StringContent(builder.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.PostAsync(new Uri(tokenUrl), content).Result.Content.ReadAsStringAsync();

            var primaryResponse = JsonUtility.GetJsonObjectFromJsonString(response);
            return primaryResponse;
        }

        private static StringBuilder GetTokenUri(string refresh_token, string uri, string clientId)
        {
            Dictionary<string, string> message = new Dictionary<string, string>
            {
                {"refresh_token", refresh_token},
                {"client_id", clientId},
                {"client_secret", Uri.EscapeDataString(Constants.MicrosoftClientSecret)},
                {"resource", Uri.EscapeDataString(uri)},
                {"grant_type", "refresh_token"}
            };

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in message)
            {
                builder.Append(keyValuePair.Key + "=" + keyValuePair.Value);
                builder.Append("&");
            }
            return builder;
        }

        public async Task<InterceptorStatus> CanInterceptAsync(IAction actionToExecute, ActionRequest request)
        {
            if (request.DataStore.GetValue("AzureToken") != null && request.DataStore.GetJson("AzureToken", "expires_on") != null)
            {
                var expiryDateTime = UnixTimeStampToDateTime(request.DataStore.GetJson("AzureToken", "expires_on"));
                if ((expiryDateTime - DateTime.Now).TotalMinutes < 5)
                {
                    return InterceptorStatus.Intercept;
                }
            }

            if (request.DataStore.GetValue("AzureTokenKV") != null && request.DataStore.GetJson("AzureTokenKV", "expires_on") != null)
            {
                var expiryDateTime = UnixTimeStampToDateTime(request.DataStore.GetJson("AzureTokenKV", "expires_on"));
                if ((expiryDateTime - DateTime.Now).TotalMinutes < 5)
                {
                    return InterceptorStatus.Intercept;
                }
            }

            if (request.DataStore.GetValue("MsCrmToken") != null && request.DataStore.GetJson("MsCrmToken", "expires_on") != null)
            {
                var expiryDateTime = UnixTimeStampToDateTime(request.DataStore.GetJson("MsCrmToken", "expires_on"));
                if ((expiryDateTime - DateTime.Now).TotalMinutes < 5)
                {
                    return InterceptorStatus.Intercept;
                }
            }

            return InterceptorStatus.Skipped;
        }

        /// <summary>
        /// Update the token (first token found in the data store)
        /// </summary>
        /// <param name="actionToExecute"> the action to execute</param>
        /// <param name="request">the request body</param>
        /// <returns>the response of the intercept</returns>
        public async Task<ActionResponse> InterceptAsync(IAction actionToExecute, ActionRequest request)
        {
            var tokenRefreshResponse = await this.ExecuteActionAsync(request);
            if (tokenRefreshResponse.Status == ActionStatus.Success)
            {
                if (request.DataStore.GetJson("AzureToken") != null)
                {
                    var dsAzureToken = request.DataStore.GetDataStoreItem("AzureToken");
                    request.DataStore.UpdateValue(dsAzureToken.DataStoreType, dsAzureToken.Route, dsAzureToken.Key, JObject.FromObject(tokenRefreshResponse.Body)["AzureToken"]);
                }

                if (request.DataStore.GetJson("MsCrmToken") != null)
                {
                    var dsMsCrmToken = request.DataStore.GetDataStoreItem("MsCrmToken");
                    request.DataStore.UpdateValue(dsMsCrmToken.DataStoreType, dsMsCrmToken.Route, dsMsCrmToken.Key, JObject.FromObject(tokenRefreshResponse.Body)["MsCrmToken"]);
                }

                if (request.DataStore.GetJson("AzureTokenKV") != null)
                {
                    var dsAzureTokenKV = request.DataStore.GetDataStoreItem("AzureTokenKV");
                    request.DataStore.UpdateValue(dsAzureTokenKV.DataStoreType, dsAzureTokenKV.Route, dsAzureTokenKV.Key, JObject.FromObject(tokenRefreshResponse.Body)["AzureTokenKV"]);
                }
            }

            return tokenRefreshResponse;
        }

        public static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(double.Parse(unixTimeStamp)).ToLocalTime();
            return dtDateTime;
        }
    }
}