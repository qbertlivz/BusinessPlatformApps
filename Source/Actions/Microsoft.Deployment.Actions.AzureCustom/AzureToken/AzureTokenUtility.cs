using Hyak.Common.Internals;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureToken
{
    public class AzureTokenUtility
    {
        public static JObject GetTokenForResource(ActionRequest request,  JToken azureToken, string resource)
        {
            JObject tokenObj;
            using (HttpClient httpClient = new HttpClient())
            {
                string tenantId = AzureUtility.GetTenantFromToken(azureToken);
                string refreshToken = AzureUtility.GetRefreshToken(azureToken);
                string tokenUrl = string.Format(Constants.AzureTokenUri, tenantId);

                string clientId = GetAzureToken.GetClientIdFromRequest(request);
                string token = GetAzureToken.GetTokenUri2(refreshToken, resource, request.Info.WebsiteRootUrl, clientId);
                StringContent content = new StringContent(token);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                string response2 = httpClient.PostAsync(new Uri(tokenUrl), content).Result.Content.AsString();
                tokenObj = JsonUtility.GetJsonObjectFromJsonString(response2);
            }

            return tokenObj;
        }
    }
}
