using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
  /// <summary>
  /// Converts boolean to constant value
  /// </summary>
  public class BoolToConstConverter : IValueConverter
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
          object parameterTrue = parameter;
          object parameterFalse = DependencyProperty.UnsetValue;
          if (parameter is String)
          {
              string[] elements = ((string) parameter).Split('|');
              parameterTrue = elements[0];
              if (elements.Length > 1)
              {
                  parameterFalse = elements[1];
              }
          }
          if (value is bool)
          {
              return (bool) value ? parameterTrue : parameterFalse;
          }
          if (value is string)
          {
              bool res;
              if (Boolean.TryParse((string) value, out res) && res)
              {
                  return parameterTrue;
              }
          }
          return parameterFalse;
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
          return value;
      }
  }
}
