using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.VisualTreeLocations;

namespace Melville.MVVM.Wpf.MouseDragging.ListRearrange
{
    public static class TreeDragDriver
    {
        private static MouseButtonEventArgs? previousDragEventArgs;
        public static void BindToEvent(FrameworkElement elt, bool dragInBackground, Type dropType)
        {
            elt.AddHandler(UIElement.MouseLeftButtonDownEvent, (MouseButtonEventHandler) InitiateDrag, dragInBackground);

             void InitiateDrag(object sender, MouseButtonEventArgs e)
             { 
                 if (e == previousDragEventArgs) return;
                 previousDragEventArgs = e;
                TreeDragDriver.InitiateDrag(CreateDragger(e, (FrameworkElement)sender), dropType);
            }
        }
        private static IMouseDataSource CreateDragger(MouseButtonEventArgs e, FrameworkElement fe) =>
            MouseClickOnDraggedItem(e, fe).DragSource();

        private static IMouseClickReport MouseClickOnDraggedItem(MouseButtonEventArgs e, FrameworkElement fe) =>
            new MouseClickReport(TreeArrange.AdornmentTarget(fe) ??
                throw new InvalidDataException("Cannot find item to drag."), e);

        private static void InitiateDrag(IMouseDataSource dragger, Type dropType)
        {
            if (dragger.Target is FrameworkElement fe && 
                TreeArrange.FindDraggedItem(fe) is {} draggedItem &&
                dropType.IsInstanceOfType(draggedItem))
            {
                dragger
                    .DragTarget(0.5)
                    .Drag(GetDataObject(fe, dropType), DragDropEffects.All,
                        RemoveMovedItemFromList);

                void RemoveMovedItemFromList(DragDropEffects operation)
                {
                    if (operation == DragDropEffects.Move)
                        ListFinder.FindParentListContainingData(fe, draggedItem)?
                            .Remove(draggedItem);
                }
            }
        }

        private static DataObject GetDataObject(FrameworkElement fe, Type dropType)
        {
            var data = TreeArrange.FindDraggedItem(fe);
            if (data == null) return new DataObject();
            var ret = new DataObject(dropType.FullName??"", data);
            TreeArrange.GetSupplementalFormats(fe)?.AddFormats(ret, data);
            return ret;
        }

    }
}