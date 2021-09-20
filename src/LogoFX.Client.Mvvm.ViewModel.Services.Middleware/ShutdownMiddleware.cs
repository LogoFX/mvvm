using Solid.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Middleware that's responsible for registering 
    /// <see cref="IShutdownService"/> into the ioc container registrator.
    /// </summary>
    public class ShutdownMiddleware<TBootstrapper> : 
        IMiddleware<TBootstrapper> where TBootstrapper : class, IHaveRegistrator
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public TBootstrapper
            Apply(TBootstrapper @object)
        {
            if (@object is IShutdownService)
            {
                @object.Registrator.RegisterInstance(typeof(IShutdownService), @object);
            }            
            return @object;
        }
    }
}