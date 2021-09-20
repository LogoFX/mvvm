using Attest.Testing.Bootstrapping;
using Attest.Testing.Context.SpecFlow;
using Attest.Testing.Integration;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra;
using Solid.Practices.IoC;
using BootstrapperBase = Attest.Testing.Bootstrapping.BootstrapperBase;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Bootstrapping
{
    internal sealed class Startup : StartupBase<Bootstrapper>
    {
        public Startup(IIocContainer iocContainer)
            : base(iocContainer, c => new Bootstrapper(c)) {}

        protected override void InitializeOverride(Bootstrapper bootstrapper)
        {
            base.InitializeOverride(bootstrapper);
            bootstrapper
                .UseIntegration<BootstrapperBase, StartApplicationService>()
                .UseKeyValueDataStore();
        }
    }
}