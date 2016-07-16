using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using LazyCache;
using Microsoft.WindowsAzure.Storage;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Features.Blob
{
    [Binding]
    public class AppendEventsAsyncSteps : IntegrationStepsBase
    {
        public AppendEventsAsyncSteps(IntegrationGiven given, Actual actual, DummyFactory dummy, IAppCache cache, ScenarioContext scenarioContext)
            : base(given, actual, dummy, cache, scenarioContext)
        {
        }

        [Given(@"a blob with (.*) lines")]
        public void GivenABlobWithLines(int lineCount)
        {
            using (var profiler = StepProfiler())
            {
                var lines = Enumerable.Range(1, lineCount).Select(i => RandomString.Next());
                var content = string.Join("\n", lines) + "\n";
                var cloudAppendBlob = Given.CloudAppendBlob;

                using (profiler.Task($"Create {cloudAppendBlob.Uri}"))
                {
                    cloudAppendBlob.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition());
                }

                using (profiler.Task($"Appending text to {cloudAppendBlob.Uri}"))
                {
                    cloudAppendBlob.AppendText(content);
                }
            }
        }

        [Given(@"(.*) events")]
        public void GivenEvents(int eventCount)
        {
            using (StepProfiler())
            {
                Given.Events = DummyEvent.Create(eventCount);
            }
        }

        [When(@"blob\.AppendEventsAsync\(IEnumerable\<object\> events\) is called")]
        public void WhenBlob_AppendEventsAsyncIEnumerableEventsIsCalled()
        {
            using (StepProfiler())
            {
                Actual.TryCatch(() => Given.Blob.AppendEventsAsync(Given.Events).Wait());
            }
        }

        [Then(@"the blob should contain (.*) lines")]
        public void ThenTheBlobShouldContainLines(int expectedLineCount)
        {
            using (StepProfiler())
            {
                Actual.Exception.Should().BeNull();
                GetCloudAppendBlobLineCount().Should().Be(expectedLineCount);
            }
        }

        private int GetCloudAppendBlobLineCount()
        {
            return GetCloudAppendBlobLines().Count();
        }

        private IEnumerable<string> GetCloudAppendBlobLines()
        {
            using (var stream = Given.CloudAppendBlob.OpenRead(AccessCondition.GenerateIfExistsCondition()))
            using (var reader = new StreamReader(stream))
            {
                string line;

                while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                {
                    yield return line;
                }
            }
        }
    }
}