using System;
using System.Collections;
using System.Collections.Generic;
using NullGuard;

namespace OpenMagic.EventStore.AzureBlobStorage.Exceptions
{
    public class AzureBlobStorageException : Exception
    {
        private readonly Dictionary<string, object> _data;

        public AzureBlobStorageException(string message)
            : base(message)
        {
        }

        public AzureBlobStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AzureBlobStorageException(string message, Type aggregateType, string aggregateId)
            : this(message, CreateDataDictionary(aggregateType, aggregateId))
        {
        }

        public AzureBlobStorageException(string message, Type aggregateType, string aggregateId, Exception innerException)
            : this(message, CreateDataDictionary(aggregateType, aggregateId), innerException)
        {
        }

        public AzureBlobStorageException(string message, Dictionary<string, object> data)
            : base(message)
        {
            _data = data;
        }

        public AzureBlobStorageException(string message, Dictionary<string, object> data, Exception innerException)
            : base(message, innerException)
        {
            _data = data;
        }


        public override IDictionary Data
        {
            [return: AllowNull] get { return _data; }
        }


        private static Dictionary<string, object> CreateDataDictionary(Type aggregateType, string aggregateId)
        {
            return new Dictionary<string, object>
            {
                {nameof(aggregateType), aggregateType},
                {nameof(aggregateId), aggregateId}
            };
        }
    }
}