using Attest.Testing.Context.SpecFlow;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    internal sealed class SimpleValidationScenarioDataStore : ScenarioDataStoreBase
    {
        public SimpleValidationScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public SimpleTestValueObject ValueObject
        {
            get => GetValue<SimpleTestValueObject>();
            set => SetValue(value);
        }
    }
}
