using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using OpenMagic.EventStore.AzureBlobStorage.Exceptions;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public class InMemoryBlobContainer : IBlobContainer
    {
        private readonly ConcurrentDictionary<string, IBlob> _blobs = new ConcurrentDictionary<string, IBlob>();
        private readonly IEventEnvelopeSerializer _serializer;

        public InMemoryBlobContainer(IEventEnvelopeSerializer serializer)
        {
            _serializer = serializer;
        }

        public Task<IBlob> CreateBlobAsync(Type aggregateType, string aggregateId)
        {
            return Task.FromResult(_blobs.AddOrUpdate(
                GetKey(aggregateType, aggregateId),
                key => new InMemoryBlob(aggregateType, aggregateId, _serializer),
                (key, blob) => { throw new BlobExistsException(key); }));
        }

        public Task<IBlob> GetBlobAsync(Type aggregateType, string aggregateId)
        {
            return Task.FromResult(_blobs.GetOrAdd(GetKey(aggregateType, aggregateId), key => { throw new BlobNotFoundException(key); }));
        }

        private static string GetKey(Type aggregateType, string aggregateId)
        {
            return $"{aggregateType}/{aggregateId}";
        }
    }
}