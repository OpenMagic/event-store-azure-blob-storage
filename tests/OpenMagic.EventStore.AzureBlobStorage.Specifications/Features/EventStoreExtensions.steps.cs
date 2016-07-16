using System;
using System.Linq;
using Anotar.LibLog;
using FakeItEasy;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features
{
    [Binding]
    public class EventStoreExtensionsSteps
    {
        private readonly Given _given;

        public EventStoreExtensionsSteps(Given given)
        {
            _given = given;
        }

        [Given(@"an event store")]
        public void GivenAnEventStore()
        {
            _given.EventStore = A.Fake<IEventStore>();
        }

        [Given(@"an aggregate type")]
        public void GivenAnAggregateType()
        {
            _given.AnAggregateType();
        }

        [Given(@"an aggregateId")]
        public void GivenAnAggregateId()
        {
            _given.AnAggregateId();
        }

        [Given(@"an expectedVersion")]
        public void GivenAnExpectedVersion()
        {
            _given.ExpectedVersion = RandomNumber.NextInt();
        }

        [Given(@"an event")]
        public void GivenAnEvent()
        {
            _given.Event = new DummyEvent();
            _given.Events = (_given.Events ?? Enumerable.Empty<object>()).Concat(new[] {_given.Event});
        }

        [Given(@"an event that contains a new line character")]
        public void GivenAnEventThatContainsANewLineCharacter()
        {
            _given.Event = new DummyEvent("message with\nnew line and\r\nCrLf");
        }

        [When(@"eventStore\.SaveEventAsync\<TAggregate\>\(this IEventStore eventStore, string aggregateId, int expectedVersion, object @event\) is called")]
        public void WhenEventStore_SaveEventAsyncTAggregateThisIEventStoreEventStoreStringAggregateIdObjectEventIsCalled()
        {
            if (_given.AggregateType != typeof(DummyAggregate))
            {
                throw new NotImplementedException($"Support for AggregateType {_given.AggregateType} has not been implemented.");
            }
            _given.EventStore.SaveEventAsync<DummyAggregate>(_given.AggregateId, _given.ExpectedVersion, _given.Event);
        }

        [When(@"eventStore\.SaveEventAsync\(this IEventStore eventStore, Type aggregateType, string aggregateId, int expectedVersion, object @event\)")]
        public void WhenEventStore_SaveEventAsyncThisIEventStoreEventStoreTypeAggregateTypeStringAggregateIdObjectEvent()
        {
            _given.EventStore.SaveEventAsync(_given.AggregateType, _given.AggregateId, _given.ExpectedVersion, _given.Event);
        }

        [When(@"eventStore\.SaveEventsAsync\<TAggregate\>\(this IEventStore eventStore, string aggregateId, int expectedVersion, IEnumerable\<object\> events\)")]
        public void WhenEventStore_SaveEventsAsyncTAggregateThisIEventStoreEventStoreStringAggregateIdIEnumerableEvents()
        {
            if (_given.AggregateType != typeof(DummyAggregate))
            {
                throw new NotImplementedException($"Support for AggregateType {_given.AggregateType} has not been implemented.");
            }
            _given.EventStore.SaveEventsAsync<DummyAggregate>(_given.AggregateId, _given.ExpectedVersion, new[] {_given.Event});
        }

        [Then(@"eventStore\.SaveEventsAsync\(Type aggregateType, string aggregateId, int expectedVersion, IEnumerable\<object\> events\) should be called")]
        public void ThenEventStore_SaveEventsAsyncTypeAggregateTypeStringAggregateIdIEnumerableEventsShouldBeCalled()
        {
            LogTo.Trace("ThenEventStore_SaveEventsAsyncTypeAggregateTypeStringAggregateIdIEnumerableEventsShouldBeCalled()");
            A.CallTo(() => _given.EventStore.SaveEventsAsync(_given.AggregateType, _given.AggregateId, _given.ExpectedVersion, A<object[]>.That.Matches(events => events.Length == 1 && events[0].Equals(_given.Event))))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}