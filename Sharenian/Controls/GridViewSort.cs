using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Sharenian.Controls;

public class GridViewSort
{
    #region PropertyName Attached Dependency Property

    public static readonly DependencyProperty PropertyNameProperty =
        DependencyProperty.RegisterAttached("PropertyName", typeof(string), typeof(GridViewSort),
            new UIPropertyMetadata(null));

    public static string GetPropertyName(DependencyObject d) => (string)d.GetValue(PropertyNameProperty);

    public static void SetPropertyName(DependencyObject d, string value) => d.SetValue(PropertyNameProperty, value);

    #endregion

    #region AutoSort Attached Dependency Property

    public static readonly DependencyProperty AutoSortProperty = DependencyProperty.RegisterAttached("AutoSort",
        typeof(bool), typeof(GridViewSort), new UIPropertyMetadata(false, AutoSortChangedCallback));

    public static bool GetAutoSort(DependencyObject d) => (bool)d.GetValue(AutoSortProperty);

    public static void SetAutoSort(DependencyObject d, bool value) => d.SetValue(AutoSortProperty, value);

    private static void AutoSortChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ListView listView)
            return;

        if (GetCommand(listView) != null)
            return;

        var (oldValue, newValue) = ((bool)e.OldValue, (bool)e.NewValue);

        switch (oldValue)
        {
            case true when !newValue:
                listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                break;
            case false when newValue:
                listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                break;
        }
    }

    #endregion

    #region Command Attached Dependency Property

    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command",
        typeof(ICommand), typeof(GridViewSort), new UIPropertyMetadata(null, CommandChangedCallback));

    public static ICommand? GetCommand(DependencyObject d) => (ICommand?)d.GetValue(CommandProperty);

    public static void SetCommand(DependencyObject d, ICommand? value) => d.SetValue(CommandProperty, value);

    private static void CommandChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ItemsControl listView)
            return;

        if (GetAutoSort(listView))
            return;

        if (e is { OldValue: not null, NewValue: null })
            listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
        if (e is { OldValue: null, NewValue: not null })
            listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
    }

    #endregion

    #region Private Methods

    private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not GridViewColumnHeader headerClicked)
            return;

        var propertyName = GetPropertyName(headerClicked.Column);
        if (string.IsNullOrEmpty(propertyName))
            return;

        var listView = GetAncestor<ListView>(headerClicked);
        if (listView == null)
            return;

        var command = GetCommand(listView);

        if (command != null && command.CanExecute(propertyName))
            command.Execute(propertyName);
        else if (GetAutoSort(listView))
            ApplySort(listView.Items, propertyName);

        #region Local Functions

        static T? GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(reference);
            while (parent is not null and not T)
                parent = VisualTreeHelper.GetParent(parent);

            return (T?)parent;
        }

        static void ApplySort(ICollectionView view, string propertyName)
        {
            var direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                var currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                    direction = currentSort.Direction == ListSortDirection.Ascending
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending;

                view.SortDescriptions.Clear();
            }

            if (!string.IsNullOrEmpty(propertyName))
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }

        #endregion
    }

    #endregion
}
