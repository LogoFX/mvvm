using System;
using System.Windows.Data;
using System.Globalization;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Returns the inverted value of the provided <see cref="bool"/> parameter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class BoolRevertConverter : IValueConverter
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
            if(value is bool)
            {
                bool b = (bool) value;
                return !b;
            }
            return value;
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
        //    throw new NotImplementedException();
            if (value is bool)
            {
                bool b = (bool)value;
                return !b;
            }
            return value;
        }
    }
}
