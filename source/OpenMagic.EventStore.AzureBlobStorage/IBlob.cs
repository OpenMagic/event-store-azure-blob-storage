using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public interface IBlob
    {
        string Name { get; }

        Task AppendEventsAsync(IEnumerable<object> events);
        Task<BlobMetadata> GetMetadataAsync();
        Task<IEnumerable<object>> ReadEventsAsync();
        Task UpdateMetadataAsync(BlobMetadata blobMetadata);

        // todo: remove this
        Task<string> DownloadTextAsync();
    }
}