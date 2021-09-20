using System;
using System.ComponentModel;
using Attest.Testing.Context.SpecFlow;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    internal sealed class ModelScenarioDataStore : ScenarioDataStoreBase
    {
        public ModelScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public INotifyPropertyChanged Model
        {
            get => GetValue<INotifyPropertyChanged>();
            set => SetValue(value);
        }

        public WeakReference IsErrorRaisedRef
        {
            get => GetValue<WeakReference>();
            set => SetValue(value);
        }

        public WeakReference IsDirtyRaisedRef
        {
            get => GetValue<WeakReference>();
            set => SetValue(value);
        }
    }
}
