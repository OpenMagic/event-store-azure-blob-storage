using FluentAssertions;
using LazyCache;
using OpenMagic.EventStore.AzureBlobStorage.Infrastructure;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.BlobContainer
{
    [Binding]
    public class GetAsyncSteps : IntegrationStepsBase
    {
        public GetAsyncSteps(IntegrationGiven given, Actual actual, DummyFactory dummy, IAppCache cache)
            : base(given, actual, dummy, cache)
        {
        }

        [Given(@"blob container exists")]
        public void GivenBlobContainerExists()
        {
            if (Cache.GetOrAdd(Given.CloudBlobContainer.StorageUri.PrimaryUri.ToString(), () => Given.CloudBlobContainer.Exists()))
            {
                return;
            }

            Given.CloudBlobContainer.Create();
            Cache.Replace(Given.CloudBlobContainer.StorageUri.PrimaryUri.ToString(), Given.CloudBlobContainer.Exists());
        }

        [When(@"blobContainer\.GetAsync\(string connectionString, string containerName\) is called")]
        public void WhenBlobContainer_GetAsyncStringConnectionStringStringContainerNameIsCalled()
        {
            Actual.TryCatch(() => Actual.BlobContainer = AzureBlobStorage.BlobContainer.GetAsync(Given.ConnectionString, Given.ContainerName).Result);
        }

        [Then(@"BlobContainer should be returned")]
        public void ThenBlobContainerShouldBeReturned()
        {
            Actual.Exception.Should().BeNull();
            Actual.BlobContainer.Should().NotBeNull();
        }
    }
}