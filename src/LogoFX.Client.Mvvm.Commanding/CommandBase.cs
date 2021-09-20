namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Base class for ICommand implementations
    /// </summary>
    public abstract class CommandBase
        : CommandBase<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// </summary>
        public CommandBase() : this(true) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// </summary>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        public CommandBase(bool isActive)
            : base(isActive) {}

        #region Additional Methods

        /// <summary>
        /// Returns <c>true</c> if the command can be executed; <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool CanExecute()
        {
            return IsActive && OnCanExecute();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public virtual void Execute()
        {
            // here we go directly to OnExecute - also we call OnCommandExecuted as this is not called from the base class
            if (CanExecute())
            {
                OnExecute();
                OnCommandExecuted(new CommandEventArgs(null));
            }
        }

        /// <summary>
        /// Executes the command with the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public override void Execute(object parameter)
        {
            // we redirect this to the non-parameter version, this also ensures we don't call OnCommandExecuted twice
            Execute();      
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <returns></returns>
        protected abstract bool OnCanExecute();

        /// <summary>
        /// Override to inject custom logic during execution.
        /// </summary>
        protected abstract void OnExecute();

        #endregion

        #region Overrides

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <returns></returns>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        protected override bool OnCanExecute(object parameter)
        {
            return CanExecute();      // ignores the parameter
        }

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void OnExecute(object parameter)
        {
            Execute();              // ignores the parameter
        }

        #endregion
    }
}