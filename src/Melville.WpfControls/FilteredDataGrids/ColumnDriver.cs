using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.Bindings;

namespace Melville.WpfControls.FilteredDataGrids
{
    public class ColumnDriver: NotifyBase
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
                dataGrid.Items.GroupDescriptions.Remove(groupDescription);
                groupDescription = null;
            }
            dataGrid.Items.Refresh();
            ShowMenu = false;
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
        private string filterString = "";
        public string FilterString
        {
            get => filterString;
            set {
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
            dataGrid.Items.Filter =
                ShouldFilter() ? (Predicate<object>) (AlwaysTruePredicate) 
                    : i => filterStringValueFunc(i).Contains(FilterString, StringComparison.CurrentCultureIgnoreCase);
        }

        private bool AlwaysTruePredicate(object i) => true;

        private bool ShouldFilter() => string.IsNullOrWhiteSpace(FilterString);

        #endregion
    }
}