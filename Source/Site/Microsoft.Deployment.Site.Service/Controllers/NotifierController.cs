using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.Deployment.Site.Service.Controllers
{

    public class NotifierController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> PostDeploymentId()
        {           
            try
            {
                var resp = new HttpResponseMessage();
                var body = this.Request.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrEmpty(body))
                {
                    resp = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    resp.ReasonPhrase = "Content is null";
                    return resp;
                }

                var content = JsonUtility.GetJsonObjectFromJsonString(body);
                string refreshToken = content["tokens"]?["refresh"].ToString();
                string accessToken = content["tokens"]?["access"].ToString();
                string deploymentId = content["deploymentId"].ToString();

                if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(deploymentId))
                {
                    resp = new HttpResponseMessage(HttpStatusCode.Forbidden); 
                    resp.ReasonPhrase = "Refresh token, access token or deployment id is null.";
                    return resp;
                }

                var originalClaim = new JwtSecurityToken(accessToken).Claims.First(e => e.Type == "_claim_sources").Value;

                string tokenUrl = string.Format(Constants.AzureTokenUri, "common");

                string newClaim;

                try
                {

                    var primaryResponse = await GetToken(refreshToken, tokenUrl, Constants.MicrosoftClientId);

                    var access = new JwtSecurityToken(primaryResponse["access_token"].ToString());
                    newClaim = access.Claims.First(e => e.Type == "_claim_sources").Value;
                }
                catch(Exception e)
                {
                    resp = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    resp.ReasonPhrase = "Access token could not be refreshed, or claim does not exist.";
                    return resp;
                }

                if (originalClaim == newClaim)
                {
                    string deploymentIdsConnection = Constants.BpstDeploymentIdDatabase;
                    var cmd = $"INSERT INTO deploymentids VALUES('{deploymentId}','{DateTime.UtcNow.ToString("o")}')";
                    SqlUtility.InvokeSqlCommand(deploymentIdsConnection, cmd, new Dictionary<string, string>());
                }
                else
                {
                    resp = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    resp.ReasonPhrase = "Claims could not be verified.";
                    return resp;
                }
                                
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                resp.ReasonPhrase = "An internal error has occurred ";
                return resp;               
            }
        }

        private static async Task<JObject> GetToken(string refreshToken, string tokenUrl, string clientId)
        {
            HttpClient client = new HttpClient();

            var builder = GetTokenUri(refreshToken, Constants.AzureManagementCoreApi, clientId);
            var content = new StringContent(builder.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.PostAsync(new Uri(tokenUrl), content);

            var primaryResponse = JsonUtility.GetJsonObjectFromJsonString(response.Content.ReadAsStringAsync().Result);
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
    }
}