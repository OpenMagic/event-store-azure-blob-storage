using System.IO;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public interface IEventEnvelopeSerializer
    {
        object Deserialize(string serializedEventEnvelope);
        void Serialize(StreamWriter streamWriter, EventEnvelope envelope);
    }
}