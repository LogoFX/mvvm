using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace LogoFX.Client.Mvvm.View.Interactivity.Behaviors
{
    /// <summary>
    /// Provides functionality missed in unsupported platforms :
    /// {Binding UpdateSourceTrigger=PropertyChange}    
    /// </summary>
    public class UpdateSourceOnChangeBehavior : Behavior<DependencyObject>
    {
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();

            var txt = AssociatedObject as TextBox;
            if (txt != null)
            {
                txt.TextChanged += OnTextChanged;
                return;
            }
#if SILVERLIGHT
            var pass = AssociatedObject as PasswordBox;
            if (pass != null)
            {
                pass.PasswordChanged += OnPasswordChanged;
                return;
            }
#endif
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            var txt = AssociatedObject as TextBox;
            if (txt != null)
            {
                txt.TextChanged -= OnTextChanged;
                return;
            }
#if SILVERLIGHT
            var pass = AssociatedObject as PasswordBox;
            if (pass != null)
            {
                pass.PasswordChanged -= OnPasswordChanged;
                return;
            }
#endif
            base.OnDetaching();
        }
#if SILVERLIGHT
        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var txt = sender as PasswordBox;
            if (txt == null)
                return;
            var be = txt.GetBindingExpression(PasswordBox.PasswordProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }
#endif
        static void OnTextChanged(object sender,
          TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null)
                return;
            var be = txt.GetBindingExpression(TextBox.TextProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }
    }
}
