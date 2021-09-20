namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents service that allows shutting down the application.
    /// </summary>
    public interface IShutdownService
    {
        /// <summary>
        /// Shuts down the application.
        /// </summary>
        void Shutdown();
    }
}