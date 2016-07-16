using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using OpenMagic.EventStore.AzureBlobStorage.Exceptions;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public class EventStore : IEventStore
    {
        private readonly IBlobContainer _blobContainer;

        public EventStore(string connectionString, string containerName)
            : this(BlobContainer.GetAsync(connectionString, containerName).Result)
        {
        }

        public EventStore(IBlobContainer blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public async Task<IEnumerable<object>> GetEventsAsync(Type aggregateType, string aggregateId)
        {
            var blob = await _blobContainer.GetBlobAsync(aggregateType, aggregateId);
            var events = await blob.ReadEventsAsync();

            return events;
        }

        public Task SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
        {
            LogTo.Trace($"{nameof(SaveEventsAsync)}({aggregateType}, {aggregateId}, {expectedVersion}, events");
            return SaveEventsAsync(aggregateType, aggregateId, expectedVersion, events.ToArray());
        }

        private async Task SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, object[] events)
        {
            LogTo.Trace($"{nameof(SaveEventsAsync)}({aggregateType}, {aggregateId}, {expectedVersion}, {events.Length}");

            ValidateEvents(events);
            ValidateExpectedVersionIsNotLessThanZero(expectedVersion);

            var blob = await GetOrCreateBlobAsync(aggregateType, aggregateId, expectedVersion);

            await ValidateExpectedVersionMatchesActualVersionNumberAsync(aggregateType, aggregateId, expectedVersion, blob);

            await blob.AppendEventsAsync(events);
            await UpdateVersionNumberAsync(blob, expectedVersion + events.Length);

            LogTo.Trace($"Exit {nameof(SaveEventsAsync)}({aggregateType}, {aggregateId}, {expectedVersion}, {events.Length}");
        }

        private async Task<IBlob> CreateBlobAsync(Type aggregateType, string aggregateId)
        {
            LogTo.Trace($"{nameof(CreateBlobAsync)}({aggregateType}, {aggregateId}");
            try
            {
                return await _blobContainer.CreateBlobAsync(aggregateType, aggregateId);
            }
            catch (BlobExistsException exception)
            {
                throw new ConcurrencyException("expectedVersion cannot be '0' when the aggregate exists.", aggregateType, aggregateId, exception);
            }
            finally
            {
                LogTo.Trace($"Exit {nameof(CreateBlobAsync)}({aggregateType}, {aggregateId}");
            }
        }

        private async Task<IBlob> GetBlobAsync(Type aggregateType, string aggregateId, int expectedVersion)
        {
            LogTo.Trace($"{nameof(GetBlobAsync)}({aggregateType}, {aggregateId}, {expectedVersion}");
            try
            {
                return await _blobContainer.GetBlobAsync(aggregateType, aggregateId);
            }
            catch (BlobNotFoundException exception)
            {
                throw new ConcurrencyException($"expectedVersion cannot be '{expectedVersion}' for new aggregates, it must be '0'.", aggregateType, aggregateId, exception);
            }
        }

        private Task<IBlob> GetOrCreateBlobAsync(Type aggregateType, string aggregateId, int expectedVersion)
        {
            return expectedVersion == 0
                ? CreateBlobAsync(aggregateType, aggregateId)
                : GetBlobAsync(aggregateType, aggregateId, expectedVersion);
        }

        private static async Task UpdateVersionNumberAsync(IBlob blob, int versionNumber)
        {
            LogTo.Trace($"{nameof(UpdateVersionNumberAsync)}({blob.Name}, {versionNumber})");

            var metadata = await blob.GetMetadataAsync();
            metadata.VersionNumber = versionNumber;
            await blob.UpdateMetadataAsync(metadata);
        }

        private static void ValidateEvents(object[] events)
        {
            if (events.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(events), events, "Value cannot be empty.");
            }
        }

        private static void ValidateExpectedVersionIsNotLessThanZero(int expectedVersion)
        {
            if (expectedVersion < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedVersion), expectedVersion, "Value cannot be less than 0.");
            }
        }

        private static async Task ValidateExpectedVersionMatchesActualVersionNumberAsync(Type aggregateType, string aggregateId, int expectedVersion, IBlob blob)
        {
            LogTo.Trace($"{nameof(ValidateExpectedVersionMatchesActualVersionNumberAsync)}({aggregateType}, {aggregateId}, {expectedVersion}, {blob.Name}");

            var metadata = await blob.GetMetadataAsync();
            var versionNumber = metadata.VersionNumber;

            if (versionNumber != expectedVersion)
            {
                throw new ConcurrencyException($"expectedVersion cannot be '{expectedVersion}' because the aggregate's version is '{versionNumber}'.", aggregateType, aggregateId);
            }

            LogTo.Trace($"Exit {nameof(ValidateExpectedVersionMatchesActualVersionNumberAsync)}({aggregateType}, {aggregateId}, {expectedVersion}, {blob.Name}");
        }
    }
}