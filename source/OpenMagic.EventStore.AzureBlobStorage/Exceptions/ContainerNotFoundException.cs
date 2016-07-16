using System;

namespace OpenMagic.EventStore.AzureBlobStorage.Exceptions
{
    public class ContainerNotFoundException : Exception
    {
        public ContainerNotFoundException(string containerName)
            : base($"Cannot find container '{containerName}'.")
        {
        }
    }
}