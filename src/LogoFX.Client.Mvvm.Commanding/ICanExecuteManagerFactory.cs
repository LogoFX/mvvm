namespace LogoFX.Client.Mvvm.Commanding
{   
    /// <summary>
    /// The factory for creating instances of <see cref="ICanExecuteManager"/>
    /// </summary>
    public interface ICanExecuteManagerFactory
    {
        /// <summary>
        /// Creates new instance of <see cref="ICanExecuteManager"/>
        /// </summary>
        /// <returns></returns>
        ICanExecuteManager CreateCanExecuteManager();
    }    
}
