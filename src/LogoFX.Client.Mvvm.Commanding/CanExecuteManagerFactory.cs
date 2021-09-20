namespace LogoFX.Client.Mvvm.Commanding
{
    /// <inheritdoc/>    
    /// <typeparam name="T">The type of the <see cref="ICanExecuteManager"/> instance to be created.</typeparam>
    public class CanExecuteManagerFactory<T> : ICanExecuteManagerFactory where T : ICanExecuteManager, new()
    {
        /// <inheritdoc/>
        public ICanExecuteManager CreateCanExecuteManager()
        {
            return new T();
        }
    }
}