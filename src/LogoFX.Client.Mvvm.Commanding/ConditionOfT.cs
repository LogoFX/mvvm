using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents command with condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>    
    public class Condition<T> : ICommandCondition<T, ActionCommand<T>>
    {
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition{T}"/> class.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        public Condition(Func<T, bool> canExecute)
        {
            _canExecute = canExecute;
        }

        /// <summary>
        /// Associates the specified action with the command execution.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <returns></returns>
        public ActionCommand<T> Do(Action<T> execute)
        {
            return new ActionCommand<T>(execute, _canExecute);
        }
    }
}
