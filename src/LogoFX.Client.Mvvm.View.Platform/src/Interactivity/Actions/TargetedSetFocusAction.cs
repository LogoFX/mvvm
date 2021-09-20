using System.Windows;
using System.Windows.Interactivity;

namespace LogoFX.Client.Mvvm.View.Interactivity.Actions
{
    /// <summary>
    /// Sets focus to the associated object.
    /// </summary>
    [TypeConstraintAttribute(typeof(DependencyObject))]   
    public class TargetedSetFocusAction : TargetedTriggerAction<UIElement>
    {
        /// <inheritdoc />
        protected override void Invoke(object parameter)
        {
            Target?.Focus();
        }
    }
}
