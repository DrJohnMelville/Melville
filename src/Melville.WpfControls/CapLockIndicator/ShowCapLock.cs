using  System;
using System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.CapLockIndicator;

public sealed class ShowCapLock : ContentControl
{
  public bool ShowMessage
  {
    get { return (bool)GetValue(ShowMessageProperty); }
    set { SetValue(ShowMessageProperty, value); }
  }
  public static readonly DependencyProperty ShowMessageProperty =
    DependencyProperty.Register("ShowMessage", typeof(bool), typeof(ShowCapLock), new PropertyMetadata(false));

  static ShowCapLock()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(ShowCapLock), new FrameworkPropertyMetadata(typeof(ShowCapLock)));
  }

  public ShowCapLock()
  {
    Focusable = false;
    IsKeyboardFocusWithinChanged += (s, e) => RecomputeShowMessage();
    PreviewKeyDown += (s, e) => RecomputeShowMessage();
    PreviewKeyUp += (s, e) => RecomputeShowMessage();
  }

  private void RecomputeShowMessage()
  {
    ShowMessage = IsKeyboardFocusWithin && Console.CapsLock;
  }
}