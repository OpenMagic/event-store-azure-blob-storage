using System.Collections.Generic;
using System.Linq;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies
{
    public class DummyEvent
    {
        public DummyEvent()
            : this(RandomString.Next())
        {
        }

        public DummyEvent(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public static IEnumerable<object> Create(int blobCount)
        {
            return Enumerable.Range(1, blobCount).Select(i => new DummyEvent());
        }
    }
}