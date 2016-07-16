using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public abstract class StepsBase
    {
        protected readonly Actual Actual;
        protected readonly DummyFactory Dummy;
        protected readonly Given Given;

        protected StepsBase(Given given, Actual actual, DummyFactory dummy)
        {
            Given = given;
            Actual = actual;
            Dummy = dummy;
        }
    }
}