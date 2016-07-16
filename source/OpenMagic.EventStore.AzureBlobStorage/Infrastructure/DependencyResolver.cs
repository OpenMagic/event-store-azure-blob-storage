using JsonNet.PrivateSettersContractResolvers;
using LazyCache;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;

namespace OpenMagic.EventStore.AzureBlobStorage.Infrastructure
{
    internal static class DependencyResolver
    {
        private static readonly StandardKernel Kernel;

        static DependencyResolver()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<IAppCache>().To<CachingService>();
            Kernel.Bind<IBlobNamer>().To<BlobNamer>();
            Kernel.Bind<IContractResolver>().To<PrivateSetterContractResolver>();
            Kernel.Bind<IEventEnvelopeSerializer>().To<EventEnvelopeSerializer>();
            Kernel.Bind<JsonSerializer>().ToMethod(ctx => new JsonSerializer {ContractResolver = ctx.Kernel.Get<IContractResolver>()});
        }

        /// <summary>
        ///     Gets an instance of the requested service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        ///     If defined the custom service is returned, otherwise the default implementation is returned.
        /// </returns>
        internal static TService Get<TService>() where TService : class
        {
            return GetCustomService<TService>() ?? GetDefaultService<TService>();
        }

        private static TService GetCustomService<TService>()
        {
            return ServiceLocator.IsLocationProviderSet
                ? ServiceLocator.Current.GetInstance<TService>()
                : default(TService);
        }

        private static TService GetDefaultService<TService>()
        {
            return Kernel.Get<TService>();
        }
    }
}