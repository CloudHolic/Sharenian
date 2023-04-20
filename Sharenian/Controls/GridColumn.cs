using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Sharenian.Controls;

public static class GridColumn
{
    #region MinWidth Dependency Property

    public static readonly DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached("MinWidth", typeof(double),
        typeof(GridColumn), new PropertyMetadata(75d, (d, _) =>
        {
            if (d is not GridViewColumn gridColumn)
                return;

            SetMinWidth(gridColumn);
            ((INotifyPropertyChanged)gridColumn).PropertyChanged += (_, ce) =>
            {
                if (ce.PropertyName == nameof(GridViewColumn.ActualWidth))
                    SetMinWidth(gridColumn);
            };
        }));

    private static void SetMinWidth(GridViewColumn column)
    {
        var minWidth = (double)column.GetValue(MinWidthProperty);

        if (column.Width < minWidth)
            column.Width = minWidth;
    }

    public static double GetMinWidth(DependencyObject obj) => (double)obj.GetValue(MinWidthProperty);

    public static void SetMinWidth(DependencyObject obj, double value) => obj.SetValue(MinWidthProperty, value);

    #endregion

    #region MaxWidth Dependency Property

    public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.RegisterAttached("MaxWidth", typeof(double),
        typeof(GridColumn), new PropertyMetadata(75d, (d, _) =>
        {
            if (d is not GridViewColumn gridColumn)
                return;

            SetMaxWidth(gridColumn);
            ((INotifyPropertyChanged)gridColumn).PropertyChanged += (_, ce) =>
            {
                if (ce.PropertyName == nameof(GridViewColumn.ActualWidth))
                    SetMaxWidth(gridColumn);
            };
        }));

    private static void SetMaxWidth(GridViewColumn column)
    {
        var maxWidth = (double)column.GetValue(MaxWidthProperty);

        if (column.Width > maxWidth)
            column.Width = maxWidth;
    }

    public static double GetMaxWidth(DependencyObject obj) => (double)obj.GetValue(MaxWidthProperty);

    public static void SetMaxWidth(DependencyObject obj, double value) => obj.SetValue(MaxWidthProperty, value);

    #endregion
}