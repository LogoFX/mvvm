using System;
using System.Globalization;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Subtracts the provided parameter from the value.
    /// </summary>
    public class SubConstantConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                parameter = 0;

            var dec = value.ToDecimal();

            return dec - parameter.ToDecimal();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                parameter = 0;

            return value.ToDecimal() + parameter.ToDecimal();
        }
    }
}
