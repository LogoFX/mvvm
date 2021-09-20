using Attest.Testing.Context.SpecFlow;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    internal sealed class SimpleDirtyScenarioDataStore : ScenarioDataStoreBase
    {
        public SimpleDirtyScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public SimpleEditableModel Child
        {
            get => GetValue<SimpleEditableModel>();
            set => SetValue(value);
        }
    }
}
