using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMagic.EventStore.AzureBlobStorage.Exceptions;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    /// <summary>
    ///     Store and read events for an aggregate.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        ///     Get the events for the specified aggregate.
        /// </summary>
        /// <param name="aggregateType">The type of the aggregate the events belong to.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        Task<IEnumerable<object>> GetEventsAsync(Type aggregateType, string aggregateId);

        /// <summary>
        ///     Save events for the specified aggregate.
        /// </summary>
        /// <param name="aggregateType">The type of the aggregate the events belong to.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="expectedVersion">The version of the stream that is expected. This is used to control concurrency concerns.</param>
        /// <param name="events">The events to save.</param>
        /// <exception cref="ConcurrencyException"> is thrown when <paramref name="expectedVersion" /> does not match aggregates current version.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> is thrown when <paramref name="expectedVersion" /> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> is thrown when <paramref name="expectedVersion" /> when <paramref name="events" /> is empty.</exception>
        Task SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events);
    }
}