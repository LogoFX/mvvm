using System;
using System.Globalization;
using FluentAssertions;
using LogoFX.Client.Mvvm.View.Converters;
using Xunit;

namespace LogoFX.Client.Mvvm.View.Platform.Tests
{
    public class EqualsToBooleanConverterTests
    {
        [Fact]
        public void Convert_StringValuesAreTheSame_ReturnsTrue()
        {
            var converter = new EqualsToBooleanConverter();
            var value = "Val";
            var parameter = "Val";
            var result = converter.Convert(value, default(Type), parameter, CultureInfo.InvariantCulture);

            result.Should().Be(true);
        }

        [Fact]
        public void Convert_StringValuesAreDifferent_ReturnsFalse()
        {
            var converter = new EqualsToBooleanConverter();
            var value = "Val";
            var parameter = "Param";
            var result = converter.Convert(value, default(Type), parameter, CultureInfo.InvariantCulture);

            result.Should().Be(false);
        }
    }
}
