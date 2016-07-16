Feature: GetMetadataAsync

Scenario: blob exists
	Given an empty blob
	And blob.Metadata[versionnumber] is 10
	When blob.GetMetadataAsync() is called
	Then BlobMetadata should be return
	And VersionNumber should be 10

Scenario: empty blob
	Given an empty blob
	When blob.GetMetadataAsync() is called
	Then BlobMetadata should be return
	And VersionNumber should be 0
