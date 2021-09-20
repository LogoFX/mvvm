using Attest.Testing.Context.SpecFlow;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    internal sealed class SimpleEditableScenarioDataStore : ScenarioDataStoreBase
    {
        public SimpleEditableScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public string PropertyName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
