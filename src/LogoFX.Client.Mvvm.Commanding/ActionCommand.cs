using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Command implementation which allows custom notifications, composite execution predicates, etc.
    /// </summary>
    public class ActionCommand
         : CommandBase
    {
        private readonly Func<bool> _canExecuteHandler;
        private readonly Action _executeHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        public ActionCommand(Action executeHandler)
            : this(executeHandler, null, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        public ActionCommand(Action executeHandler, bool isActive)
            : this(executeHandler, null, isActive) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        /// <param name="canExecuteHandler">The can execute handler.</param>
        public ActionCommand(Action executeHandler, Func<bool> canExecuteHandler)
            : this(executeHandler, canExecuteHandler, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        /// <param name="canExecuteHandler">The can execute handler.</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        public ActionCommand(Action executeHandler, Func<bool> canExecuteHandler, bool isActive)
            : base(isActive)
        {
            Guard.ArgumentNotNull(executeHandler, "executeHandler");

            _executeHandler = executeHandler;
            _canExecuteHandler = canExecuteHandler;
        }

        #region Overrides

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <returns></returns>
        protected override bool OnCanExecute()
        {
            return _canExecuteHandler != null ? _canExecuteHandler() : true;
        }

        /// <summary>
        /// Override to inject custom logic during execution.
        /// </summary>
        protected override void OnExecute()
        {
            _executeHandler();
        }

        #endregion

        /// <summary>
        /// Specifies the condition that must be satisfied for command execution
        /// </summary>
        /// <param name="condition">Condition to be satisfied</param>
        /// <returns>Command condition</returns>
        public static ICommandCondition<ActionCommand> When(Func<bool> condition)
        {
            return new Condition(condition);
        }

        /// <summary>
        /// Specifies the action to be run on command execution
        /// </summary>
        /// <param name="execute">Action to be run</param>
        /// <returns>Extended command</returns>
        public static IExtendedCommand Do(Action execute)
        {
            return new ActionCommand(execute, () => true);
        }
    }
}
