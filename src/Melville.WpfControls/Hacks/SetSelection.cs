using  System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Melville.MVVM.Functional;

namespace Melville.WpfControls.Hacks
{
  public static class SetSelection
  {
    #region SelectOnBool


    public static bool GetOnBool(DependencyObject obj)
    {
      return (bool)obj.GetValue(OnBoolProperty);
    }

    public static void SetOnBool(DependencyObject obj, bool value)
    {
      obj.SetValue(OnBoolProperty, value);
    }

    // Using a DependencyProperty as the backing store for OnBool.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnBoolProperty =
      DependencyProperty.RegisterAttached("OnBool", typeof(bool), typeof(SetSelection),
        new FrameworkPropertyMetadata(false, OnBoolChanged));

    private static void OnBoolChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if ((bool)e.NewValue && (!(bool)e.OldValue))
      {
        FocusControl(sender);
      }
    }

    #endregion

    #region SelectOnCount


    public static int GetOnCount(DependencyObject obj)
    {
      return (int)obj.GetValue(OnCountProperty);
    }

    public static void SetOnCount(DependencyObject obj, int value)
    {
      obj.SetValue(OnCountProperty, value);
    }

    // Using a DependencyProperty as the backing store for OnCount.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnCountProperty =
      DependencyProperty.RegisterAttached("OnCount", typeof(int), typeof(SetSelection),
        new FrameworkPropertyMetadata(0, CountChanged));

