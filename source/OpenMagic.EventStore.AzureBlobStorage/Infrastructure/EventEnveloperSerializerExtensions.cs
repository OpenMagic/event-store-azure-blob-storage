using System.IO;
using Anotar.LibLog;

namespace OpenMagic.EventStore.AzureBlobStorage.Infrastructure
{
    internal static class EventEnveloperSerializerExtensions
    {
        internal static string Serialize(this IEventEnvelopeSerializer serializer, EventEnvelope eventEnvelope)
        {
            LogTo.Trace($"{nameof(Serialize)}({nameof(eventEnvelope)}");

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                serializer.Serialize(streamWriter, eventEnvelope);
                streamWriter.Flush();

                using (var streamReader = new StreamReader(memoryStream.GotoBeginning()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}