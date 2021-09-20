using System;
using System.Windows.Input;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents command with parameter after it has been setup with execution condition.
    /// </summary>
    /// <typeparam name="TParameter">Type of command parameter.</typeparam>
    /// <typeparam name="TCommand">Type of command.</typeparam>
    public interface ICommandCondition<TParameter, out TCommand> where TCommand: ICommand
    {
        /// <summary>
        /// Associates the specified action with the command execution.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <returns></returns>
        TCommand Do(Action<TParameter> execute);
    }

    /// <summary>
    /// Represents command after it has been setup with execution condition.
    /// </summary>
    /// <typeparam name="TCommand">Type of command.</typeparam>
    public interface ICommandCondition<out TCommand> where TCommand: ICommand
    {
        /// <summary>
        /// Associates the specified action with the command execution.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <returns></returns>
        TCommand Do(Action execute);
    }
}
