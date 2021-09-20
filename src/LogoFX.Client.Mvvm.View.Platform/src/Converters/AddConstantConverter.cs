using System;
using System.Globalization;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Adds two specified values.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class AddConstantConverter : IValueConverter
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
            if (parameter == null)
            {
                parameter = 0;
            }

            try
            {
                return System.Convert.ToInt32(value) + System.Convert.ToInt32(parameter);
            }
            catch (Exception)
            {
                return value;
            }
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                parameter = 0;
            }

            try
            {
                return System.Convert.ToInt32(value) - System.Convert.ToInt32(parameter);
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}