    private static void CountChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var value = (int)e.NewValue;
      if (value == 0) return;
      FocusControl(sender);
    }
    #endregion

    #region Changed

    public static object GetChanged(DependencyObject ob) => (object)ob.GetValue(ChangedProperty);
    public static void SetChanged(DependencyObject ob, object value) => ob.SetValue(ChangedProperty, value);
    public static readonly DependencyProperty ChangedProperty =
      DependencyProperty.RegisterAttached("Changed",
        typeof(object), typeof(SetSelection), new FrameworkPropertyMetadata(null, ChangedChanged));
    private static void ChangedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      FocusControl(d);
    }

    #endregion

    #region SelectOnDataContext 


    public static object GetOnDataContext(DependencyObject obj)
    {
      return obj.GetValue(OnDataContextProperty);
    }

    public static void SetOnDataContext(DependencyObject obj, object value)
    {
      obj.SetValue(OnDataContextProperty, value);
    }

    // Using a DependencyProperty as the backing store for OnDataContext.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnDataContextProperty =
      DependencyProperty.RegisterAttached("OnDataContext", typeof(object), typeof(SetSelection),
        new UIPropertyMetadata(null, OnDataContextChanged));

    private static void OnDataContextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender.GetValue(FrameworkElement.DataContextProperty) == e.NewValue)
      {
        FocusControl(sender);
      }
    }
    #endregion

    #region Immediate


    public static object GetImmediate(DependencyObject obj)
    {
      return obj.GetValue(ImmediateProperty);
    }

    public static void SetImmediate(DependencyObject obj, object value)
    {
      obj.SetValue(ImmediateProperty, value);
    }

    // Using a DependencyProperty as the backing store for Immediate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ImmediateProperty =
      DependencyProperty.RegisterAttached("Immediate", typeof(object), typeof(SetSelection),
        new PropertyMetadata(false, (s, _) =>
        {
          if (GetSuppressImmediate(s)) return;
          FocusControl(s);
        }));



    public static bool GetSuppressImmediate(DependencyObject obj)
    {
      return (bool)obj.GetValue(SuppressImmediateProperty);
    }

    public static void SetSuppressImmediate(DependencyObject obj, bool value)
    {
      obj.SetValue(SuppressImmediateProperty, value);
    }

    // Using a DependencyProperty as the backing store for SuppressImmediate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuppressImmediateProperty =
        DependencyProperty.RegisterAttached("SuppressImmediate", typeof(bool), typeof(SetSelection),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));


    #endregion

    #region ElementToSelect


    public static FrameworkElement GetElementToSelect(DependencyObject obj)
    {
      return (FrameworkElement)obj.GetValue(ElementToSelectProperty);
    }

    public static void SetElementToSelect(DependencyObject obj, FrameworkElement value)
    {
      obj.SetValue(ElementToSelectProperty, value);
    }

    // Using a DependencyProperty as the backing store for ElementToSelect.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ElementToSelectProperty =
      DependencyProperty.RegisterAttached("ElementToSelect", typeof(FrameworkElement),
        typeof(SetSelection), new UIPropertyMetadata(null));


    #endregion

    #region FocusChildByDataContext

    public static void FocusChildByDataContext(this FrameworkElement parent, object dataContext)
    {
      var target = FrameworkElementFromDataContext(parent, dataContext);
      if (target != null)
      {
        FocusControl(target);
      }
    }

    public static FrameworkElement? FrameworkElementFromDataContext(FrameworkElement parent, object dataContext)
    {
      var target = parent.Descendants().OfType<FrameworkElement>()
        .FirstOrDefault(i => i.DataContext == dataContext);
      return target;
    }

    #endregion

    public static void FocusControl(DependencyObject sender)
    {
      var senderAsInput = sender as IInputElement;
      if ((senderAsInput != null) && (senderAsInput.Focusable))
      {
        SetFocus(senderAsInput);
      }
      else
      {
        sender.Dispatcher.BeginInvoke((Action)(() =>
        {
          var elt = GetElementToSelect(sender);
          if (elt == null)
          {
            elt = GetElementToFocus(sender);
          }
          if (elt != null)
          {
            SetFocus(elt);
          }
        }), DispatcherPriority.ApplicationIdle);
      }
    }

    private static FrameworkElement? GetElementToFocus(DependencyObject sender) =>
      sender.Descendants().
        OfType<FrameworkElement>().
        FirstOrDefault(i => i.Focusable && i.IsEnabled && i.IsVisible);

    private static bool IsFocusableType(FrameworkElement element)
    {
      var type = element.GetType();
      return typeof(IInputElement).IsAssignableFrom(type) || typeof(DatePicker).IsAssignableFrom(type);
    }

    private static void SetFocus(IInputElement senderAsInput)
    {
      try
      {
        if (senderAsInput is FrameworkElement fe)
        {
          fe.BringIntoView();
        }

        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, (Action)(() =>
        {
          senderAsInput.Focus();
          Keyboard.PrimaryDevice.Focus(senderAsInput);
        }));

        //        //This hack works arround a bug in datepicker -- focusing a datepicker does not focus the textbox
        if (senderAsInput is DatePicker)
        {

          var eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
            Keyboard.PrimaryDevice.ActiveSource ?? PresentationSource.FromVisual(senderAsInput as Visual),
            0,
            Key.Up);
          eventArgs.RoutedEvent = DatePicker.KeyDownEvent;

          Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => senderAsInput.RaiseEvent(eventArgs)), DispatcherPriority.ApplicationIdle);
        }

        (senderAsInput as TextBox)?.SelectAll();
      }
      catch (InvalidOperationException)
      {
      }
    }
  }
  public static class DependencyObjectExtensions
  {
    /// <summary>
    /// Returns the passed object, followed by all its descendants
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public static IEnumerable<DependencyObject> Parents(this DependencyObject root)
    {
      return FunctionalMethods.Sequence(root, i => VisualTreeHelper.GetParent(i));
    }

    public static IEnumerable<DependencyObject> Descendants(this DependencyObject item)
    {
      return (new[] { item }).SelectRecursive(Children);
    }

    private static IEnumerable<DependencyObject> Children(DependencyObject source)
    {
      if (!(source is Visual || source is Visual3D)) return new DependencyObject[0];
      return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(source)).Select(i => VisualTreeHelper.GetChild(source, i));
    }
  }

}