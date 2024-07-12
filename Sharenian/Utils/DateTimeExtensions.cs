using System;

namespace Sharenian.Utils;

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
    {
        var diff = (7 + dateTime.DayOfWeek - startOfWeek) % 7;
        return dateTime.AddDays(-1 * diff);
    }
}
