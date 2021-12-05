using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Base class for <see cref="IActionCommand"/> with parameter implementations
    /// </summary>
    /// <typeparam name="T">Type of command parameter</typeparam>
    public abstract class CommandBase<T>
        : IActionCommand
    {
        /// <summary>
        /// The error message for invalid parameter type.
        /// </summary>
        protected const string ERROR_EXPECTED_TYPE = "Expected parameter for command ({0}) must be of {1} type";
   
        private readonly ICanExecuteManager _canExecuteManager;
        private bool _isActive = true;

        private Uri _imageUri;
        private bool _isAdvanced;
        private string _name;
        private string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase{T}"/> class.
        /// </summary>
        protected CommandBase()
        {
            _canExecuteManager = CanExecuteManagerFactoryContext.Current.CreateCanExecuteManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase{T}"/> class.
        /// </summary>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        protected CommandBase(bool isActive)
            : this()
        {
            _isActive = isActive;
        }

        #region Additional Methods

        /// <summary>
        /// Returns <c>true</c> if the command can be executed; <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool CanExecute(T parameter)
        {
            return IsActive && OnCanExecute(parameter);
        }

        /// <summary>
        /// Executes the command with the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public virtual void Execute(T parameter)
        {
            if (CanExecute(parameter))
            {
                OnExecute(parameter);
                OnCommandExecuted(new CommandEventArgs(parameter));
            }
        }

        /// <summary>
        /// Override to inject custom logic during execution condition re-evaluation.
        /// </summary>
        protected virtual void OnRequeryCanExecute()
        {
            if (_canExecuteManager.CanExecuteHandler != null) _canExecuteManager.CanExecuteHandler(this, EventArgs.Empty);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <returns></returns>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        protected abstract bool OnCanExecute(T parameter);

        /// <summary>
        /// Override to inject custom logic during execution condition evaluation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected abstract void OnExecute(T parameter);

#endregion

#region IActionCommand Members

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged.Notify(() => IsActive);
                    RequeryCanExecute();
                }
            }
        }

        /// <summary>
        /// Re-evaluates the can execute value.
        /// </summary>
        public void RequeryCanExecute()
        {
            OnRequeryCanExecute();
        }

#endregion

#region IReverseCommand Members

        /// <summary>
        /// Occurs when the <see cref="ICommand">ICommand</see> is executed.
        /// </summary>
        public event EventHandler<CommandEventArgs> CommandExecuted;

#endregion

#region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            CheckParameterType(parameter);
            return CanExecute(ParseParameter(parameter, typeof(T)));
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add => _canExecuteManager.AddHandler(value);
            remove => _canExecuteManager.RemoveHandler(value);
        }

        void ICommand.Execute(object parameter)
        {
            CheckParameterType(parameter);
            Execute(ParseParameter(parameter, typeof(T)));
        }

#endregion

#region IReceiveEvent Members

        bool IReceiveEvent.ReceiveWeakEvent(EventArgs e)
        {
            RequeryCanExecute();
            return true;        // as in always listening
        }

#endregion

        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        IEnumerable<IDisposable> IDisposableCollection.Disposables => _disposables;

        void IDisposableCollection.Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
        }

#region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies about the property change.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        protected void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> propertySelector)
        {
            Guard.ArgumentNotNull(propertySelector, "propertySelector");
            PropertyChanged.Notify(propertySelector);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Raises the <see cref="E:CommandExecuted" /> event.
        /// </summary>
        /// <param name="args">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        protected void OnCommandExecuted(CommandEventArgs args)
        {
            if (CommandExecuted != null) CommandExecuted(this, args);
        }

        /// <summary>
        /// Parses command parameter to the specified type.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parseAsType"></param>
        /// <returns></returns>
        protected virtual T ParseParameter(object parameter, Type parseAsType)
        {
            if (parameter == null) return default(T);
            if (parseAsType.GetTypeInfo().IsEnum)
            {
                return (T)Enum.Parse(parseAsType, Convert.ToString(parameter), true);
            }
            else if (parseAsType.GetTypeInfo().IsValueType)
            {
                return (T)Convert.ChangeType(parameter, parseAsType, null);
            }
            else
            {
                return (T)parameter;
            }
        }

        /// <summary>
        /// Checks the type of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected void CheckParameterType(object parameter)
        {
            if (parameter == null) return;
            if (typeof(T).GetTypeInfo().IsValueType) return;
            Guard.ArgumentValue(!typeof(T)
                    .GetTypeInfo()
                    .IsAssignableFrom(
                        parameter.GetType()
                            .GetTypeInfo()
                    ), "parameter", ERROR_EXPECTED_TYPE,
                GetType().FullName, typeof(T).FullName);
        }

#endregion

#region Implementation of IExtendedCommand

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { _name = value;
                NotifyPropertyChanged(() => Name); }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Description
        {
            get { return _description??Name; }
            set { _description = value;
                NotifyPropertyChanged(() => Description); }
        }

        /// <summary>
        /// Gets the image URI.
        /// </summary>
        public Uri ImageUri
        {
            get { return _imageUri; }
             set
            {
                _imageUri = value;
                NotifyPropertyChanged(() => ImageUri);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is advanced.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is advanced; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdvanced
        {
            get { return _isAdvanced; }
            set { _isAdvanced = value;
                NotifyPropertyChanged(() => IsAdvanced); }
        }

#endregion
    }
}
