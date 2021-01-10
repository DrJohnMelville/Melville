using  System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Serilog;

namespace Melville.MVVM.Wpf.WpfHacks
{
  /// <summary>
  /// This allows us to bind an observable collection to a listbox to hold the selected items in a bound collection
  /// </summary>
  public static class MultipleSelections
  {
    public static IList GetSelectedItems(DependencyObject obj) => (IList)obj.GetValue(SelectedItemsProperty);
    public static void SetSelectedItems(DependencyObject obj, IList value) => obj.SetValue(SelectedItemsProperty, value);
    /// <summary>
    /// Collection to bind to the selected items in a list box
    /// </summary>
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(MultipleSelections),
          new FrameworkPropertyMetadata(null, SelectedItemsChanged));

    private static void SelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue is IList list && d is ListBox sel)
      {
        new SelectionBinding(sel, list).Bind();
      }
    }

    private class SelectionBinding
    {
      private readonly ListBox selector;
      private readonly IList model;

      public SelectionBinding(ListBox selector, IList model)
      {
        this.selector = selector;
        this.model = model;
      }

      public void Bind()
      {
        SetListBoxSelectionToModel();
        selector.SelectionChanged += ListChanged;
        if (model is INotifyCollectionChanged incc)
        {
          incc.CollectionChanged += ModelChanged;
        }
      }

      /// <summary>
      /// Update the listbox when the model collection changes
      /// </summary>
      /// <param name="sender">Object that sent this message, ignored</param>
      /// <param name="e">EventArgs describing the changes to the source collection</param>
      private void ModelChanged(object? sender, NotifyCollectionChangedEventArgs e)
      {
        try
        {
          switch (e.Action)
          {
            case NotifyCollectionChangedAction.Add:
              AddItems();
              break;
            case NotifyCollectionChangedAction.Remove:
              RemoveItems();
              break;
            case NotifyCollectionChangedAction.Replace:
              AddItems();
              RemoveItems();
              break;
            case NotifyCollectionChangedAction.Move:
              // do nothing, order is not important
              break;
            case NotifyCollectionChangedAction.Reset:
              SetListBoxSelectionToModel();
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        catch (InvalidOperationException exception)
        {
          Log.Error(exception, "Invalid operation binding to multiple select list box?  Is multiple selection enabled?");
        }

        void AddItems()
        {
          foreach (var item in e.NewItems ?? Array.Empty<object>())
          {
            if (!selector.SelectedItems.Contains(item)) // this line prevents recursion
            {
              selector.SelectedItems.Add(item);
            }
          }
        }
        void RemoveItems()
        {
          foreach (var item in e.OldItems ?? Array.Empty<object>())
          {
            selector.SelectedItems.Remove(item);
          }
        }
      }

      /// <summary>
      /// Copy the model selection to the listbox
      /// </summary>
      private void SetListBoxSelectionToModel()
      {
        try
        {
          selector.SelectedItems.Clear();
          foreach (var item in model)
          {
            selector.SelectedItems.Add(item);
          }
        }
        catch (InvalidOperationException e)
        {
          Log.Error(e, "Invalid operation binding to multiple select list box?  Is multiple selection enabled?");
          throw;
        }
      }

      /// <summary>
      /// Update the model when the listbox changes
      /// </summary>
      /// <param name="sender">Object that called this event, ignored</param>
      /// <param name="e">The changes in the listbox selection</param>
      private void ListChanged(object sender, SelectionChangedEventArgs e)
      {
        foreach (var item in e?.RemovedItems ?? new object[0])
        {
          model.Remove(item);
        }
        foreach (var item in e?.AddedItems ?? new object[0])
        {
          if (model.Contains(item)) continue; // this line prevents recursion
          model.Add(item);
        }
      }
    }
  }
}