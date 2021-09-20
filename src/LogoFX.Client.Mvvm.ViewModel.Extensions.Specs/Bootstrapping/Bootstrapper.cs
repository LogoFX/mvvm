using Attest.Testing.Bootstrapping;
using Solid.Practices.Composition;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Bootstrapping
{
    internal sealed class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper(IDependencyRegistrator dependencyRegistrator) : base(dependencyRegistrator)
        {
        }

        public override CompositionOptions CompositionOptions => new CompositionOptions
        {
            Prefixes = new[]
            {
                //TODO: Check
                "LogoFX.Client.Mvvm.ViewModel.Extensions.Specs",
                "LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra",
                "LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Modules"
            }
        };
    }
}