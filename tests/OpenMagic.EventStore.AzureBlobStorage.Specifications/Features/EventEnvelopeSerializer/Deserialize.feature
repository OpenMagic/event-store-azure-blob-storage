Feature: Deserialize

Scenario: Happy path
	Given an event
	And the event is serialized
	When eventEnvelopeSerializer.Deserialize(string eventEnvelopeAsString) is called
	Then the result should be equivalent to the given event
