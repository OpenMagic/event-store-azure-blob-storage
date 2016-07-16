Feature: GetBlobAsync

Scenario: blob exists
	Given an aggregate type
	And an aggregateId
	And blob exists
	When blobContainer.GetBlobAsync(Type aggregateType, string aggregateId) is called
	Then Blob should be returned

Scenario: blob does not exist
	Given blob does not exist
	When blobContainer.GetBlobAsync(Type aggregateType, string aggregateId) is called
	Then BlobNotFoundException should be thrown
