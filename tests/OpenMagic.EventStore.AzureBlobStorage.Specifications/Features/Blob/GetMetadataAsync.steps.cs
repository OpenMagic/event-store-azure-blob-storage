using FluentAssertions;
using LazyCache;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.Blob
{
    [Binding]
    public class GetMetadataAsyncSteps : IntegrationStepsBase
    {
        public GetMetadataAsyncSteps(IntegrationGiven given, Actual actual, DummyFactory dummy, IAppCache cache)
            : base(given, actual, dummy, cache)
        {
        }

        [Given(@"a blob with '(.*)' events")]
        public void GivenABlobWithEvents(int blobCount)
        {
            Given.BlobContainer.CreateBlobAsync(Given.AggregateType, Given.AggregateId).Wait();
            Given.Blob.AppendEventsAsync(DummyEvent.Create(blobCount)).Wait();
        }

        [Given(@"blob\.Metadata\[versionnumber] is (.*)")]
        public void GivenBlob_MetadataVersionnumberIs(int versionNumber)
        {
            var cloudAppendBlob = Given.CloudAppendBlob;

            cloudAppendBlob.FetchAttributes();
            cloudAppendBlob.Metadata[BlobMetadata.VersionNumberKey] = versionNumber.ToString();
            cloudAppendBlob.SetMetadata();
        }

        [Given(@"an empty blob")]
        public void GivenAnEmptyBlob()
        {
            Given.BlobContainer.CreateBlobAsync(Given.AggregateType, Given.AggregateId).Wait();
        }

        [When(@"blob\.GetMetadataAsync\(\) is called")]
        public void WhenBlob_GetMetadataAsyncIsCalled()
        {
            Actual.TryCatch(() => Actual.Metadata = Given.Blob.GetMetadataAsync().Result);
        }

        [Then(@"BlobMetadata should be return")]
        public void ThenBlobMetadataShouldBeReturn()
        {
            Actual.Exception.Should().BeNull();
            Actual.Metadata.Should().NotBeNull();
        }

        [Then(@"VersionNumber should be (.*)")]
        public void ThenVersionNumberShouldBe(int p0)
        {
            Actual.Exception.Should().BeNull();
            Actual.Metadata.VersionNumber.Should().Be(p0);
        }
    }
}