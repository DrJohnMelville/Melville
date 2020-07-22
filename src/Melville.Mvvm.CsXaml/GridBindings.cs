using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using Melville.MVVM.CSharpHacks;
using Melville.MVVM.Functional;

namespace Melville.Mvvm.CsXaml
{
    public static class GridBindings
    {
        public static RowDefinitionCollection SetRows<TDataContext>(
            this XamlBuilder<Grid, TDataContext> target,
            string rowDeclarations) 
        {
            target.Target.RowDefinitions.AddRange(
               GridDimensionParser.ParseGridDimensions(rowDeclarations)
                    .Select(i => new RowDefinition() {Height = i}));
            return target.Target.RowDefinitions;
        }
        public static ColumnDefinitionCollection SetColumns<TDataContext>(
            this XamlBuilder<Grid, TDataContext> target,
            string columnDefinitions)
        {
            target.Target.ColumnDefinitions.AddRange(
                GridDimensionParser.ParseGridDimensions(columnDefinitions)
                    .Select(i => new ColumnDefinition() {Width = i}));
            return target.Target.ColumnDefinitions;
        }
        public static GridPlacement RowMajorChildren<TDataContext>(
            this XamlBuilder<Grid, TDataContext> target) =>
            new GridPlacement(target.Target.ColumnDefinitions.Count,
                Grid.RowProperty, Grid.ColumnProperty, 
                Grid.ColumnSpanProperty, Grid.RowSpanProperty);
        public static GridPlacement ColumnMajorChildren<TDataContext>(
            this XamlBuilder<Grid, TDataContext> target) =>
            new GridPlacement(target.Target.RowDefinitions.Count,
                Grid.ColumnProperty, Grid.RowProperty,  
                Grid.RowSpanProperty, Grid.ColumnSpanProperty);

    }

    public class GridPlacement
    {
        private readonly DependencyProperty primaryPositionProp;
        private readonly DependencyProperty secondaryPositionProp;
        private readonly DependencyProperty primarySpanProp;
        private readonly DependencyProperty secondarySpanProp;
        private GridPositioner positioner;

        public GridPlacement(int width, 
            DependencyProperty primaryPositionProp, DependencyProperty secondaryPositionProp,
            DependencyProperty primarySpanProp, DependencyProperty secondarySpanProp)
        {
            this.primaryPositionProp = primaryPositionProp;
            this.secondaryPositionProp = secondaryPositionProp;
            this.primarySpanProp = primarySpanProp;
            this.secondarySpanProp = secondarySpanProp;
            positioner = new GridPositioner(width);
        }

        public GridPlacement AddToGrid(DependencyObject elt, int primaryAxisSpan = 1, int secondaryAxisSpan = 1)
        {
            SetDimension(elt, primaryAxisSpan, primarySpanProp);
            SetDimension(elt, secondaryAxisSpan, secondarySpanProp);
            var (primaryValue, secondaryValue) = positioner.NextPlaceAsRowCol(secondaryAxisSpan, primaryAxisSpan);
            elt.SetValue(primaryPositionProp, primaryValue);
            elt.SetValue(secondaryPositionProp, secondaryValue);
            return this;
        }

        private void SetDimension(DependencyObject elt, int rowSpan, DependencyProperty targetProp)
        {
            if (rowSpan > 1) elt.SetValue(targetProp, rowSpan);
        }
    }
    
    

    // gridPositioner variable names assume left to right row major order,
    // we get column major order by just feeding in consistently wrong argumments;
    public class GridPositioner
    {
        private readonly int width;
        private readonly int[] filledSpaces;
        private int currentColumn;
        private int currentRow;

        public GridPositioner(int width)
        {
            this.width = width;
            filledSpaces = new int[width];
        }

        public (int, int) NextPlaceAsRowCol(int rowSpan, int colSpan)
        {
            if (colSpan > width)
            {
                throw new InvalidEnumArgumentException("Column span cannot be greater than width.");
            }
            
            WrapRowIfNeeded();
            while (!IsValidPosition(colSpan)) AdvancePosition();
            var nextPosition = (currentRow, currentColumn);
            FillSpaces(rowSpan, colSpan);
            return nextPosition;
        }

        private void AdvancePosition()
        {
            currentColumn++;
            WrapRowIfNeeded();
        }

        private bool IsValidPosition(in int colSpan)
        {
            if (TooLittleRoomInThisRow(colSpan)) return false;
            for (int i = 0; i < colSpan; i++)
            {
                if (filledSpaces[i + currentColumn] > 0) return false;
            }

            return true;
        }

        private bool TooLittleRoomInThisRow(int colSpan) => currentColumn + colSpan > width;

        private void FillSpaces(int rowSpan, int colSpan)
        {
            for (int i = 0; i < colSpan && currentColumn < filledSpaces.Length; i++)
            {
                filledSpaces[currentColumn] = rowSpan;
                currentColumn++;
            }
        }

        private void WrapRowIfNeeded()
        {
            if (TooLittleRoomInThisRow(1))
            {
                AdvanceCurrentLocationToNewLine();
                RemoveOneFromFilledRow();
            }

        }
        private void AdvanceCurrentLocationToNewLine()
        {
            currentRow++;
            currentColumn = 0;
        }

        private void RemoveOneFromFilledRow()
        {
            for (int i = 0; i < width; i++)
            {
                filledSpaces[i] = Math.Max(0, filledSpaces[i] - 1);
            }
        }
    }
}