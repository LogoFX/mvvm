using Solid.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Mvvm.ViewModelFactory
{
    /// <summary>
    /// Middleware that registers view model factory.
    /// </summary>    
    public class RegisterViewModelFactoryMiddleware<TBootstrapper, TViewModelFactory> :
        IMiddleware<TBootstrapper>
        where TBootstrapper : class, IHaveRegistrator
        where TViewModelFactory : class, IViewModelFactory
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public TBootstrapper
            Apply(TBootstrapper @object)
        {
            @object.Registrator.RegisterSingleton<IViewModelFactory, TViewModelFactory>();
            return @object;
        }
    }
}