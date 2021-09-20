using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using Solid.Core;
using Solid.Practices.Composition;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    public class TestBootstrapper : TestBootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<TestConductorViewModel>
    {
        public TestBootstrapper() :
            base(new ExtendedSimpleContainerAdapter(), new BootstrapperCreationOptions
            {
                UseApplication = false,
                ReuseCompositionInformation = true
            })
        {
            this.UseResolver();
            ((IInitializable)this).Initialize();
        }

        public override CompositionOptions CompositionOptions => new CompositionOptions
        {
            Prefixes = new [] { "LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels"}
        };
    }
}
