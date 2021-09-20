using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// The platform-independent abstraction of command execution predicate manager.
    /// </summary>
    public interface ICanExecuteManager
    {
        /// <summary>
        /// Gets the execution predicate handler.
        /// </summary>
        EventHandler CanExecuteHandler { get; }
        /// <summary>
        /// Adds an event handler to the execution predicate.
        /// </summary>
        /// <param name="eventHandler"></param>
        void AddHandler(EventHandler eventHandler);

        /// <summary>
        /// Removes event handler from the execution predicate.
        /// </summary>
        /// <param name="eventHandler"></param>
        void RemoveHandler(EventHandler eventHandler);
    }
}
