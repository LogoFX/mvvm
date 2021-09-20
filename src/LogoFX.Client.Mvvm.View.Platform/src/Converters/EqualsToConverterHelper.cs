using System;
using System.ComponentModel;
using System.Linq;

namespace LogoFX.Client.Mvvm.View.Converters
{
    internal static class EqualsToConverterHelper
    {
        internal static object Convert(object value, object parameter)
        {
            if (value == null && parameter != null)
            {
                return false;
            }

            if (value != null && parameter == null)
            {
                return false;
            }

            if (value == null && parameter == null)
            {
                return true;
            }

            object compareTo = null;

            if (value is Enum)
            {
                try
                {
                    compareTo = Enum.Parse(value.GetType(), (string)parameter, false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"EqualsToConverterHelper {ex}");
                }
            }
            else
            {
                compareTo = (from TypeConverterAttribute customAttribute in value.GetType().GetCustomAttributes(typeof(TypeConverterAttribute), true)
                                select (TypeConverter)Activator.CreateInstance(Type.GetType(customAttribute.ConverterTypeName))
                                into typeConverter
                                where typeConverter.CanConvertFrom(typeof(string))
                                select typeConverter.ConvertFrom(parameter)).FirstOrDefault() ?? parameter;
            }

            return value.Equals(compareTo);
        }
    }
}