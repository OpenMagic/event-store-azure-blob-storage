using System;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public class Actual
    {
        public IBlob Blob { get; set; }
        public BlobContainer BlobContainer { get; set; }
        public object Event { get; set; }
        public object[] Events { get; set; }
        public Exception Exception { get; set; }
        public BlobMetadata Metadata { get; set; }
        public object Result { get; set; }

        public void TryCatch(Action action)
        {
            Exception = null;

            try
            {
                action();
            }
            catch (AggregateException exception)
            {
                if (ThrowException(exception.InnerException))
                {
                    throw exception.InnerException;
                }
                Exception = exception.InnerException;
            }
            catch (Exception exception)
            {
                if (ThrowException(exception))
                {
                    throw;
                }
                Exception = exception;
            }
        }

        private static bool ThrowException(Exception exception)
        {
            return exception != null && (exception is NotImplementedException || exception.GetType().Name == "ActivationException");
        }
    }
}