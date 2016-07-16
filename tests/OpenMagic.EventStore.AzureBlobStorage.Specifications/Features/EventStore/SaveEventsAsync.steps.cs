using System.Linq;
using System.Text.RegularExpressions;
using Anotar.LibLog;
using FluentAssertions;
using Newtonsoft.Json;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.Extensions;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.EventStore
{
    [Binding]
    public class SaveEventsAsyncSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public SaveEventsAsyncSteps(Given given, Actual actual, IEventStore eventStore, JsonSerializerSettings jsonSerializerSettings)
        {
            _given = given;
            _actual = actual;
            _jsonSerializerSettings = jsonSerializerSettings;
            _given.EventStore = eventStore;
        }

        [Given(@"expectedVersion is (.*)")]
        public void GivenExpectedVersionIs(int expectedVersion)
        {
            _given.ExpectedVersion = expectedVersion;
        }

        [When(@"eventStore\.SaveEventsAsync\(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable(.*) events\)")]
        public void WhenEventStore_SaveEventsAsyncTypeAggregateTypeStringAggregateIdIntExpectedVersionIEnumerableEvents(string p0)
        {
            LogTo.Trace($"{nameof(WhenEventStore_SaveEventsAsyncTypeAggregateTypeStringAggregateIdIntExpectedVersionIEnumerableEvents)}({p0})");

            var events = (_given.Event == null ? Enumerable.Empty<object>() : new[] {_given.Event}).ToArray();

            _actual.TryCatch(() =>
            {
                _given.EventStore
                    .SaveEventsAsync(_given.AggregateType, _given.AggregateId, _given.ExpectedVersion, events)
                    .Wait();
            });
        }

        [Then(@"a new blob should be created")]
        public void ThenANewBlobShouldBeCreated()
        {
            _given.Blob.Should().NotBeNull();
        }

        [Then(@"the blob's VersionNumber should be (.*)")]
        public void ThenTheBlobSVersionNumberShouldBe(int expectedVersionNumber)
        {
            _actual.Exception.Should().BeNull();
            _given.Blob.GetMetadataAsync().Result.VersionNumber.Should().Be(expectedVersionNumber);
        }

        [Then(@"the blob should contain the given event")]
        public void ThenTheBlobShouldContainTheGivenEvent()
        {
            _actual.Exception.Should().BeNull();
            _given.Blob.DownloadTextAsync().Result.Should().Contain(JsonConvert.SerializeObject(_given.Event));
        }

        [Then(@"the blob should contain both given events")]
        public void ThenTheBlobShouldContainBothGivenEvents()
        {
            var json = JsonConvert.SerializeObject(_given.Event, _jsonSerializerSettings);

            _given.Blob.DownloadTextAsync().Result.Occurs(json).Should().Be(2);
        }

        [Then(@"(.*) should be thrown")]
        public void ThenExceptionShouldBeThrown(string expectedType)
        {
            _actual.Exception.Should().NotBeNull();
            _actual.Exception.GetType().Name.Should().Be(expectedType);
        }

        [Then(@"the error message should be expectedVersion cannot be '(.*)' for new aggregates, it must be '(.*)'.")]
        public void ThenTheErrorMessageShouldBeExpectedVersionCannotBeForNewAggregatesItMustBe(int p0, int p1)
        {
            _actual.Exception.Message.Should().Be($"expectedVersion cannot be '{p0}' for new aggregates, it must be '{p1}'.");
        }

        [Then(@"the error message should be")]
        public void ThenTheErrorMessageShouldBe(string expectedMessage)
        {
            _actual.Exception.Message.Should().Be(expectedMessage);
        }

        [Then(@"the error message should be expectedVersion cannot be '(.*)' when the aggregate exists\.")]
        public void ThenTheErrorMessageShouldBeExpectedVersionCannotBeWhenTheAggregateExists_(int p0)
        {
            _actual.Exception.Message.Should().Be($"expectedVersion cannot be '{p0}' when the aggregate exists.");
        }

        [Then(@"the error message should be expectedVersion cannot be '(.*)' because the aggregate\'s version is '(.*)'\.")]
        public void ThenTheErrorMessageShouldBeExpectedVersionCannotBeBecauseTheAggregatesVersionIs_(int p0, int p1)
        {
            _actual.Exception.Message.Should().Be($"expectedVersion cannot be '{p0}' because the aggregate's version is '{p1}'.");
        }

        [Then(@"the blob should contain (.*) line feed")]
        public void ThenTheBlobShouldContainLineFeed(int p0)
        {
            var content = _given.Blob.DownloadTextAsync().Result;

            Regex.Matches(content, Regex.Escape("\n")).Count.Should().Be(p0);
        }

        [Then(@"the blob should contain (.*) carriage return")]
        public void ThenTheBlobShouldContainCarriageReturn(int p0)
        {
            var content = _given.Blob.DownloadTextAsync().Result;

            Regex.Matches(content, Regex.Escape("\r")).Count.Should().Be(p0);
        }

        [Then(@"the blob should end with carriage return line feed")]
        public void ThenTheBlobShouldEndWithCarriageReturnLineFeed()
        {
            _given.Blob.DownloadTextAsync().Result.Should().EndWith("\r\n");
        }
    }
}