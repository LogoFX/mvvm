using System;
using System.Windows.Input;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents an <see cref="ICommand">ICommand</see> whose execution can be handled in the View.
    /// </summary>
    public interface IReverseCommand
		 : ICommand
    {
        /// <summary>
        /// Occurs when the <see cref="ICommand">ICommand</see> is executed.
        /// </summary>
        event EventHandler<CommandEventArgs> CommandExecuted;
    }
}
