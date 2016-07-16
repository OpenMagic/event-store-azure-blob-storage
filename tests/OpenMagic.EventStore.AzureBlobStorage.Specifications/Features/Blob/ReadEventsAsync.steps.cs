using System.Linq;
using FluentAssertions;
using LazyCache;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.Blob
{
    [Binding]
    public class ReadEventsAsyncSteps : IntegrationStepsBase
    {
        public ReadEventsAsyncSteps(IntegrationGiven given, Actual actual, DummyFactory dummy, IAppCache cache)
            : base(given, actual, dummy, cache)
        {
        }

        [Given(@"(.*) events are appended to the blob")]
        public void GivenEventsAreAppendedToTheBlob(int eventCount)
        {
            var events = DummyEvent.Create(eventCount).ToArray();
            Given.Events = (Given.Events ?? Enumerable.Empty<object>()).Concat(events);
            Given.Blob.AppendEventsAsync(events).Wait();
        }

        [When(@"blob\.ReadEventsAsync\(\) is called")]
        public void WhenBlob_ReadEventsAsyncIsCalled()
        {
            Actual.TryCatch(() => Actual.Events = Given.Blob.ReadEventsAsync().Result.ToArray());
        }

        [Then(@"the result should be equivalent to the given events")]
        public void ThenTheResultShouldBeEquivalentToTheGivenEvents()
        {
            Actual.Exception.Should().BeNull();
            Actual.Events.ShouldAllBeEquivalentTo(Given.Events);
        }
    }
}