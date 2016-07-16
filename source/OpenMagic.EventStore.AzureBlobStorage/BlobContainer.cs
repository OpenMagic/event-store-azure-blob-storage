using System;
using System.Threading.Tasks;
using Anotar.LibLog;
using LazyCache;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OpenMagic.EventStore.AzureBlobStorage.Exceptions;
using OpenMagic.EventStore.AzureBlobStorage.Infrastructure;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public class BlobContainer : IBlobContainer
    {
        private readonly IBlobNamer _blobNamer;
        private readonly IAppCache _cache;
        private readonly CloudBlobContainer _container;
        private readonly IEventEnvelopeSerializer _serializer;

        private BlobContainer(string connectionString, string containerName, IBlobNamer blobNamer, IEventEnvelopeSerializer serializer, IAppCache cache)
        {
            _blobNamer = blobNamer;
            _serializer = serializer;
            _cache = cache;

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            _container = blobClient.GetContainerReference(containerName);
        }

        public async Task<IBlob> CreateBlobAsync(Type aggregateType, string aggregateId)
        {
            LogTo.Trace($"{nameof(BlobContainer)}.{nameof(CreateBlobAsync)}({aggregateType}, {aggregateId}");
            return await Blob.CreateAsync(_container, _blobNamer.GetBlobName(aggregateType, aggregateId), _serializer, _cache);
        }

        public async Task<IBlob> GetBlobAsync(Type aggregateType, string aggregateId)
        {
            return await Blob.GetAsync(_container, _blobNamer.GetBlobName(aggregateType, aggregateId), _serializer, _cache);
        }

        public static async Task<BlobContainer> GetAsync(string connectionString, string containerName)
        {
            LogTo.Trace($"{nameof(BlobContainer)}.{nameof(GetAsync)}(connectionString, {containerName}");

            var container = new BlobContainer(connectionString, containerName, DependencyResolver.Get<IBlobNamer>(), DependencyResolver.Get<IEventEnvelopeSerializer>(), DependencyResolver.Get<IAppCache>());

            if (await container.ExistsAsync())
            {
                return container;
            }

            throw new ContainerNotFoundException(containerName);
        }

        private bool Exists()
        {
            var key = _container.StorageUri.PrimaryUri.ToString();
            var inCache = true;
            var exists = _cache.GetOrAdd(key, () => ExistsFactory(out inCache), TimeSpan.FromMinutes(20));

            LogTo.Trace($"Container: {key}, exists: {exists}, inCache {inCache}");

            return exists;
        }

        private Task<bool> ExistsAsync()
        {
            return Task.FromResult(Exists());
        }

        private bool ExistsFactory(out bool inCache)
        {
            inCache = false;
            return _container.Exists();
        }
    }
}