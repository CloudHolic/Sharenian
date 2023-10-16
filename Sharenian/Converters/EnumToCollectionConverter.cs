using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Sharenian.Utils;

namespace Sharenian.Converters;

[ValueConversion(typeof(Enum), typeof(IEnumerable<EnumExtensions.ValueDescription>))]
public class EnumToCollectionConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter != null)
            return EnumExtensions.GetValuesAndDescriptions(value?.GetType(), null);

        var values = (value as IEnumerable)?.Cast<Enum>().ToList();
        return EnumExtensions.GetValuesAndDescriptions(values?.Count > 0 ? values[0].GetType() : null, x => values != null && values.Contains(x));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not EnumExtensions.ValueDescription description)
            return null;

        return description.Value;
    }
}