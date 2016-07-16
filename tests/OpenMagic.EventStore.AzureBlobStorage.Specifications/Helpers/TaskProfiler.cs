using System;
using System.Diagnostics;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public class TaskProfiler : IDisposable
    {
        public TaskProfiler(string message)
        {
            Message = message;
            Stopwatch = Stopwatch.StartNew();
        }

        public string Message { get; }
        public Stopwatch Stopwatch { get; }

        public void Dispose()
        {
            Stopwatch.Stop();
        }
    }
}