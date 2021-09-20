using System;
using System.Globalization;
using System.Windows;
using FluentAssertions;
using LogoFX.Client.Mvvm.View.Converters;
using Xunit;

namespace LogoFX.Client.Mvvm.View.Platform.Tests
{
    public class EqualsToVisibilityConverterTests
    {
        [Fact]
        public void Convert_StringValuesAreTheSame_ReturnsVisible()
        {
            var converter = new EqualsToVisibilityConverter();
            var value = "Val";
            var parameter = "Val";
            var result = converter.Convert(value, default(Type), parameter, CultureInfo.InvariantCulture);

            result.Should().Be(Visibility.Visible);
        }

        [Fact]
        public void Convert_StringValuesAreDifferent_ReturnsCollapsed()
        {
            var converter = new EqualsToVisibilityConverter();
            var value = "Val";
            var parameter = "Param";
            var result = converter.Convert(value, default(Type), parameter, CultureInfo.InvariantCulture);

            result.Should().Be(Visibility.Collapsed);
        }
    }
}
