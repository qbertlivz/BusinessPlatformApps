using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;

namespace Microsoft.Deployment.Actions.AzureCustom.Reddit
{
    [Export(typeof(IAction))]
    public class RetrieveSocialGistApiKey : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            // Using the super private Microsoft key, make an HTTP request to a SocialGist endpoint (tbd) and request a new Social Gist API key is generated
            // once retrieved, put it in "SocialGistApiKey" in the DataStore (note: must it be public?  Can it be private?  Better to be private, but I don't really know how the DS keeps things secure)
            
            // currently returns no value, which we then use and place into the AzureFunction AppSetting step.  Once you populate this with the real value from SocialGist, you shouldn't have to change
            // the init.json
            request.DataStore.AddToDataStore("SocialGistApiKey", "", DataStoreType.Public);
            return new ActionResponse(ActionStatus.Success);
        }
    }
}