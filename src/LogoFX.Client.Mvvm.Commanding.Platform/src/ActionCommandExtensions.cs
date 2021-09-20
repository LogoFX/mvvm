using System;
using System.Windows.Input;
using LogoFX.Client.Mvvm.Core;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// The platform-specific <see cref="ActionCommand"/> extensions.
    /// </summary>
    public static class ActionCommandExtensions
    {
        /// <summary>
        /// Queries for command state according to the specified property notifications
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="source">Source of property notifications</param>
        /// <param name="path">Property path</param>
        /// <returns>Command after the setup</returns>
        public static T RequeryOnPropertyChanged<T>(this T command, object source, string path)
            where
            T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotDefault(source, "source");
            source.NotifyOn(path, (a, b) => command.ReceiveWeakEvent(new EventArgs()));
            return command;
        }
    }
}
