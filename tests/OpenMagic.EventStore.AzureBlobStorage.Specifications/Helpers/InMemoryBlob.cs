using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using OpenMagic.EventStore.AzureBlobStorage.Infrastructure;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public class InMemoryBlob : IBlob
    {
        private readonly string _aggregateId;
        private readonly Type _aggregateType;
        private readonly IEventEnvelopeSerializer _serializer;
        private BlobMetadata _blobMetadata;
        private string _events;

        public InMemoryBlob(Type aggregateType, string aggregateId, IEventEnvelopeSerializer serializer)
        {
            _aggregateType = aggregateType;
            _aggregateId = aggregateId;
            _serializer = serializer;

            _events = "";
            _blobMetadata = new BlobMetadata(new Dictionary<string, string>());
        }

        public string Name => $"{_aggregateType}/{_aggregateId}";

        public Task AppendEventsAsync(IEnumerable<object> events)
        {
            LogTo.Trace($"{nameof(InMemoryBlob)}.{nameof(AppendEventsAsync)}(events)");
            return Task.Run(() => AppendEvents(events));
        }

        public Task<BlobMetadata> GetMetadataAsync()
        {
            return Task.FromResult(_blobMetadata);
        }

        public Task<IEnumerable<object>> ReadEventsAsync()
        {
            var lines = _events.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            var events = lines.Select(e => _serializer.Deserialize(e));

            return Task.FromResult(events);
        }

        public Task UpdateMetadataAsync(BlobMetadata blobMetadata)
        {
            return Task.Run(() => _blobMetadata = blobMetadata);
        }

        public Task<string> DownloadTextAsync()
        {
            return Task.FromResult(_events);
        }

        private void AppendEvents(IEnumerable<object> events)
        {
            LogTo.Trace($"{nameof(InMemoryBlob)}.{nameof(AppendEvents)}(events)");
            foreach (var @event in events)
            {
                var eventEnvelope = new EventEnvelope(@event);

                _events += _serializer.Serialize(eventEnvelope);
                _events += Environment.NewLine;
            }
            LogTo.Trace($"Exit {nameof(InMemoryBlob)}.{nameof(AppendEvents)}(events)");
        }
    }
}