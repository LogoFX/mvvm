namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// The Ambient Context for <see cref="ICanExecuteManagerFactory"/>
    /// </summary>
    public static class CanExecuteManagerFactoryContext
    {
        private static ICanExecuteManagerFactory _canExecuteManagerFactory = new DefaultCanExecuteManagerFactory();

        /// <summary>
        /// Gets or sets the current <see cref="ICanExecuteManagerFactory"/> implementation.
        /// </summary>
        public static ICanExecuteManagerFactory Current
        {
            get => _canExecuteManagerFactory;
            set => _canExecuteManagerFactory = value;
        }
    }
}