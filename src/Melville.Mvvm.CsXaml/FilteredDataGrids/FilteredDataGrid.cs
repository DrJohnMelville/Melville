using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using Melville.MVVM.BusinessObjects;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Mvvm.CsXaml.FilteredDataGrids
{
    public static class FilteredDataGrid
    {
        public static DataGrid WithGroupStyle(this DataGrid dg, GroupStyle gs)
        {
            dg.GroupStyle.Add(gs);
            return dg;
        }

        public static DataGrid AsFilteredDataGrid(this DataGrid dg) =>
            dg.WithCanUserSortColumns(false)
                .WithGroupStyle(SetGridGroupinTemplate())
                .WithColumnHeaderStyle(
                    Create.Style<DataGridColumnHeader>()
                        .WithCodeStyle(i => ColumnHeadderCodeStyle(i, dg)));

        private static GroupStyle SetGridGroupinTemplate()
        {
            return new GroupStyle()
                .WithContainerStyle(Create.Style<GroupItem>()
                    .WithTemplate((ControlTemplate)new GridParts()["GroupTemplate"]));
        }

        private static void ColumnHeadderCodeStyle(DataGridColumnHeader i, DataGrid dg)
        {
            var driver = new ColumnDriver(i, dg);
            var context = Create.TargetedBindingContext(driver);
            i.WithTemplate(Create.ControlTemplate<DataGridColumnHeader>(ColumnHeader));

            object ColumnHeader(TemplateBindingContext<DataGridColumnHeader> c)
            {
                var panel = new DockPanel();
                return panel
                    .WithDataContext(driver)    
                    .WithChild(Dock.Right, 
                        DownArrowButton(context.Bind(j=>j.ShowMenu).As<bool?>())
                            .WithVisibility(context.Bind(i=>i.Header.Column.SortMemberPath, 
                             j=>string.IsNullOrWhiteSpace(j)?Visibility.Collapsed:Visibility.Visible)))
                    .WithChild(PopUpWindow(context, panel, ColumnMenu()))
                    .WithChild(Create.ContentPresenter(i.Content,
                        context.Bind(j=>j.Header.ContentTemplate),
                        context.Bind(j=>j.Header.ContentTemplateSelector),
                        context.Bind(j=>j.Header.ContentStringFormat)));
            }
        }

        private static StackPanel ColumnMenu()
        {
            return new StackPanel()
                   .WithChild(Create.Button("Sort A - Z", "SortAscending"))
                   .WithChild(Create.Button("Sort Z-A", "SortDescending"))
                   .WithChild(Create.Button("Group / Ungroup", "GroupBy"))
                ;
        }

        private static Popup PopUpWindow(SourcedBindingContext<ColumnDriver> context, DockPanel panel, 
            FrameworkElement content)
        {
            return new Popup()
                .WithIsOpen(context.Bind(j=>j.ShowMenu))
                .WithStaysOpen(false)
                .WithPlacementTarget(panel)
                .WithPlacement(PlacementMode.Bottom)
                .WithMinWidth(Create.TargetedBindingContext(panel).Bind(j=>j.ActualWidth))
                .WithChild(Create.Border(1,Brushes.Black, Brushes.Beige)
                    .WithChild(content));
        }

        private static ToggleButton DownArrowButton(ValueProxy<bool?> value) =>
            Create.ToggleButton(value,"u")
                .WithTextBlock_FontFamily(new FontFamily("Marlett"))
                .WithMargin((5,0,5,0));

        public class ColumnDriver: NotifyBase
        {
            private readonly DataGrid dataGrid;
            public DataGridColumnHeader Header { get; }

            private bool showMenu;
            public bool ShowMenu
            {
                get => showMenu;
                set => AssignAndNotify(ref showMenu, value);
            }

            public void SortAscending() => Sort(ListSortDirection.Ascending);
            public void SortDescending() => Sort(ListSortDirection.Descending);
            private SortDescription? sortDescription = null;
            
            private void Sort(ListSortDirection sortDirection)
            {
                CommitPendingEdits();
                if (sortDescription != null)
                {
                    dataGrid.Items.SortDescriptions.Remove(sortDescription.Value);
                }

                sortDescription = new SortDescription(Header.Column.SortMemberPath, sortDirection);
                dataGrid.Items.SortDescriptions.Insert(0, sortDescription.Value);
                dataGrid.Items.Refresh();
                ShowMenu = false;
            }

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

            private void CommitPendingEdits()
            {
                // I am not sure why, but two successive commits fixes the bug that one does not.
                //https://stackoverflow.com/a/28526533/50088
                dataGrid.CommitEdit();
                dataGrid.CommitEdit();
            }


            public ColumnDriver(DataGridColumnHeader header, DataGrid dataGrid)
            {
                this.dataGrid = dataGrid;
                Header = header;
            }
        }
    }
}