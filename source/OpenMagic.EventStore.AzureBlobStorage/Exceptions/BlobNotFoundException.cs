namespace OpenMagic.EventStore.AzureBlobStorage.Exceptions
{
    public class BlobNotFoundException : AzureBlobStorageException
    {
        public BlobNotFoundException(string blobName)
            : base($"Cannot find blob '{blobName}'.")
        {
        }
    }
}