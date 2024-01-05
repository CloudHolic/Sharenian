using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Sharenian.Converters;

public class BooleanToComboBoxTextConverter : MarkupExtension, IValueConverter
{
    private const string ThisWeekText = "이번주";
    private const string LastWeekText = "저번주";

    public override object ProvideValue(IServiceProvider serviceProvider) => this;


    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Equals(value, true) ? ThisWeekText : LastWeekText;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Equals(value, ThisWeekText);
}
