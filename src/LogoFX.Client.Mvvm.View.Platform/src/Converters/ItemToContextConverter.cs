using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
  /// <summary>
  /// Conditionally extracts <see cref="FrameworkElement.DataContext"/> from value.
  /// </summary>
  public class ItemToContextConverter : IValueConverter
  {
      /// <inheritdoc />
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value is FrameworkElement)
            value = ((FrameworkElement) value).DataContext;

        return value;
      }

      /// <inheritdoc />
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
  }
}
