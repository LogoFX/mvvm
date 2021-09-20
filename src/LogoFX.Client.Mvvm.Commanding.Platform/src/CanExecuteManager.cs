using System;
#if NET || NETCORE || NETFRAMEWORK
using System.Windows.Input;
#endif

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <inheritdoc />
    public class CanExecuteManager : ICanExecuteManager
    {
        /// <inheritdoc />
        public EventHandler CanExecuteHandler { get; private set; }

#if WINDOWS_UWP || NETFX_CORE

        /// <inheritdoc />
        public void AddHandler(EventHandler eventHandler)
        {            
            CanExecuteHandler += eventHandler;
        }

        /// <inheritdoc />
        public void RemoveHandler(EventHandler eventHandler)
        {            
            CanExecuteHandler -= eventHandler;
        }
#endif

#if NET || NETCORE || NETFRAMEWORK
        /// <inheritdoc />
        public void AddHandler(EventHandler eventHandler)
        {
            CommandManager.RequerySuggested += eventHandler;
            CanExecuteHandler += eventHandler;
        }

        /// <inheritdoc />
        public void RemoveHandler(EventHandler eventHandler)
        {
            CommandManager.RequerySuggested -= eventHandler;
            CanExecuteHandler -= eventHandler;
        }
#endif
    }    
}
