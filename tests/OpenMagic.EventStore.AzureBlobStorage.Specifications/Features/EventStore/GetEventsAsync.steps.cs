using System.Linq;
using Anotar.LibLog;
using FluentAssertions;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using OpenMagic.Extensions;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.EventStore
{
    [Binding]
    public class GetEventsAsyncSteps : StepsBase
    {
        public GetEventsAsyncSteps(Given given, Actual actual, DummyFactory dummy)
            : base(given, actual, dummy)
        {
        }

        [Given(@"a new blob")]
        public void GivenANewBlob()
        {
            // nothing to do
        }

        [Given(@"a blob that has (.*) events")]
        public void GivenABlobThatHasEvents(int events)
        {
            LogTo.Trace($"GivenABlobThatHasEvents({events})");

            Given.AnAggregateType();
            Given.AnAggregateId();
            Given.Events = Enumerable.Range(1, events).Select(i => new DummyEvent()).ToArray();
            Given.EventStore.SaveEventsAsync(Given.AggregateType, Given.AggregateId, 0, Given.Events).Wait();
        }

        [Given(@"blob does not exist")]
        public void GivenBlobDoesNotExist()
        {
            Given.AnAggregateType();
            Given.AnAggregateId();
        }

        [When(@"eventStore\.GetEventsAsync\(\) is called")]
        public void WhenEventStore_GetEventsAsyncIsCalled()
        {
            Actual.TryCatch(() => { Actual.Events = Given.EventStore.GetEventsAsync(Given.AggregateType, Given.AggregateId).Result.ToArray(); });
        }

        [Then(@"the (.*) events should be returned")]
        public void ThenTheEventsShouldBeReturned(int p0)
        {
            Actual.Exception.Should().BeNull();
            Actual.Events.ShouldAllBeEquivalentTo(Given.Events);
        }

        [Then(@"the error message should start with (.*)")]
        public void ThenTheErrorMessageShouldStartWith(string p0)
        {
            Actual.Exception.Message.Should().StartWith(p0.TrimStart("{").TrimEnd("}"));
        }

        [Then(@"the error message should end with (.*)")]
        public void ThenTheErrorMessageShouldEndWith_(string p0)
        {
            Actual.Exception.Message.Should().EndWith(p0.TrimStart("{").TrimEnd("}"));
        }
    }
}