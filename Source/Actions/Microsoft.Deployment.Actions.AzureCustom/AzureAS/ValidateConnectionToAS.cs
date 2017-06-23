using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.AnalysisServices.Tabular;
using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Actions.AzureCustom.AzureToken;

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
            if (string.IsNullOrEmpty(serverUrl))
                throw new ArgumentNullException(nameof(serverUrl));

            if (!Uri.IsWellFormedUriString(serverUrl, UriKind.Absolute))
                throw new ArgumentException("Invalid URL specified for Analysis Services", nameof(serverUrl));

            string connectionString = $"Provider=MSOLAP;Data Source={serverUrl}";
            Uri uri = new Uri(serverUrl);
            string resource = $"{Uri.UriSchemeHttps}://{uri.Host}";

            var asToken = AzureTokenUtility.GetTokenForResourceFromExistingToken("as", request.Info.WebsiteRootUrl, azureToken, resource);
            string asAccessToken = AzureUtility.GetAccessToken(asToken);

            if (!string.IsNullOrEmpty(asAccessToken))
            {
                connectionString += $";Password={asAccessToken};UseADALCache=0";
            }

            Server server = null;
            try
            {
                server = new Server();
                server.Connect(connectionString);
                server.Disconnect(true);
                request.DataStore.AddToDataStore("ASConnectionString", connectionString, DataStoreType.Private);

                return connectionString;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (server != null)
                {
                    // In theory, we could end up here with a connected server object
                    if (server.Connected)
                    {
                        try
                        {
                            server.Disconnect(true);
                        }
                        catch { }
                    }
                    server.Dispose();
                }
            }
        }
    }
}