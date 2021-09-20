using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
  /// <summary>
  /// Negates value
  /// </summary>
  public class NegativeConverter : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double d;
      if (value is string && Double.TryParse((string)value, out d))
      {
        return -(d);
      }
      else 
      {
        try
        {
          return -(System.Convert.ToDouble(value));
        }
        catch (Exception)
        {}
      }
      return DependencyProperty.UnsetValue;
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Convert(value,targetType, parameter, culture);
    }
  }
}
