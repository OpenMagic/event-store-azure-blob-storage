Feature: ReadEventsAsync

Scenario: empty blob
	Given an empty blob
	And 5 events are appended to the blob
	When blob.ReadEventsAsync() is called
	Then the result should be equivalent to the given events

Scenario: blob exists
	Given an empty blob
	And 5 events are appended to the blob
	And 10 events are appended to the blob
	When blob.ReadEventsAsync() is called
	Then the result should be equivalent to the given events
