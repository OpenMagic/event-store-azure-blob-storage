using System;
using System.Collections.Generic;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public class Given
    {
        private readonly IBlobContainer _blobContainer;

        public Given(IBlobContainer blobContainer, IEventStore eventStore)
        {
            _blobContainer = blobContainer;
            EventStore = eventStore;
        }

        public Type AggregateType { get; set; }
        public string AggregateId { get; set; }
        public object Event { get; set; }
        public IEventStore EventStore { get; set; }
        public int ExpectedVersion { get; set; }
        public IEnumerable<object> Events { get; internal set; }
        public IBlob Blob => _blobContainer.GetBlobAsync(AggregateType, AggregateId).Result;
        public string SerializedEvent { get; set; }

        public Type AnAggregateType()
        {
            AggregateType = typeof(DummyAggregate);
            return AggregateType;
        }

        public string AnAggregateId()
        {
            AggregateId = Guid.NewGuid().ToString();
            return AggregateId;
        }
    }
}