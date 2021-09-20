using Attest.Testing.Context;
using JetBrains.Annotations;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    public sealed class CommonScenarioDataStore<TRootObject> 
    {
        private readonly RootObjectScenarioDataStore _rootObjectScenarioDataStore;

        public CommonScenarioDataStore(RootObjectScenarioDataStore rootObjectScenarioDataStore)
        {
            _rootObjectScenarioDataStore = rootObjectScenarioDataStore;
        }

        public TRootObject RootObject => (TRootObject)_rootObjectScenarioDataStore.RootObject;
    }
}