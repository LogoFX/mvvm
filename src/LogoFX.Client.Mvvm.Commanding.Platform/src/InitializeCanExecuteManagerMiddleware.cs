using Solid.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// This middleware initializes the <see cref="ICanExecuteManager"/> factory
    /// </summary>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
    public class InitializeCanExecuteManagerMiddleware<TBootstrapper> : IMiddleware<TBootstrapper>
        where TBootstrapper : class, IHaveRegistrator
    {
        /// <inheritdoc/>        
        public TBootstrapper Apply(TBootstrapper @object)
        {
            CanExecuteManagerFactoryContext.Current = new CanExecuteManagerFactory<CanExecuteManager>();
            return @object;
        }
    }
}
