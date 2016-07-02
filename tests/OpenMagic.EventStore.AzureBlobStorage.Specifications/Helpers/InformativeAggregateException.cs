using System;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    internal class InformativeAggregateException : AggregateException
    {
        internal InformativeAggregateException(AggregateException exception)
            : base(GetInformativeMessage(exception), exception)
        {
        }

        private static string GetInformativeMessage(AggregateException exception)
        {
            return exception.InnerExceptions.Count == 1 ? exception.InnerException.Message : exception.Message;
        }
    }
}