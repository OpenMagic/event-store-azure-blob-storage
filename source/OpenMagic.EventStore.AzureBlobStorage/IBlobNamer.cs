using System;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public interface IBlobNamer
    {
        string GetBlobName(Type aggregateType, string aggregateId);
    }
}