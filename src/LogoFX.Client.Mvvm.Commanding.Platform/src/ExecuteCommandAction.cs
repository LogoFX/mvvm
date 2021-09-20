using System;
using System.Windows.Input;
using System.Globalization;

#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Interactivity;
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
    /// Executes the attached command
    /// </summary>
#if NET || NETCORE || NETFRAMEWORK
    [ContentProperty("Parameter")]
#else
    [ContentProperty(Name = "Parameter")]
#endif   
    public class ExecuteCommandAction : TriggerAction<FrameworkElement>
    {
        #region Fields

#if NETFX_CORE
        // ReSharper disable once InconsistentNaming
        private const double INTERACTIVITY_ENABLED = 1d;
        // ReSharper disable once InconsistentNaming
        private const double INTERACTIVITY_DISABLED = 0.5d;
#endif

        #endregion

        #region Dependency Properties

        /// <summary>
        /// The command property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(ExecuteCommandAction),
                new PropertyMetadata(
                    null,
                    OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ExecuteCommandAction) d).SetupEnableState(e.NewValue as ICommand, e.OldValue as ICommand);
        }

        /// <summary>
        /// Gets or sets the command.
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
        /// The parameter property
        /// </summary>
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(
                "Parameter",
                typeof(object),
                typeof(ExecuteCommandAction),
                new PropertyMetadata(
                    null,
                    OnParameterChanged));

        private static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ExecuteCommandAction) d).UpdateEnabledState();
        }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
#if NET || NETCORE || NETFRAMEWORK
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public object Parameter
        {
            get { return GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        /// <summary>
        /// The trigger parameter converter property
        /// </summary>
        public static readonly DependencyProperty TriggerParameterConverterProperty =
            DependencyProperty.Register(
                "TriggerParameterConverter",
                typeof(IValueConverter),
                typeof(ExecuteCommandAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the trigger parameter converter.
        /// </summary>
#if NET || NETCORE || NETFRAMEWORK
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public IValueConverter TriggerParameterConverter
        {
            get { return (IValueConverter) GetValue(TriggerParameterConverterProperty); }
            set { SetValue(TriggerParameterConverterProperty, value); }
        }

        /// <summary>
        /// The use trigger parameter property
        /// </summary>
        public static readonly DependencyProperty UseTriggerParameterProperty =
            DependencyProperty.Register(
                "UseTriggerParameter",
                typeof(bool),
                typeof(ExecuteCommandAction),
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

        #endregion

        #region Public Properties

        private bool _manageEnableState = true;

        /// <summary>
        /// Gets or sets a value indicating whether the enable state should be managed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enable state should be managed; otherwise, <c>false</c>.
        /// </value>
        public bool ManageEnableState
        {
            get { return _manageEnableState; }
            set { _manageEnableState = value; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateEnabledState();
        }

        /// <summary>
        /// Called when the action is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            DisposeEnableState();
        }

        /// <summary>
        /// Invokes the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        protected override void Invoke(object arg)
        {
            if (AssociatedObject == null)
            {
                return;
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

            if (Command != null && Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }

        #endregion

        #region Private Members

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateEnabledState();
        }

        private void SetupEnableState(ICommand newCommand, ICommand oldCommand)
        {
            if (!ManageEnableState)
            {
                return;
            }

            // we detach or attach
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= Command_CanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += Command_CanExecuteChanged;
            }

            // and update
            UpdateEnabledState();
        }

        private void UpdateEnabledState()
        {
            if (!ManageEnableState || AssociatedObject == null || Command == null) return;

            // we get if it is enabled or not
            var canExecute = Command.CanExecute(Parameter);

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
        }

        private void DisposeEnableState()
        {
            if (!ManageEnableState || AssociatedObject == null || Command == null)
            {
                return;
            }

            if (AssociatedObject != null)
            {
                Command.CanExecuteChanged -= Command_CanExecuteChanged;
            }
        }

        #endregion
    }
}