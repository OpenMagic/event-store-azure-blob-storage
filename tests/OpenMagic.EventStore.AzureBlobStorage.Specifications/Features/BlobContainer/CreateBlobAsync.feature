Feature: CreateBlobAsync

Scenario: blob exists
	Given blob exists
	When BlobContainer.CreateBlobAsync(Type aggregateType, string aggregateId) is called
	Then BlobExistsException should be thrown

Scenario: blob does not exist
	Given blob does not exist
	When BlobContainer.CreateBlobAsync(Type aggregateType, string aggregateId) is called
	Then Blob should be returned
