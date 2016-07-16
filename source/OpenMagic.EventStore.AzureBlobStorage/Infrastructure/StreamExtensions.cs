using System.IO;

namespace OpenMagic.EventStore.AzureBlobStorage.Infrastructure
{
    internal static class StreamExtensions
    {
        internal static Stream GotoBeginning(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}