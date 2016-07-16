using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Anotar.LibLog;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public class StepProfiler : IDisposable
    {
        private readonly StepInfo _stepInfo;
        private readonly Stopwatch _stopwatch;
        private readonly List<TaskProfiler> _taskProfilers;

        public StepProfiler(StepInfo stepInfo)
        {
            _stepInfo = stepInfo;
            _taskProfilers = new List<TaskProfiler>();
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            LogTo.Info($"{_stepInfo.StepDefinitionType} {_stepInfo.Text} took {_stopwatch.ElapsedMilliseconds:N0}ms");

            if (!_taskProfilers.Any())
            {
                return;
            }

            var other = _stopwatch.Elapsed;

            foreach (var taskProfiler in _taskProfilers)
            {
                LogTo.Debug($"   - {taskProfiler.Message} took {taskProfiler.Stopwatch.ElapsedMilliseconds:N0}ms");
                other = other.Subtract(taskProfiler.Stopwatch.Elapsed);
            }
            LogTo.Debug($"   - Other took {other.TotalMilliseconds:N0}ms");
        }

        public IDisposable Task(string message)
        {
            var task = new TaskProfiler(message);
            _taskProfilers.Add(task);
            return task;
        }
    }
}