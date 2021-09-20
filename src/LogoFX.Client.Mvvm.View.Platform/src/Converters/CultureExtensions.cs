using System;
using System.Globalization;

namespace LogoFX.Client.Mvvm.View.Converters
{
    static class CultureExtensions
    {
        internal static decimal ToDecimal(this object value)
        {
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }
    }
}
