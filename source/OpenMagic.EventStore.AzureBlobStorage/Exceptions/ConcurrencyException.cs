using System;

namespace OpenMagic.EventStore.AzureBlobStorage.Exceptions
{
    public class ConcurrencyException : AzureBlobStorageException
    {
        public ConcurrencyException(string message, Type aggregateType, string aggregateId)
            : base(message, aggregateType, aggregateId)
        {
        }

        public ConcurrencyException(string message, Type aggregateType, string aggregateId, Exception innerException)
            : base(message, aggregateType, aggregateId, innerException)
        {
        }
    }
}