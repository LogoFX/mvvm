using Solid.Bootstrapping;
using Solid.Extensibility;
using Unity;

namespace LogoFX.Client.Mvvm.ViewModelFactory.Unity
{
    /// <summary>
    /// The bootstrapper extension methods.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the view model factory which is based on <see cref="UnityContainer"/>
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
