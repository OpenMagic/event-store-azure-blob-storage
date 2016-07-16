using System;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public class EventEnvelope
    {
        public EventEnvelope(object @event)
        {
            Type = @event.GetType();
            Event = @event;
        }

        public Type Type { get; }
        public object Event { get; set; }
    }
}