using System;
using System.Globalization;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Miltiplies the value by the provided parameter.
    /// </summary>
    public class MulConstantConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the default parameter value.
        /// </summary>
        public decimal DefaultParam { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="MulConstantConverter"/>
        /// </summary>
        public MulConstantConverter()
        {
            DefaultParam = 1;
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                parameter = DefaultParam;
            var dec = value.ToDecimal();
            if (dec < 0)
            {
                return double.NaN;
            }
            return dec * parameter.ToDecimal();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                parameter = DefaultParam;
            return value.ToDecimal() / parameter.ToDecimal();
        }        
    }
}
