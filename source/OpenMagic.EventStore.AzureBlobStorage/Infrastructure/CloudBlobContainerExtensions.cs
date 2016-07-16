using System;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.WindowsAzure.Storage.Blob;

namespace OpenMagic.EventStore.AzureBlobStorage.Infrastructure
{
    internal static class CloudBlobContainerExtensions
    {
        internal static async Task<CloudAppendBlob> GetAppendBlobAsync(this CloudBlobContainer blobContainer, string blobName, IAppCache cache)
        {
            await blobContainer.CreateIfNotExistsAsync(cache);

            return blobContainer.GetAppendBlobReference(blobName);
        }

        internal static Task CreateIfNotExistsAsync(this CloudBlobContainer blobContainer, IAppCache cache)
        {
            var cacheKey = $"{nameof(CloudBlobContainerExtensions)}/{nameof(CreateIfNotExistsAsync)}/{blobContainer.Name}";
            var slidingExpiration = TimeSpan.FromMinutes(20);
            return Task.Run(() => cache.GetOrAdd(cacheKey, () => blobContainer.CreateIfNotExists(), slidingExpiration));
        }
    }
}