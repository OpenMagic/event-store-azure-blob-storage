using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    /// <summary>
    ///     Extends the <see cref="IEventStore" /> interface.
    /// </summary>
    public static class EventStoreExtensions
    {
        /// <summary>
        ///     Save an event for the specified <typeparamref name="TAggregate">aggregate</typeparamref>.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate the event belongs to.</typeparam>
        /// <param name="eventStore">The event store to save event to.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="expectedVersion">The version of the stream that is expected. This is used to control concurrency concerns.</param>
        /// <param name="event">The event to save.</param>
        public static Task SaveEventAsync<TAggregate>(this IEventStore eventStore, string aggregateId, int expectedVersion, object @event)
        {
            return eventStore.SaveEventAsync(typeof(TAggregate), aggregateId, expectedVersion, @event);
        }

        /// <summary>
        ///     Save an event for the specified <paramref name="aggregateType">aggregate</paramref>.
        /// </summary>
        /// <param name="eventStore">The event store to save event to.</param>
        /// <param name="aggregateType">The type of the aggregate the event belongs to.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="expectedVersion">The version of the stream that is expected. This is used to control concurrency concerns.</param>
        /// <param name="event">The event to save.</param>
        public static Task SaveEventAsync(this IEventStore eventStore, Type aggregateType, string aggregateId, int expectedVersion, object @event)
        {
            return eventStore.SaveEventsAsync(aggregateType, aggregateId, expectedVersion, new[] {@event});
        }

        /// <summary>
        ///     Save events for the specified <typeparamref name="TAggregate">aggregate</typeparamref>.
        /// </summary>
        /// <param name="eventStore">The event store to save events to.</param>
        /// <typeparam name="TAggregate">The type of the aggregate the events belong to.</typeparam>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="expectedVersion">The version of the stream that is expected. This is used to control concurrency concerns.</param>
        /// <param name="events">The events to save.</param>
        public static Task SaveEventsAsync<TAggregate>(this IEventStore eventStore, string aggregateId, int expectedVersion, IEnumerable<object> events)
        {
            return eventStore.SaveEventsAsync(typeof(TAggregate), aggregateId, expectedVersion, events);
        }
    }
}