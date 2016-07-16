using System;
using System.Threading.Tasks;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public interface IBlobContainer
    {
        Task<IBlob> CreateBlobAsync(Type aggregateType, string aggregateId);
        Task<IBlob> GetBlobAsync(Type aggregateType, string aggregateId);
    }
}