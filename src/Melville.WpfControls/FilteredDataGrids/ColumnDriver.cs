using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.Bindings;

namespace Melville.WpfControls.FilteredDataGrids;

public class ColumnDriver : NotifyBase
{
    private readonly DataGrid dataGrid;
    public DataGridColumnHeader Header { get; }

    private Func<object, string>? filterStringValueFunc =>
        Header.Column.ConstructFilterStringMethod();

    public static readonly IMultiValueConverter Creator = LambdaConverter.Create(
        (DataGridColumnHeader header, DataGrid dataGrid1) => new ColumnDriver(header, dataGrid1));

    public ColumnDriver(DataGridColumnHeader header, DataGrid dataGrid)
    {
        this.dataGrid = dataGrid;
        Header = header;
    }

    public static IMultiValueConverter OrConv = LambdaConverter.Create(
        (string path, object content) => content == null || string.IsNullOrWhiteSpace(path)
            ? Visibility.Collapsed
            : Visibility.Visible);


    private bool showMenu;

    public bool ShowMenu
    {
        get => showMenu;
        set => AssignAndNotify(ref showMenu, value);
    }

    #region Sorting

    public void SortAscending() => Sort(ListSortDirection.Ascending);
    public void SortDescending() => Sort(ListSortDirection.Descending);

    private void Sort(ListSortDirection sortDirection)
    {
        CommitPendingEdits();
        UpdateSortDescriptions(sortDirection);
        dataGrid.Items.Refresh();
        ShowMenu = false;
    }

    private void UpdateSortDescriptions(ListSortDirection sortDirection)
    {
        dataGrid.Items.SortDescriptions.Clear();
        dataGrid.Items.SortDescriptions.Add(
            new SortDescription(Header.Column.SortMemberPath, sortDirection));
    }

    #endregion

    #region Grouping

    private PropertyGroupDescription? groupDescription;

    public void GroupBy()
    {
        CommitPendingEdits();
        if (groupDescription == null)
        {
            groupDescription = new PropertyGroupDescription(Header.Column.SortMemberPath);
            dataGrid.Items.GroupDescriptions.Add(groupDescription);
        }
        else
        {
            ClearGroupDescription();
        }

        dataGrid.Items.Refresh();
        ShowMenu = false;
    }

    private void ClearGroupDescription()
    {
        dataGrid.Items.GroupDescriptions.Remove(groupDescription);
        groupDescription = null;
    }

    public void GroupByRegexCapture()
    {
        try
        {
            if (filterStringValueFunc == null) return;
            var regex = RegexFromFilter();
            var converter = LambdaConverter.Create<object, string>
            (i =>
            {
                var m = regex.Match(filterStringValueFunc(i));
                return m.Success ? m.Groups.OfType<Group>().Last().Value: "<No Match>";
            });
            if (groupDescription != null) ClearGroupDescription();
            groupDescription = new PropertyGroupDescription("", converter);
            dataGrid.Items.GroupDescriptions.Add(groupDescription);
            dataGrid.Items.Refresh();
            ShowMenu = false;
        }
        catch (Exception)
        {
            // do nothing
        }
    }

    #endregion

    private void CommitPendingEdits()
    {
        // I am not sure why, but two successive commits fixes the bug that one does not.
        //https://stackoverflow.com/a/28526533/50088
        dataGrid.CommitEdit();
        dataGrid.CommitEdit();
    }

    #region Filtering

    private bool filterByRegex;

    public bool FilterByRegex
    {
        get => filterByRegex;
        set
        {
            if (AssignAndNotify(ref filterByRegex, value))
            {
                SetupFilter();
            }
        }
    }


    private string filterString = "";

    public string FilterString
    {
        get => filterString;
        set
        {
            if (AssignAndNotify(ref filterString, value))
            {
                SetupFilter();
            }
        }
    }

    private void SetupFilter()
    {
        if (filterStringValueFunc == null) return;
        CommitPendingEdits();
        dataGrid.Items.Filter = CreateFilter(filterStringValueFunc);
    }

    private Predicate<object> CreateFilter(Func<object, string> columnValueFunction)
    {
        return (FilterString, FilterByRegex) switch
        {
            (var s, _) when string.IsNullOrWhiteSpace(s) => AlwaysTruePredicate,
            (var s, false) => SimpleContainsPredicate(columnValueFunction),
            (var s, true) => RegexPredicate(columnValueFunction)
        };
    }

    private Predicate<object> RegexPredicate(Func<object, string> columnValueFunction)
    {
        try
        {
            var regex = RegexFromFilter();
            return i => regex.IsMatch(columnValueFunction(i));
        }
        catch (Exception )
        {
            return AlwaysFalsePredicate;
        }
    }

    private Regex RegexFromFilter() => new Regex(FilterString, RegexOptions.IgnoreCase);

    private Predicate<object> SimpleContainsPredicate(Func<object, string> columnValueFunction) => 
        i => columnValueFunction(i).Contains(FilterString, StringComparison.CurrentCultureIgnoreCase);
    private bool AlwaysTruePredicate(object i) => true;
    private bool AlwaysFalsePredicate(object i) => false;
    #endregion
}