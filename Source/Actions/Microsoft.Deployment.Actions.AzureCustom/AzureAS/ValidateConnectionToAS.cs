using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.Tabular;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Actions.AzureCustom.AzureToken;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureAS
{
    [Export(typeof(IAction))]
    public class ValidateConnectionToAS : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string serverUrl = request.DataStore.GetValue("ASServerUrl");
            var azureToken = request.DataStore.GetJson("AzureTokenAS");
            string connectionString = GetASConnectionString(request, azureToken, serverUrl);

            return new ActionResponse(ActionStatus.Success);
        }

        public static string GetASConnectionString(ActionRequest request, JToken azureToken, string serverUrl)
        {
            string connectionString = $"Provider=MSOLAP;Data Source={serverUrl}";
            Uri uri = new Uri(serverUrl);
            string resource = "https://" + uri.Host;

            var asToken = AzureTokenUtility.GetTokenForResourceFromExistingToken("as", request.Info.WebsiteRootUrl, azureToken, resource);
            string asAccessToken = AzureUtility.GetAccessToken(asToken);

            if (!string.IsNullOrEmpty(asAccessToken))
            {
                connectionString +=
                    $";Password={asAccessToken};UseADALCache=0";
            }

            try
            {
                Server server = new Server();
                server.Connect(connectionString);
                request.DataStore.AddToDataStore("ASConnectionString", connectionString, DataStoreType.Private);
                return connectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}