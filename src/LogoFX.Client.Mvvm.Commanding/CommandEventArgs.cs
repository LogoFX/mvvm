using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents event arguments with passed command parameter.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class CommandEventArgs
		 : EventArgs
    {
        private readonly object _commandParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEventArgs"/> class.
        /// </summary>
        /// <param name="commandParameter">The command parameter.</param>
        public CommandEventArgs(object commandParameter)
        {
            _commandParameter = commandParameter;
        }

        /// <summary>
        /// Gets the command parameter.
        /// </summary>
        /// <value>
        /// The command parameter.
        /// </value>
        public object CommandParameter
        {
            get { return _commandParameter; }
        }
    }
}
