using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Default implementation of the <see cref="ICanExecuteManager"/>
    /// </summary>
    public class DefaultCanExecuteManager : ICanExecuteManager
    {
        /// <inheritdoc/>
        public EventHandler CanExecuteHandler { get; private set; }

        /// <inheritdoc/>
        public void AddHandler(EventHandler eventHandler)
        {
            CanExecuteHandler += eventHandler;
        }

        /// <inheritdoc/>
        public void RemoveHandler(EventHandler eventHandler)
        {
            CanExecuteHandler -= eventHandler;
        }
    }
}