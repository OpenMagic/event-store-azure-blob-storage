using System;
using OpenMagic.Extensions;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public class BlobNamer : IBlobNamer
    {
        public virtual string GetBlobName(Type aggregateType, string aggregateId)
        {
            return $"{GetAggregateName(aggregateType)}/{aggregateId}";
        }

        private static string GetAggregateName(Type aggregateType)
        {
            var name = aggregateType.Name;

            return name.TrimEnd("Aggregate").InsertStringBeforeEachUpperCaseCharacter("-").ToLower();
        }
    }
}