using Attest.Testing.Context.SpecFlow;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    public sealed class SimpleScenarioDataStore : ScenarioDataStoreBase
    {
        public SimpleScenarioDataStore(
            ScenarioContext scenarioContext) : 
            base(scenarioContext)
        {
        }

        public TestEditableScreenSimpleObjectViewModel SystemUnderTest
        {
            get => GetValue<TestEditableScreenSimpleObjectViewModel>();
            set => SetValue(value);
        }

        public SimpleEditableModel Model
        {
            get => GetValue<SimpleEditableModel>();
            set => SetValue(value);
        }

        public FakeMessageService MockMessageService
        {
            get => GetValue<FakeMessageService>();
            set => SetValue(value);
        }

        public bool WasDirtyRaised
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool WasCancelChangesRaised
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }
}
