using Attest.Testing.Lifecycle.SpecFlow;
using BoDi;
using JetBrains.Annotations;
using Solid.Practices.IoC;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Bootstrapping
{
    [Binding, UsedImplicitly]
    internal sealed class LifecycleHook : LifecycleHookBase
    {
        public LifecycleHook(
            ObjectContainer objectContainer)
            : base(objectContainer)
        {
        }

        protected override void InitializeContainer(IIocContainer iocContainer)
        {
            new Startup(iocContainer).Initialize();
        }

        [AfterTestRun, UsedImplicitly]
        public new static void AfterAllScenarios()
        {
            LifecycleHookBase.AfterAllScenarios();
        }
    }
}
