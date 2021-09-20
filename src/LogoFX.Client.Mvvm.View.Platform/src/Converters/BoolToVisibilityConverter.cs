using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Converts the provided boolean value to the correspondent <see cref="Visibility"/> one.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = false;
            if (value is bool)
                bValue = (bool) value;
            else if (value is string)
                Boolean.TryParse((string) value, out bValue);
            else
                return Visibility.Visible;

            bool result;

            if (!Boolean.TryParse((string) parameter, out result))
                result = true;

            if (result)
                return bValue ? Visibility.Visible : Visibility.Collapsed;
            else
                return bValue ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.Exception">The method or operation is not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}