using FluentAssertions;
using OpenMagic.EventStore.AzureBlobStorage.Infrastructure;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.EventEnvelopeSerializer
{
    [Binding]
    public class DeserializeSteps : StepsBase
    {
        private readonly AzureBlobStorage.EventEnvelopeSerializer _serializer;

        public DeserializeSteps(Given given, Actual actual, DummyFactory dummy, AzureBlobStorage.EventEnvelopeSerializer serializer)
            : base(given, actual, dummy)
        {
            _serializer = serializer;
        }

        [Given(@"the event is serialized")]
        public void GivenTheEventIsSerialized()
        {
            var eventEnvelope = new EventEnvelope(Given.Event);

            Given.SerializedEvent = _serializer.Serialize(eventEnvelope);
        }

        [When(@"eventEnvelopeSerializer\.Deserialize\(string eventEnvelopeAsString\) is called")]
        public void WhenEventEnvelopeSerializer_DeserializeStringEventEnvelopeAsStringIsCalled()
        {
            Actual.TryCatch(() => Actual.Event = _serializer.Deserialize(Given.SerializedEvent));
        }

        [Then(@"the result should be equivalent to the given event")]
        public void ThenTheResultShouldBeEquivalentToTheGivenEvent()
        {
            Actual.Exception.Should().BeNull();
            Actual.Event.ShouldBeEquivalentTo(Given.Event);
        }
    }
}