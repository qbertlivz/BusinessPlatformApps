using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;

namespace Microsoft.Deployment.Actions.AzureCustom.Reddit
{
    [Export(typeof(IAction))]
    public class RetrieveSocialGistApiKey : BaseAction
    {
        private const string SocialGistKeyQueryParameterName = "key";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            // Using the super private Microsoft key, make an HTTP request to a SocialGist endpoint (tbd) and request a new Social Gist API key is generated
            // once retrieved, put it in "SocialGistApiKey" in the DataStore (note: must it be public?  Can it be private?  Better to be private, but I don't really know how the DS keeps things secure)
            var socialGistApiKey = await RetrieveKey("tbd", "changeme");

            // currently returns no value, which we then use and place into the AzureFunction AppSetting step.  Once you populate this with the real value from SocialGist, you shouldn't have to change
            // the init.json
            request.DataStore.AddToDataStore("SocialGistApiKey", socialGistApiKey, DataStoreType.Public);
            return new ActionResponse(ActionStatus.Success);
        }

        internal async Task<string> RetrieveKey(
            string socialGistKeyManagerUri,
            string microsoftSocialGistKey
        )
        {
            return "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            using (var client = new System.Net.Http.HttpClient())
            {
                var socialGistUriBuilder = new UriBuilder(socialGistKeyManagerUri);
                socialGistUriBuilder.Query = $"{HttpUtility.UrlEncode(SocialGistKeyQueryParameterName)}={HttpUtility.UrlEncode(microsoftSocialGistKey)}";
                var response = await client.GetAsync(socialGistUriBuilder.Uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Unable to contact Social Gist key management service.");
                }
                var key = await response.Content.ReadAsStringAsync();
                // this is probably wrong, as it's almost definitely going to be a json response to us, not just a straight string
                return key;
            }
        }
    }
}