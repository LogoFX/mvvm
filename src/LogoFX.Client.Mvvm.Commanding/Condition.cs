using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents command with condition.
    /// </summary>    
    public class Condition : ICommandCondition<ActionCommand>
    {
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        public Condition(Func<bool> canExecute)
        {
            _canExecute = canExecute;
        }

        /// <summary>
        /// Associates the specified action with the command execution.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <returns></returns>
        public ActionCommand Do(Action execute)
        {
            return new ActionCommand(execute, _canExecute);
        }
    }
}
