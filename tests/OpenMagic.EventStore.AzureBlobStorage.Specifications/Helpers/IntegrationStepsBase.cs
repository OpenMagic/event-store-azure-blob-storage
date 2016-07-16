using LazyCache;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public abstract class IntegrationStepsBase
    {
        protected readonly Actual Actual;
        protected readonly IAppCache Cache;
        protected readonly DummyFactory Dummy;
        protected readonly IntegrationGiven Given;
        protected readonly ScenarioContext ScenarioContext;

        protected IntegrationStepsBase(IntegrationGiven given, Actual actual, DummyFactory dummy, IAppCache cache, ScenarioContext scenarioContext = null)
        {
            Given = given;
            Actual = actual;
            Dummy = dummy;
            Cache = cache;
            ScenarioContext = scenarioContext;
        }

        protected StepProfiler StepProfiler()
        {
            return new StepProfiler(ScenarioContext.StepContext.StepInfo);
        }
    }
}