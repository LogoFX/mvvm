using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
  /// <summary>
  /// Converts <see cref="DateTime"/> to <see cref="double"/> according to supplied format
  /// </summary>
  public class TimeToDoubleConverter : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is System.DateTime)
      {
        switch ((string)parameter)
        {
          case "Hours":
            return new TimeSpan(((DateTime)value).Ticks).TotalHours;
          case "Days":
            return new TimeSpan(((DateTime)value).Ticks).TotalDays;
          case "Minutes":
            return new TimeSpan(((DateTime)value).Ticks).TotalMinutes;
          case "Seconds":
            return new TimeSpan(((DateTime)value).Ticks).TotalSeconds;
          case "Milliseconds":
            return new TimeSpan(((DateTime)value).Ticks).TotalMilliseconds;
          case "Ticks":
          default:
            return (double)((DateTime)value).Ticks;
        }
      }
      return DependencyProperty.UnsetValue;
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch ((string)parameter)
      {
        case "Hours":
          return new DateTime(TimeSpan.FromHours((double)value).Ticks);
        case "Days":
          return new DateTime(TimeSpan.FromDays((double)value).Ticks);
        case "Minutes":
          return new DateTime(TimeSpan.FromMinutes((double)value).Ticks);
        case "Seconds":
          return new DateTime(TimeSpan.FromSeconds((double)value).Ticks);
        case "Milliseconds":
          return new DateTime(TimeSpan.FromMilliseconds((double)value).Ticks);
        case "Ticks":
        default:
          return new DateTime((long)(double)value);
      }
    }
  }
}
