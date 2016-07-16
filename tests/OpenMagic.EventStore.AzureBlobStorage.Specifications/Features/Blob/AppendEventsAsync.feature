Feature: AppendEventsAsync

Scenario: empty blob
	Given an empty blob
	And 5 events
	When blob.AppendEventsAsync(IEnumerable<object> events) is called
	Then the blob should contain 5 lines

Scenario: blob exists
	Given a blob with 10 lines
	And 5 events
	When blob.AppendEventsAsync(IEnumerable<object> events) is called
	Then the blob should contain 15 lines
