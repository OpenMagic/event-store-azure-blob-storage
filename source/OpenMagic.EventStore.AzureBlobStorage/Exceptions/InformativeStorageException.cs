using System;
using Microsoft.WindowsAzure.Storage;

namespace OpenMagic.EventStore.AzureBlobStorage.Exceptions
{
    /// <summary>
    ///     Wrap a <see cref="StorageException" /> to provide an error message more likely to
    ///     inform the developer of the 'real' cause of the exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InformativeStorageException : Exception
    {
        public InformativeStorageException(StorageException exception)
            : base(GetInformativeMessage(exception), exception)
        {
        }

        private static string GetInformativeMessage(StorageException exception)
        {
            try
            {
                return exception.RequestInformation.HttpStatusMessage;
            }
            catch (Exception ex)
            {
                return $"Could not create information message '{ex.Message}'.";
            }
        }
    }
}