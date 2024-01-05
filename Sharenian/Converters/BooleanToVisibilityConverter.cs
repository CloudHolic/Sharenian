using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Sharenian.Types;

namespace Sharenian.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var boolValue = (bool?)value ?? false;
        var inverted = (Inverted)Enum.Parse(typeof(Inverted), (string?)parameter ?? string.Empty) == Inverted.Inverted;

        return boolValue ^ inverted ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}