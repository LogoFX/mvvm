using System.Windows;
using System.Windows.Interactivity;

namespace LogoFX.Client.Mvvm.View.Interactivity.Actions
{
    /// <summary>
    /// Sets focus to the associated object.
    /// </summary>
    public class SetFocusAction : TriggerAction<UIElement>
    {
        /// <inheritdoc />
        protected override void Invoke(object parameter)
        {
            AssociatedObject?.Focus();
        }
    }
}