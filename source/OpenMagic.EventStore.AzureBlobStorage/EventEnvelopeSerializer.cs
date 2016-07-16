using System;
using System.IO;
using System.Text;
using Anotar.LibLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public class EventEnvelopeSerializer : IEventEnvelopeSerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public EventEnvelopeSerializer(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public object Deserialize(string serializedEventEnvelope)
        {
            try
            {
                using (var stringReader = new StringReader(serializedEventEnvelope))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    var jObject = GetJObject(jsonReader);
                    var eventType = GetEventType(jObject);
                    var @event = GetEvent(jObject, eventType);

                    return @event;
                }
            }
            catch (Exception exception)
            {
                LogTo.Error($"{serializedEventEnvelope.StartsWith("{")} {serializedEventEnvelope}");
                LogTo.Error($"{string.Join(", ", Encoding.ASCII.GetBytes(serializedEventEnvelope))}");

                throw new Exception($"Could not deserialize '{serializedEventEnvelope}'", exception);
            }
        }

        public void Serialize(StreamWriter streamWriter, EventEnvelope envelope)
        {
            _jsonSerializer.Serialize(streamWriter, envelope);
        }

        private object GetEvent(JObject jObject, Type eventType)
        {
            try
            {
                var eventAsJToken = jObject[nameof(EventEnvelope.Event)];
                var @event = eventAsJToken.ToObject(eventType, _jsonSerializer);
                return @event;
            }
            catch (Exception exception)
            {
                throw new Exception($"Could not get event. jObject: '{jObject}', eventType: '{eventType}'.", exception);
            }
        }

        private static Type GetEventType(JObject jObject)
        {
            try
            {
                var eventTypeAsString = jObject[nameof(EventEnvelope.Type)].Value<string>();
                var eventType = Type.GetType(eventTypeAsString);
                return eventType;
            }
            catch (Exception exception)
            {
                throw new Exception($"Could not get event type '{jObject}'", exception);
            }
        }

        private JObject GetJObject(JsonReader jsonReader)
        {
            try
            {
                return _jsonSerializer.Deserialize<JObject>(jsonReader);
            }
            catch (Exception exception)
            {
                throw new Exception($"Could not get JObject '{jsonReader}'.", exception);
            }
        }
    }
}