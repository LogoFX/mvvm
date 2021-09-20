using Solid.Bootstrapping;
using Solid.Extensibility;

namespace LogoFX.Client.Mvvm.ViewModelFactory
{
    /// <summary>
    /// The bootstrapper extension methods.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the view model factory.
        /// </summary>        
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
        /// <typeparam name="TViewModelFactory">The type of the view model factory.</typeparam>
        public static TBootstrapper UseViewModelFactory<TBootstrapper, TViewModelFactory>(
            this TBootstrapper bootstrapper)
            where TBootstrapper : class, IExtensible<TBootstrapper>, IHaveRegistrator
            where TViewModelFactory : class, IViewModelFactory
        {
            return bootstrapper.Use(new RegisterViewModelFactoryMiddleware<TBootstrapper, TViewModelFactory>());
        }
    }
}