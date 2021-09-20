using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace LogoFX.Client.Mvvm.View.Interactivity.Behaviors
{
    /// <summary>
    /// Selects text on focus event.
    /// </summary>
    public class SelectTextOnFocusBehavior
        : Behavior<FrameworkElement>
    {
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += AssociatedObjectGotFocus;
            AssociatedObject.GotKeyboardFocus += AssociatedObjectGotFocus;
        }

        void AssociatedObjectGotFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject is TextBox)
                ((TextBox)AssociatedObject).SelectAll();
            else if (AssociatedObject is PasswordBox)
                ((PasswordBox)AssociatedObject).SelectAll();
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= AssociatedObjectGotFocus;
        }
    }
}
