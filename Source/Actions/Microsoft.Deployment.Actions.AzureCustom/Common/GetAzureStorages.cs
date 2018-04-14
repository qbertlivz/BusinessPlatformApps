using System.ComponentModel.Composition;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    public class GetAzureStorages : BaseAction
    {
        private Regex resourceGroupEx = new Regex("resourceGroups/([a-zA-Z0-9_-]*)/providers", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var idSubscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");

            AzureHttpClient client = new AzureHttpClient(azureToken, idSubscription);

            var response = await client.ExecuteWithSubscriptionAsync(HttpMethod.Get, $"providers/Microsoft.Storage/storageAccounts", "2017-06-01", string.Empty);
            if (response.IsSuccessStatusCode)
            {
                var storageAccounts = JsonUtility.GetJObjectFromJsonString(await response.Content.ReadAsStringAsync());
                
                JArray array = new JArray();
                foreach (var item in storageAccounts["value"])
                {
                    var storageAccountId = item["id"].ToString();
                    var storageAccountName = item["name"].ToString();
                    JObject newStorageAccountKey = new JObject();
                    newStorageAccountKey.Add("StorageAccountId", storageAccountId);
                    newStorageAccountKey.Add("StorageAccountName", storageAccountName);

                    var connectionString = await this.GetStorageAccountConnectionStringAsync(azureToken, idSubscription, storageAccountName, storageAccountId);
                    newStorageAccountKey.Add("StorageAccountConnectionString", connectionString);

                    newStorageAccountKey.Add("Containers", GetContainers(connectionString));

                    array.Add(newStorageAccountKey);
                }

                request.DataStore.AddToDataStore("StorageAccounts", array);
                return new ActionResponse(ActionStatus.Success, array, true);
            }

            return new ActionResponse(ActionStatus.Failure);
        }

        private async Task<string> GetStorageAccountConnectionStringAsync(string azureToken, string subscriptionId, string storageAccountName, string storageAccountId)
        {
            var matches = resourceGroupEx.Match(storageAccountId);
            var resourceGroup = matches.Success ? matches.Groups[1].Value : string.Empty;

            AzureHttpClient client = new AzureHttpClient(azureToken, subscriptionId, resourceGroup);

            var response = await client.ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod.Post, $"providers/Microsoft.Storage/storageAccounts/{storageAccountName}/listKeys", "2016-01-01", string.Empty);
            if (response.IsSuccessStatusCode)
            {
                var subscriptionKeys = JsonUtility.GetJObjectFromJsonString(await response.Content.ReadAsStringAsync());
                string key = subscriptionKeys["keys"][0]["value"].ToString();
                return $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={key};EndpointSuffix=core.windows.net";
            }

            return string.Empty;
        }

        // hate paging, do it like this for now
        private JArray GetContainers(string storageAccountConnectionString)
        {
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageAccountConnectionString);

            // Create a blob client for interacting with the blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            var containers = blobClient.ListContainers();
            var array = new JArray();
            foreach (var container in containers)
            {
                JObject containerObject = new JObject();
                containerObject.Add("Name", container.Name);
                array.Add(containerObject);
            }

            return array;
        }

        /// <summary>
        /// Validates the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string</param>
        /// <returns>CloudStorageAccount object</returns>
        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            return CloudStorageAccount.Parse(storageConnectionString);
        }
    }
}