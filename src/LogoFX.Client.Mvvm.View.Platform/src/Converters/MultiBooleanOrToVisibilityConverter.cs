using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Converts collection of <see cref="bool"/> values to the respective visibility value using OR logical function.
    /// </summary>
    public class MultiBooleanOrToVisibilityConverter : IMultiValueConverter
    {
        /// <summary>
        /// Gets or sets the visibility value when the underlying logical function yields <code>true</code> />
        /// </summary>
        public Visibility TrueVisibility { get; set; }

        /// <summary>
        /// Gets or sets the visibility value when the underlying logical function yields <code>false</code> /> 
        /// </summary>
        public Visibility FalseVisibility { get; set; }

        /// <inheritdoc />
        public MultiBooleanOrToVisibilityConverter()
        {
            TrueVisibility = Visibility.Visible;
            FalseVisibility = Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object Convert(object[] values,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            var visible = values.OfType<bool>().Aggregate(false, (current, value) => current || value);

            return visible ? TrueVisibility : FalseVisibility;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value,
            Type[] targetTypes,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}