using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents a command that supports various re-query options 
    /// and is able to receive events on property and collection notifications.
    /// </summary>
    public interface IActionCommand
        : IReverseCommand,
            IReceiveEvent,
            IExtendedCommand,
            INotifyPropertyChanged,
            IDisposableCollection
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        bool IsActive { get; set; }

        /// <summary>
        /// Re-evaluates the can execute value.
        /// </summary>
        void RequeryCanExecute();
    }
}
