namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// The default implementation of the <see cref="ICanExecuteManagerFactory"/>
    /// </summary>
    public class DefaultCanExecuteManagerFactory : ICanExecuteManagerFactory
    {
        /// <inheritdoc/>
        public ICanExecuteManager CreateCanExecuteManager()
        {
            return new DefaultCanExecuteManager();
        }
    }
}