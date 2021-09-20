using Attest.Testing.Context.SpecFlow;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    public sealed class CompositeScenarioDataStore : ScenarioDataStoreBase
    {
        public CompositeScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public TestEditableScreenCompositeObjectViewModel SystemUnderTest
        {
            get => GetValue<TestEditableScreenCompositeObjectViewModel>();
            set => SetValue(value);
        }

        public CompositeEditableModel Model
        {
            get => GetValue<CompositeEditableModel>();
            set => SetValue(value);
        }

        public FakeMessageService MockMessageService
        {
            get => GetValue<FakeMessageService>();
            set => SetValue(value);
        }
    }
}