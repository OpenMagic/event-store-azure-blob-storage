using Microsoft.WindowsAzure.Storage;

namespace OpenMagic.EventStore.AzureBlobStorage.Exceptions
{
    public class BlobExistsException : AzureBlobStorageException
    {
        public BlobExistsException(string blobName)
            : base(CreateMessage(blobName))
        {
        }

        public BlobExistsException(string blobName, StorageException innerException)
            : base(CreateMessage(blobName), innerException)
        {
        }

        private static string CreateMessage(string blobName)
        {
            return $"Cannot overwrite blob '{blobName}'.";
        }
    }
}