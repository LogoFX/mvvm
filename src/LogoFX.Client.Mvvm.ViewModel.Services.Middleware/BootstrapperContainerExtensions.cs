using Solid.Bootstrapping;
using Solid.Extensibility;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Bootstrapper extensions.
    /// </summary>
    public static class BootstrapperContainerExtensions
    {
        /// <summary>
        /// Uses the view model creator service middleware.
        /// </summary>
        /// <param name="bootstrapperContainer">The bootstrapper container.</param>
        /// <returns></returns>
        public static TBootstrapper
            UseViewModelCreatorService<TBootstrapper>(this TBootstrapper bootstrapperContainer)
            where TBootstrapper : class, IExtensible<TBootstrapper>, IHaveRegistrator
        {
            return bootstrapperContainer.Use(
                new RegisterViewModelCreatorServiceMiddleware<TBootstrapper>());            
        }

        /// <summary>
        /// Uses the shutdown middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper container.</param>
        /// <returns></returns>
        public static TBootstrapper
            UseShutdown<TBootstrapper>(this TBootstrapper bootstrapper) 
            where TBootstrapper : class, IExtensible<TBootstrapper>, IHaveRegistrator
        {
            return bootstrapper.Use(
                new ShutdownMiddleware<TBootstrapper>());
        }
    }
}