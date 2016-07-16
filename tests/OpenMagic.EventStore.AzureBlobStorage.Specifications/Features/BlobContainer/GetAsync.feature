Feature: GetAsync

Scenario: container exists
	Given blob container exists
	When blobContainer.GetAsync(string connectionString, string containerName) is called
	Then BlobContainer should be returned

Scenario: container does not exist
	Given blob container does not exist
	When blobContainer.GetAsync(string connectionString, string containerName) is called
	Then ContainerNotFoundException should be thrown

