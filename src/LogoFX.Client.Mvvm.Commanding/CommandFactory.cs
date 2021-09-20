using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents a command factory, ensuring one-time initialization for fields.
    /// </summary>
    public static class CommandFactory
    {
        /// <summary>
        /// Gets a new action command if it doesn't exist already; 
        /// otherwise returns the existing instance.
        /// </summary>
        /// <param name="field">The underlying command field</param>
        /// <param name="do">What to to when the command is executed.</param>
        /// <param name="when">When the command can be executed.</param>
        /// <returns></returns>
        public static IActionCommand GetCommand(ref IActionCommand field, Action @do, Func<bool> when = null)
        {
            return field ?? (field =
                       when == null ? ActionCommand.When(() => true).Do(@do) : ActionCommand.When(when).Do(@do));
        }
    }
}
