Feature: EventStoreExtensions

Scenario: SaveEventAsync<TAggregate>(this IEventStore eventStore, string aggregateId, int expectedVersion, object @event)
	Given an event store
	And an aggregate type
	And an aggregateId
	And an expectedVersion
	And an event
	When eventStore.SaveEventAsync<TAggregate>(this IEventStore eventStore, string aggregateId, int expectedVersion, object @event) is called
	Then eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events) should be called

Scenario: SaveEventAsync(this IEventStore eventStore, Type aggregateType, string aggregateId, int expectedVersion, object @event)
	Given an event store
	And an aggregate type
	And an aggregateId
	And an expectedVersion
	And an event
	When eventStore.SaveEventAsync(this IEventStore eventStore, Type aggregateType, string aggregateId, int expectedVersion, object @event)
	Then eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events) should be called

Scenario: SaveEventsAsync<TAggregate>(this IEventStore eventStore, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Given an event store
	And an aggregate type
	And an aggregateId
	And an expectedVersion
	And an event
	When eventStore.SaveEventsAsync<TAggregate>(this IEventStore eventStore, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events) should be called
