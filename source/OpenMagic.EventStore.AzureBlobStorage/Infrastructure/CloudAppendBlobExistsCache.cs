using System;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.WindowsAzure.Storage.Blob;

namespace OpenMagic.EventStore.AzureBlobStorage.Infrastructure
{
    public static class CloudAppendBlobExistsCache
    {
        private static readonly TimeSpan CachePolicy = TimeSpan.FromMinutes(20);

        public static void Exists(this CloudAppendBlob blob, IAppCache cache, bool value)
        {
            cache.Replace(blob.CacheKey(), value, CachePolicy);
        }

        public static Task<bool> ExistsAsync(this CloudAppendBlob blob, IAppCache cache)
        {
            return Task.FromResult(cache.GetOrAdd(blob.CacheKey(), () => blob.ExistsAsync().Result, CachePolicy));
        }

        private static string CacheKey(this IListBlobItem blobReference)
        {
            return $"{blobReference.Uri}/exists";
        }
    }
}