using Solid.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Middleware that's responsible for registering <see cref="IViewModelCreatorService"/> into the ioc container registrator.
    /// </summary>
    public class RegisterViewModelCreatorServiceMiddleware<TBootstrapper> :
        IMiddleware<TBootstrapper> where TBootstrapper : class, IHaveRegistrator
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public TBootstrapper Apply(
            TBootstrapper @object)
        {
            @object.Registrator.RegisterSingleton<IViewModelCreatorService, ViewModelCreatorService>();
            return @object;
        }
    }
}
