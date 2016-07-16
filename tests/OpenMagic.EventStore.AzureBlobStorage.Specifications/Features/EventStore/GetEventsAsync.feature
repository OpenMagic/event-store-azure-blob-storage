Feature: GetEventsAsync

Scenario: blob has events
	Given a blob that has 10 events
	When eventStore.GetEventsAsync() is called
	Then the 10 events should be returned
