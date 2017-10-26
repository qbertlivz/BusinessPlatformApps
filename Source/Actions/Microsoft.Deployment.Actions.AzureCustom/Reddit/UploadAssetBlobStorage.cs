using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.ErrorCode;
using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Deployment.Actions.AzureCustom.Reddit
{

    [Export(typeof(IAction))]
    public class UploadAssetBlobStorage : BaseAction
    {
        public const string StorageAccountConnectionString = "StorageAccountConnectionString";
        public const string BlobContainer = "BlobContainer";
        public const string AssetFile = "AssetFile";
        public const string AccessAssetUriParameter = "AccessAssetUriParameter";
        public const string DefaultAccessAssetUriParameter = "AssetAccessUri";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var storageAccountConnectionString = request.DataStore.GetValue(StorageAccountConnectionString);
            var blobContainerName = request.DataStore.GetValue(BlobContainer);
            var assetFile = request.DataStore.GetValue(AssetFile);
            var accessAssetUriParameter = request.DataStore.GetValue(AccessAssetUriParameter);

            if (string.IsNullOrWhiteSpace(accessAssetUriParameter))
            {
                accessAssetUriParameter = DefaultAccessAssetUriParameter;
            }

            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(storageAccountConnectionString))
            {
                errors.Add($"{StorageAccountConnectionString} not defined");
            }

            if (string.IsNullOrWhiteSpace(blobContainerName))
            {
                errors.Add($"{BlobContainer} not defined");
            }

            if (string.IsNullOrWhiteSpace(assetFile))
            {
                errors.Add($"{AssetFile} not defined");
            }

            if (errors.Count != 0)
            {
                var errorMessage = string.Join("<br/>", errors);
                return new ActionResponse(
                    ActionStatus.Failure,
                    errors,
                    null,
                    DefaultErrorCodes.DefaultErrorCode,
                    errorMessage
                );
            }

            var localPath = $"{request.Info.App.AppFilePath}/{assetFile}";

            if (!File.Exists(localPath))
            {
                return new ActionResponse(
                    ActionStatus.Failure,
                    assetFile,
                    null,
                    DefaultErrorCodes.DefaultErrorCode,
                    $"{AssetFile} {assetFile} not found"
                );
            }

            if (!CloudStorageAccount.TryParse(storageAccountConnectionString, out var cloudStorageAccount))
            {
                return new ActionResponse(
                    ActionStatus.Failure,
                    storageAccountConnectionString,
                    null,
                    DefaultErrorCodes.DefaultErrorCode,
                    $"Azure storage account {storageAccountConnectionString} was not resolvable"
                );
            }

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(blobContainerName);
            await blobContainer.CreateIfNotExistsAsync();
            var cloudBlob = blobContainer.GetBlockBlobReference(FileNameOnly(assetFile));
            using (var fileStream = File.OpenRead(localPath))
            {
                cloudBlob.UploadFromStream(fileStream);
            }

            request.DataStore.AddToDataStore(accessAssetUriParameter, cloudBlob.Uri, DataStoreType.Public);

            return new ActionResponse(ActionStatus.Success);
        }

        private static string FileNameOnly(string path)
        {
            var segments = path.Split('/');
            return segments[segments.Length - 1];
        }
    }
}