using Solid.Bootstrapping;
using Solid.Extensibility;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer
{
    /// <summary>
    /// The bootstrapper extension methods.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the view model factory which is based on <see cref="LogoFX.Practices.IoC.SimpleContainer"/>.
        /// </summary>        
        /// <param name="bootstrapper">The bootstrapper.</param>
        public static TBootstrapper UseViewModelFactory<TBootstrapper>(
            this TBootstrapper bootstrapper)
            where TBootstrapper : class, IExtensible<TBootstrapper>, IHaveRegistrator
        {
            return bootstrapper.UseViewModelFactory<TBootstrapper, ViewModelFactory>();
        }
    }
}
