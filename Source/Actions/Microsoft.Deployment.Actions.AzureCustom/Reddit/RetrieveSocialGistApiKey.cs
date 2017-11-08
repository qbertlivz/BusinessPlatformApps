using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.ErrorCode;

namespace Microsoft.Deployment.Actions.AzureCustom.Reddit
{
    [Export(typeof(IAction))]
    public class RetrieveSocialGistApiKey : BaseAction
    {
        private const string SocialGistKeyQueryParameterName = "key";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            // Using the super private Microsoft key, make an HTTP request to a SocialGist endpoint and request a new Social Gist Reddit API key is generated on behalf of the user
            // once retrieved, put it in "SocialGistApiKey" in the DataStore 
            try
            {
                var socialGistApiKey = await RetrieveKey(
                    Constants.SocialGistProvisionKeyUrl,
                    Constants.SocialGistProvisionKeySecret
                );
                // currently returns no value, which we then use and place into the AzureFunction AppSetting step.  Once you populate this with the real value from SocialGist, you shouldn't have to change
                // the init.json
                request.DataStore.AddToDataStore(
                    "SocialGistApiKey", 
                    socialGistApiKey, 
                    DataStoreType.Private
                );
                return new ActionResponse(ActionStatus.Success);
            }
            catch (Exception e)
            {
                return new ActionResponse(
                    ActionStatus.Failure,
                    null,
                    e,
                    DefaultErrorCodes.DefaultErrorCode,
                    "An error occurred contacting SocialGist for your Reddit API key"
                );
            }
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