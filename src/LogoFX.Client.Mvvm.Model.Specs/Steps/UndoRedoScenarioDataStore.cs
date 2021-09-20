using Attest.Testing.Context.SpecFlow;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    internal sealed class UndoRedoScenarioDataStore : ScenarioDataStoreBase
    {
        public UndoRedoScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public int Value
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int[] Data
        {
            get => GetValue<int[]>();
            set => SetValue(value);
        }

        public SimpleEditableModel Child
        {
            get => GetValue<SimpleEditableModel>();
            set => SetValue(value);
        }
    }
}
