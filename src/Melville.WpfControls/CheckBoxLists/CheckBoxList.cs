using  System.Collections;
using System.Windows;
using System.Windows.Controls;
using Melville.Hacks;

namespace Melville.WpfControls.CheckBoxLists;

public class CheckBoxList : ListBox
{
    
  public IList BoundSelectedItems
  {
    get => (IList)GetValue(BoundSelectedItemsProperty);
    set => SetValue(BoundSelectedItemsProperty, value);
  }
  public static readonly DependencyProperty BoundSelectedItemsProperty =
    DependencyProperty.Register("BoundSelectedItems", typeof(IList), typeof(CheckBoxList), 
      new FrameworkPropertyMetadata(null, ReadSelectionFromViewModel));


  static CheckBoxList() => 
    DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBoxList), 
      new FrameworkPropertyMetadata(typeof(CheckBoxList)));

  public CheckBoxList()
  {
    SelectionMode = SelectionMode.Multiple;
    SelectionChanged += WriteSelectionToViewModel;
  }

  private readonly PreventRecursion mutex = new PreventRecursion();

  private static void ReadSelectionFromViewModel(DependencyObject d, DependencyPropertyChangedEventArgs e) => 
    ((CheckBoxList) d).ReadSelectionFromViewModel((IList)e.NewValue);

  private void ReadSelectionFromViewModel(IList newValue) => 
    mutex.DoNonRecursive(()=>SetSelectedItems(newValue));

  private void WriteSelectionToViewModel(object s, SelectionChangedEventArgs e) => 
    mutex.DoNonRecursive(() => BoundSelectedItems = SelectedItems);
}