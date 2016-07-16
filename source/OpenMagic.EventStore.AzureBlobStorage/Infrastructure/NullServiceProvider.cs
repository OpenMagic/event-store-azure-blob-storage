using System;
using NullGuard;

namespace OpenMagic.EventStore.AzureBlobStorage.Infrastructure
{
    internal class NullServiceProvider : IServiceProvider
    {
        [return: AllowNull]
        public object GetService(Type serviceType)
        {
            return null;
        }
    }
}