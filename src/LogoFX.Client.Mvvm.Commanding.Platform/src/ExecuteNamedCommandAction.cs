using System;
using System.Windows.Input;
using System.Globalization;

#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
#endif

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
#endif

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Provides way to call some named command with notion of visual tree routing
    /// Will search for property of type <see cref="ICommand"/> with name />
    /// </summary>
#if NET || NETCORE || NETFRAMEWORK
    [ContentProperty("Parameter")]
#else
    [ContentProperty(Name = "Parameter")]
#endif
    public class ExecuteNamedCommandAction : TriggerAction<FrameworkElement>
    {
        #region Fields

#if NETFX_CORE
        // ReSharper disable once InconsistentNaming
        private const double INTERACTIVITY_ENABLED = 1d;
        // ReSharper disable once InconsistentNaming
        private const double INTERACTIVITY_DISABLED = 0.5d;
#endif

        private bool _manageEnableState = true;

        #endregion

        #region Events

        /// <summary>
        /// Occurs before the message detaches from the associated object.
        /// </summary>
        public event EventHandler Detaching = delegate { };

        #endregion

        #region Dependency Properties

        /// <summary>
        /// The use trigger parameter dependency property.
        /// </summary>
        public static readonly DependencyProperty UseTriggerParameterProperty =
            DependencyProperty.Register(
                "UseTriggerParameter",
                typeof(bool),
                typeof(ExecuteNamedCommandAction),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether to use trigger parameter.
        /// </summary>
        /// <value>
        ///   <c>true</c> if trigger parameter is used; otherwise, <c>false</c>.
        /// </value>
        public bool UseTriggerParameter
        {
            get { return (bool) GetValue(UseTriggerParameterProperty); }
            set { SetValue(UseTriggerParameterProperty, value); }
        }

        /// <summary>
        /// The command dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(ExecuteNamedCommandAction),
                new PropertyMetadata(
                    null,
                    OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExecuteNamedCommandAction executeNamedCommandAction = (ExecuteNamedCommandAction) d;

            if (e.NewValue != null || string.IsNullOrEmpty(executeNamedCommandAction.CommandName))
            {
                executeNamedCommandAction.InternalCommand = e.NewValue as ICommand;
            }
        }

        /// <summary>
        /// Gets or sets the attached command.
        /// </summary>
#if NET || NETCORE || NETFRAMEWORK
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Represents the method name of an action message.
        /// </summary>
        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.Register(
                "CommandName",
                typeof(string),
                typeof(ExecuteNamedCommandAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of the command to be invoked on the presentation model class.
        /// </summary>
        /// <value>The name of the method.</value>
        public string CommandName
        {
            get { return (string) GetValue(CommandNameProperty); }
            set { SetValue(CommandNameProperty, value); }
        }

        /// <summary>
        /// Represents the parameters of an action message.
        /// </summary>
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(
                "Parameter",
                typeof(object),
                typeof(ExecuteNamedCommandAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the parameters to pass as part of the method invocation.
        /// </summary>
        /// <value>The parameters.</value>
        public object Parameter
        {
            get { return GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        /// <summary>
        /// The trigger parameter converter dependency property.
        /// </summary>
        public static readonly DependencyProperty TriggerParameterConverterProperty =
            DependencyProperty.Register(
                "TriggerParameterConverter",
                typeof(IValueConverter),
                typeof(ExecuteNamedCommandAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the trigger parameter converter.
        /// </summary>
        /// <value>
        /// The trigger parameter converter.
        /// </value>
        public IValueConverter TriggerParameterConverter
        {
            get { return (IValueConverter) GetValue(TriggerParameterConverterProperty); }
            set { SetValue(TriggerParameterConverterProperty, value); }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether enable state is managed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enable state is managed; otherwise, <c>false</c>.
        /// </value>
        public bool ManageEnableState
        {
            get { return _manageEnableState; }
            set { _manageEnableState = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Forces an update of the UI's Enabled/Disabled state based on the the preconditions associated with the method.
        /// </summary>
        public void UpdateAvailability()
        {
            if (AssociatedObject == null)
            {
                return;
            }

            if (InternalCommand == null)
            {
                SetPropertyBinding();
            }

            UpdateAvailabilityCore();
        }

        #endregion

        #region Private Members

        private ICommand _internalCommand;

        private ICommand InternalCommand
        {
            get { return _internalCommand; }
            set
            {
                if (_internalCommand != null)
                {
                    _internalCommand.CanExecuteChanged -= CanexecuteChanged;
                }

                _internalCommand = value;
                if (_internalCommand != null)
                {
                    _internalCommand.CanExecuteChanged += CanexecuteChanged;
                }
            }
        }

        private void CanexecuteChanged(object sender, EventArgs e)
        {
            UpdateAvailabilityCore();
        }

        private void ElementLoaded(object sender, RoutedEventArgs e)
        {
            SetPropertyBinding();
            UpdateAvailabilityCore();
        }

        private bool UpdateAvailabilityCore()
        {
            return !ManageEnableState || ApplyAvailabilityEffect();
        }

        /// <summary>
        /// Applies an availability effect, such as IsEnabled, to an element.
        /// </summary>
        /// <remarks>Returns a value indicating whether or not the action is available.</remarks>
        private bool ApplyAvailabilityEffect()
        {
            if (AssociatedObject == null || InternalCommand == null) return false;

            // we get if it is enabled or not
            bool canExecute = InternalCommand.CanExecute(Parameter);

            // we check if it is a control in SL
#if NETFX_CORE
            var control = AssociatedObject as Control;
            if (control != null)
            {
                var target = control;
                target.IsEnabled = canExecute;
            }
            else
            {
                AssociatedObject.IsHitTestVisible = canExecute;
                AssociatedObject.Opacity = canExecute ? INTERACTIVITY_ENABLED : INTERACTIVITY_DISABLED;
            }
#else
            AssociatedObject.IsEnabled = canExecute;
#endif
            return canExecute;
        }

        /// <summary>
        /// Sets the target, method and view on the context. Uses a bubbling strategy by default.
        /// </summary>
        private void SetPropertyBinding()
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                return;
            }

            DependencyObject commandTargetElement = AssociatedObject;                     
            InternalCommand = null;
            var elementAnalyzer = new ElementAnalyzer(CommandName);
            ElementAnalysisResult analysisResult;

            //TODO: Check the case for GetProperty(CommandName, BindingFlags.FlattenHierarchy) if something fails to work
            while (commandTargetElement != null && InternalCommand == null)
            {
                analysisResult = elementAnalyzer.Analyze(commandTargetElement);
                if (analysisResult.CanUseCommand)
                {
                    InternalCommand = analysisResult.Command;
                }
                else
                {
                    commandTargetElement = analysisResult.NextElement;
                }                
            }                   
        }
        
        #endregion

        #region Overrides

        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            ElementLoaded(null, null);
            AssociatedObject.Loaded += ElementLoaded;
            base.OnAttached();
        }

        /// <summary>
        /// Called when the action is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            Detaching(this, EventArgs.Empty);
            AssociatedObject.Loaded -= ElementLoaded;
            if (_internalCommand != null)
            {
                _internalCommand.CanExecuteChanged -= CanexecuteChanged;
            }
            base.OnDetaching();
        }

        /// <summary>
        /// Invokes the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        protected override void Invoke(object arg)
        {
            if (InternalCommand == null)
            {
                SetPropertyBinding();
                if (!UpdateAvailabilityCore())
                {
                    return;
                }
            }

#if NET || NETCORE || NETFRAMEWORK
            var lang = CultureInfo.CurrentCulture;
#else
            var lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
#endif
            // if a trigger parameter converter is specified, then we use that to get the command parameter
            // else we use the given parameter - note_ the parameter can be null
            var parameter = TriggerParameterConverter != null
                ? TriggerParameterConverter.Convert(arg, typeof(object), AssociatedObject, lang)
                : Parameter;

            if (parameter == null && UseTriggerParameter)
            {
                parameter = arg;
            }

            if (InternalCommand != null && InternalCommand.CanExecute(parameter))
            {
                InternalCommand.Execute(parameter);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return $"NamedCommandAction: {CommandName}";
        }

        #endregion
    }
}
