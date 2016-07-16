using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using LazyCache;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OpenMagic.EventStore.AzureBlobStorage.Exceptions;
using OpenMagic.EventStore.AzureBlobStorage.Infrastructure;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public class Blob : IBlob
    {
        private readonly CloudAppendBlob _blobReference;
        private readonly IEventEnvelopeSerializer _serializer;
        private BlobMetadata _metadata;

        private Blob(CloudAppendBlob blobReference, IEventEnvelopeSerializer serializer)
        {
            _blobReference = blobReference;
            _serializer = serializer;
        }

        public string Name => _blobReference.Name;

        public Task AppendEventsAsync(IEnumerable<object> events)
        {
            LogTo.Trace($"{nameof(AppendEventsAsync)}(events)");
            return AppendEventEnvelopesAsync(events.Select(e => new EventEnvelope(e)));
        }

        public Task<BlobMetadata> GetMetadataAsync()
        {
            if (_metadata != null)
            {
                return Task.FromResult(_metadata);
            }

            _blobReference.FetchAttributes();
            _metadata = new BlobMetadata(_blobReference.Metadata);

            return Task.FromResult(_metadata);
        }

        public async Task<IEnumerable<object>> ReadEventsAsync()
        {
            var events = new List<object>();

            using (var stream = await _blobReference.OpenReadAsync())
            using (var reader = new StreamReader(stream))
            {
                string line;

                while (!string.IsNullOrWhiteSpace(line = await reader.ReadLineAsync()))
                {
                    object @event;
                    try
                    {
                        @event = _serializer.Deserialize(line);
                    }
                    catch (Exception exception)
                    {
                        LogTo.Error($"{line.StartsWith("{")} {line}");
                        throw new Exception($"Cannot deserialize event[{events.Count}] '{line}'", exception);
                    }
                    events.Add(@event);
                }
            }

            return events;
        }

        public Task UpdateMetadataAsync(BlobMetadata blobMetadata)
        {
            return _blobReference.SetMetadataAsync();
        }

        public Task<string> DownloadTextAsync()
        {
            throw new NotImplementedException();
        }

        private async Task AppendEventEnvelopesAsync(IEnumerable<EventEnvelope> eventEnvelopes)
        {
            LogTo.Trace($"{nameof(AppendEventEnvelopesAsync)}(eventEnvelopes)");

            foreach (var eventEnvelope in eventEnvelopes)
            {
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    _serializer.Serialize(streamWriter, eventEnvelope);

                    streamWriter.WriteLine();
                    streamWriter.Flush();

                    memoryStream.GotoBeginning();

                    await _blobReference.AppendBlockAsync(memoryStream);
                }
            }
        }

        public static async Task<Blob> CreateAsync(CloudBlobContainer container, string name, IEventEnvelopeSerializer serializer, IAppCache cache)
        {
            var blobReference = container.GetAppendBlobReference(name);
            var blobExists = await blobReference.ExistsAsync(cache);

            if (blobExists)
            {
                throw new BlobExistsException(name);
            }

            await blobReference.CreateOrReplaceAsync(AccessCondition.GenerateIfNotExistsCondition(), null, null);

            blobReference.Exists(cache, true);

            return new Blob(blobReference, serializer);
        }

        public static async Task<IBlob> GetAsync(CloudBlobContainer container, string name, IEventEnvelopeSerializer serializer, IAppCache cache)
        {
            var blobReference = container.GetAppendBlobReference(name);

            if (await blobReference.ExistsAsync(cache))
            {
                return new Blob(blobReference, serializer);
            }

            throw new BlobNotFoundException(name);
        }
    }
}