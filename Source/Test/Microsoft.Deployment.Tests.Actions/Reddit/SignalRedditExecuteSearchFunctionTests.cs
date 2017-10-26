using System;
using System.Threading.Tasks;
using Microsoft.Deployment.Actions.AzureCustom.Reddit;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Tests.Actions.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Deployment.Tests.Actions.Reddit
{
    [TestClass]
    public class SignalRedditExecuteSearchFunctionTests
    {
        /// <summary>
        /// This field requires an actual storage account connection string.  It must not be left empty.  It must be valid.
        /// </summary>
        // I tested this with a storage account on Project Essex' subscription.  I also am not going to put actual connection strings out on Github.
        private const string WorkingStorageAccountConnectionString = "";


        private const string storageQueueName = "line";
        private readonly string brokenStorageAccountConnectionString = WorkingStorageAccountConnectionString.Replace("Account", "ACC");

        [TestMethod]
        public async Task MissingArguments()
        {
            var dataStore = await TestManager.GetDataStore();

            var response = await TestManager.ExecuteActionAsync("Microsoft-SignalExecuteRedditSearchFunction", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Failure);
        }

        [TestMethod]
        public async Task BadConnectionString()
        {
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore(SignalExecuteRedditSearchFunction.StorageAccountConnectionString, brokenStorageAccountConnectionString, DataStoreType.Public);

            var response = await TestManager.ExecuteActionAsync("Microsoft-SignalExecuteRedditSearchFunction", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Failure);
        }

        [TestMethod]
        public async Task TestAddToQueue()
        {
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore(SignalExecuteRedditSearchFunction.StorageAccountConnectionString, WorkingStorageAccountConnectionString, DataStoreType.Public);
            dataStore.AddToDataStore(SignalExecuteRedditSearchFunction.StorageQueueName, storageQueueName, DataStoreType.Public);

            var response = await TestManager.ExecuteActionAsync("Microsoft-SignalExecuteRedditSearchFunction", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Success);
        }

        [TestCleanup]
        public void CleanQueue()
        {
            if (!CloudStorageAccount.TryParse(WorkingStorageAccountConnectionString, out var cloudStorageAccount))
            {
                throw new Exception($"Unable to connect to {WorkingStorageAccountConnectionString} to remove queue {storageQueueName}");
            }

            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var cloudQueue = queueClient.GetQueueReference(storageQueueName);
            cloudQueue.DeleteIfExists();
        }
        
    }
}