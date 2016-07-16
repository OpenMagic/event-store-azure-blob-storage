Feature: SaveEventsAsync

Scenario: Save events for new aggregate
	Given an aggregate type
	And an aggregateId
	And expectedVersion is 0
	And an event
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then a new blob should be created
	And the blob's VersionNumber should be 1
	And the blob should contain the given event

Scenario: Add events to existing aggregate
	Given an aggregate type
	And an aggregateId
	And expectedVersion is 0
	And an event
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Given expectedVersion is 1
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then the blob's VersionNumber should be 2
	And the blob should contain both given events

Scenario: Save events for new aggregate when expectedVersion is not 0
	Given an aggregate type
	And an aggregateId
	And expectedVersion is 1
	And an event
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then ConcurrencyException should be thrown
	And the error message should be expectedVersion cannot be '1' for new aggregates, it must be '0'.

Scenario: expectedVersion is less than 0
	Given an aggregate type
	And an aggregateId
	And expectedVersion is -1
	And an event
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then ArgumentOutOfRangeException should be thrown
	And the error message should be
		"""
		Value cannot be less than 0.
		Parameter name: expectedVersion
		Actual value was -1.
		"""

Scenario: events is empty
	Given an aggregate type
	And an aggregateId
	And expectedVersion is 0
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then ArgumentOutOfRangeException should be thrown
	And the error message should be
		"""
		Value cannot be empty.
		Parameter name: events
		Actual value was System.Object[].
		"""

Scenario: expectedVersion is 0 and aggregate exists
	Given an aggregate type
	And an aggregateId
	And expectedVersion is 0
	And an event
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then ConcurrencyException should be thrown
	And the error message should be expectedVersion cannot be '0' when the aggregate exists.

Scenario: expectedVersion does not match
	Given an aggregate type
	And an aggregateId
	And expectedVersion is 0
	And an event
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Given expectedVersion is 2
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then ConcurrencyException should be thrown
	And the error message should be expectedVersion cannot be '2' because the aggregate's version is '1'.

Scenario: event contains new line
	Given an aggregate type
	And an aggregateId
	And expectedVersion is 0
	And an event that contains a new line character
	When eventStore.SaveEventsAsync(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable<object> events)
	Then a new blob should be created
	And the blob's VersionNumber should be 1
	And the blob should contain the given event
	And the blob should contain 1 carriage return
	And the blob should contain 1 line feed
	And the blob should end with carriage return line feed

