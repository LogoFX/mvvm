using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Globalization;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Extracts the description attribute.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class EnumDescriptionConverter : IValueConverter
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
            if (value == null) return value;

            Type t = value.GetType();
            DescriptionAttribute displayAttribute = null;
            displayAttribute = (DescriptionAttribute)value.GetType()
                                                          .GetField(value.ToString())
                                                          .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                          .FirstOrDefault();
            if (displayAttribute == null)
            {
                return value;
            }
            else
            {
                return displayAttribute.Description;
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
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}