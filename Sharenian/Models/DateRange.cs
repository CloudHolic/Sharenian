using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Sharenian.Utils;

namespace Sharenian.Models;

public class DateRange(DateTime pivot) : ObservableObject
{
    public DateTime PivotDate { get; set; } = pivot;

    public string Display { get; set; } = pivot.DayOfWeek == DayOfWeek.Thursday
        ? $"{pivot.AddDays(-7):MM/dd} ~ {pivot.AddDays(-1):MM/dd}"
        : $"{pivot.StartOfWeek(DayOfWeek.Thursday):MM/dd} ~ {DateTime.Today:MM/dd}";

    public DateRange() : this(DateTime.Today)
    {

    }
}
