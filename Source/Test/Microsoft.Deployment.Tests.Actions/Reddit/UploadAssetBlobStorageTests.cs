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
    public class UploadAssetBlobStorageTest
    {
        /// <summary>
        /// This field requires an actual storage account connection string.  It must not be left empty.  It must be valid.
        /// </summary>
        // I tested this with a storage account on Project Essex' subscription.  I also am not going to put actual connection strings out on Github.
        private const string WorkingStorageAccountConnectionString = "";


        private const string ImageFileName = "reddit-architecture.png";
        private readonly string testImageLocation = $"Web/Images/{ImageFileName}";
        private readonly string blobContainerName = $"test{DateTime.Now.Ticks}";
        private readonly string brokenStorageAccountConnectionString = WorkingStorageAccountConnectionString.Replace("Account", "ACC");

        [TestMethod]
        public async Task MissingArguments()
        {
            var dataStore = await TestManager.GetDataStore();

            var response = await TestManager.ExecuteActionAsync("Microsoft-UploadAssetBlobStorage", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Failure);
        }

        [TestMethod]
        public async Task BadConnectionString()
        {
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore(UploadAssetBlobStorage.BlobContainer, blobContainerName, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.AssetFile, testImageLocation, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.StorageAccountConnectionString, brokenStorageAccountConnectionString, DataStoreType.Public);

            var response = await TestManager.ExecuteActionAsync("Microsoft-UploadAssetBlobStorage", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Failure);
        }

        [TestMethod]
        public async Task LocalFileNotFound()
        {
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore(UploadAssetBlobStorage.BlobContainer, blobContainerName, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.AssetFile, "file/not/found.jpg", DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.StorageAccountConnectionString, WorkingStorageAccountConnectionString, DataStoreType.Public);

            var response = await TestManager.ExecuteActionAsync("Microsoft-UploadAssetBlobStorage", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Failure);
        }

        [TestMethod]
        public async Task TestUploadAssetToAzure()
        {
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore(UploadAssetBlobStorage.BlobContainer, blobContainerName, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.AssetFile, testImageLocation, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.StorageAccountConnectionString, WorkingStorageAccountConnectionString, DataStoreType.Public);

            var response = await TestManager.ExecuteActionAsync("Microsoft-UploadAssetBlobStorage", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Success);
            var uriString = dataStore.GetValue(UploadAssetBlobStorage.DefaultAccessAssetUriParameter);
            Assert.IsTrue(uriString.Contains($"{blobContainerName}/{ImageFileName}"));
        }

        [TestMethod]
        public async Task TestUploadAssetToAzure_CustomUriParameter()
        {
            var dataStore = await TestManager.GetDataStore();
            dataStore.AddToDataStore(UploadAssetBlobStorage.BlobContainer, blobContainerName, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.AssetFile, testImageLocation, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.StorageAccountConnectionString, WorkingStorageAccountConnectionString, DataStoreType.Public);
            dataStore.AddToDataStore(UploadAssetBlobStorage.AccessAssetUriParameter, "custom", DataStoreType.Public);

            var response = await TestManager.ExecuteActionAsync("Microsoft-UploadAssetBlobStorage", dataStore, "Microsoft-RedditTemplate");
            Assert.IsTrue(response.Status == ActionStatus.Success);
            var uriString = dataStore.GetValue("custom");
            Assert.IsTrue(uriString.Contains($"{blobContainerName}/{ImageFileName}"));
        }

        [TestCleanup]
        public void CleanStorage()
        {
            if (!CloudStorageAccount.TryParse(WorkingStorageAccountConnectionString, out var cloudStorageAccount))
            {
                throw new Exception($"Unable to connect to {WorkingStorageAccountConnectionString} to remove test container {blobContainerName}");
            }

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(blobContainerName);
            blobContainer.DeleteIfExists();
        }
        
    }
}