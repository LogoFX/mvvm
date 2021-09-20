using System.Windows;

namespace LogoFX.Client.Mvvm.View.Util
{
    /// <summary>
    /// Defines a binding proxy.
    /// </summary>
    public class BindingProxy : Freezable
    {
        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /// <summary>
        /// Defines the Data <see cref="DependencyProperty" />
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object),
                typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}