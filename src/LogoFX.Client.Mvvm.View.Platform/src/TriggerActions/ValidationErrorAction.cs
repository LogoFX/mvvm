using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace LogoFX.Client.Mvvm.View.TriggerActions
{
    /// <summary>
    /// Defines an action which should be called when validation yields an error.
    /// </summary>
    public class ValidationErrorAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the command
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Defines the command <see cref="DependencyProperty" />
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ValidationErrorAction),
                new PropertyMetadata(null));

        /// <inheritdoc />
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject == null) return;
            var command = GetCommand(AssociatedObject);
            command?.Execute(parameter);
        }
    }
}