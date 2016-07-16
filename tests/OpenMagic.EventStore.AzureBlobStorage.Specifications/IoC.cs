using BoDi;
using JsonNet.PrivateSettersContractResolvers;
using LazyCache;
using Newtonsoft.Json;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications
{
    [Binding]
    public class IoC
    {
        private readonly IObjectContainer _objectContainer;

        public IoC(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeAllTests()
        {
            var jsonSerializer = new JsonSerializer {ContractResolver = new PrivateSetterContractResolver()};
            var eventEnvelopeSerializer = new EventEnvelopeSerializer(jsonSerializer);
            var blobContainer = new InMemoryBlobContainer(eventEnvelopeSerializer);
            var eventStore = new EventStore(blobContainer);

            _objectContainer.RegisterInstanceAs(blobContainer, typeof(IBlobContainer));
            _objectContainer.RegisterInstanceAs(eventStore, typeof(IEventStore));
            _objectContainer.RegisterInstanceAs(jsonSerializer, typeof(JsonSerializer));
            _objectContainer.RegisterInstanceAs(new JsonSerializerSettings {ContractResolver = jsonSerializer.ContractResolver}, typeof(JsonSerializerSettings));
            _objectContainer.RegisterInstanceAs(new CachingService(), typeof(IAppCache));
            _objectContainer.RegisterTypeAs<BlobNamer, IBlobNamer>();
        }
    }
}