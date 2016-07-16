using FluentAssertions;
using LazyCache;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.BlobContainer
{
    [Binding]
    public class GetBlobAsyncSteps : IntegrationStepsBase
    {
        public GetBlobAsyncSteps(IntegrationGiven given, Actual actual, DummyFactory dummy, IAppCache cache)
            : base(given, actual, dummy, cache)
        {
        }

        [Given(@"blob exists")]
        public void GivenBlobExists()
        {
            Given.BlobContainer.CreateBlobAsync(Given.AggregateType, Given.AggregateId).Wait();
        }

        [Given(@"blob container does not exist")]
        public void GivenBlobContainerDoesNotExist()
        {
            Given.ContainerName = "container-that-does-not-exist";
        }

        [When(@"blobContainer.GetBlobAsync\(Type aggregateType, string aggregateId\) is called")]
        public void WhenGetBlobAsyncTypeAggregateTypeStringAggregateIdIsCalled()
        {
            Actual.TryCatch(() => { Actual.Blob = Given.BlobContainer.GetBlobAsync(Given.AggregateType, Given.AggregateId).Result; });
        }

        [Then(@"Blob should be returned")]
        public void ThenBlobShouldBeReturned()
        {
            Actual.Blob.Should().NotBeNull();
        }
    }
}