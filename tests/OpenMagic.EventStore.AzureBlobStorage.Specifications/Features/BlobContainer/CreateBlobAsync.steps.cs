using LazyCache;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.BlobContainer
{
    [Binding]
    public class CreateBlobAsyncSteps : IntegrationStepsBase
    {
        public CreateBlobAsyncSteps(IntegrationGiven given, Actual actual, DummyFactory dummy, IAppCache cache)
            : base(given, actual, dummy, cache)
        {
        }

        [When(@"BlobContainer\.CreateBlobAsync\(Type aggregateType, string aggregateId\) is called")]
        public void WhenBlobContainer_CreateBlobAsyncTypeAggregateTypeStringAggregateIdIsCalled()
        {
            Actual.TryCatch(() => Actual.Blob = Given.BlobContainer.CreateBlobAsync(Given.AggregateType, Given.AggregateId).Result);
            Actual.TryCatch(() => Actual.Blob = Given.BlobContainer.CreateBlobAsync(Given.AggregateType, Given.AggregateId).Result);
        }
    }
}