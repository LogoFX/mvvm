using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Generic implementation of <see cref="ActionCommand"/>
    /// </summary>
    /// <typeparam name="T">Type of command parameter</typeparam>
    public class ActionCommand<T>
         : CommandBase<T>
    {
        #region Declarations

        private readonly Func<T, bool> _canExecuteHandler;
        private readonly Action<T> _executeHandler;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand{T}"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        public ActionCommand(Action<T> executeHandler)
            : this(executeHandler, null, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand{T}"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        public ActionCommand(Action<T> executeHandler, bool isActive)
            : this(executeHandler, null, isActive) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand{T}"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        /// <param name="canExecuteHandler">The can execute handler.</param>
        public ActionCommand(Action<T> executeHandler, Func<T, bool> canExecuteHandler)
            : this(executeHandler, canExecuteHandler, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand{T}"/> class.
        /// </summary>
        /// <param name="executeHandler">The execute handler.</param>
        /// <param name="canExecuteHandler">The can execute handler.</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        public ActionCommand(Action<T> executeHandler, Func<T, bool> canExecuteHandler, bool isActive)
            : base(isActive)
        {
            Guard.ArgumentNotNull(executeHandler, "executeHandler");

            _executeHandler = executeHandler;
            _canExecuteHandler = canExecuteHandler;
        }

        #region Override

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <returns></returns>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        protected override bool OnCanExecute(T parameter)
        {
            return _canExecuteHandler != null ? _canExecuteHandler(parameter) : true;
        }

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void OnExecute(T parameter)
        {
            _executeHandler(parameter);
        }

        #endregion

        /// <summary>
        /// Specifies the condition that must be satisfied for command execution
        /// </summary>
        /// <param name="condition">Condition to be satisfied</param>
        /// <returns>Command condition</returns>
        public static ICommandCondition<T, ActionCommand<T>> When(Func<T, bool> condition)
        {
            return new Condition<T>(condition);
        }

        /// <summary>
        /// Specifies the action to be run on command execution
        /// </summary>
        /// <param name="execute">Action to be run</param>
        /// <returns>Extended command</returns>
        public static IExtendedCommand Do(Action<T> execute)
        {
            return new ActionCommand<T>(execute, o => true);
        }
    }
}
