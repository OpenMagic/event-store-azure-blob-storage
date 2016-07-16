using System;
using LazyCache;

namespace OpenMagic.EventStore.AzureBlobStorage.Infrastructure
{
    public static class CacheExtensions
    {
        public static void Replace(this IAppCache cache, string key, object value)
        {
            // todo - probably not thread safe
            cache.Remove(key);
            cache.Add(key, value);
        }

        public static void Replace(this IAppCache cache, string key, object value, TimeSpan slidingExpiration)
        {
            // todo - probably not thread safe
            cache.Remove(key);
            cache.Add(key, value, slidingExpiration);
        }
    }
}