using Attest.Testing.Context.SpecFlow;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    internal sealed class CompositeDirtyScenarioDataStore : ScenarioDataStoreBase
    {
        public CompositeDirtyScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public CompositeEditableModel Child
        {
            get => GetValue<CompositeEditableModel>();
            set => SetValue(value);
        }

        public CompositeEditableModel FirstChild
        {
            get => GetValue<CompositeEditableModel>();
            set => SetValue(value);
        }

        public CompositeEditableModel SecondChild
        {
            get => GetValue<CompositeEditableModel>();
            set => SetValue(value);
        }

        public SimpleEditableModel GrandChild
        {
            get => GetValue<SimpleEditableModel>();
            set => SetValue(value);
        }

        public SimpleEditableModel FirstGrandChild
        {
            get => GetValue<SimpleEditableModel>();
            set => SetValue(value);
        }

        public SimpleEditableModel SecondGrandChild
        {
            get => GetValue<SimpleEditableModel>();
            set => SetValue(value);
        }
    }
}