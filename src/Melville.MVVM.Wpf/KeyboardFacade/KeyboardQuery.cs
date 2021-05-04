using  System.Windows.Input;

namespace Melville.MVVM.Wpf.KeyboardFacade
{
  public interface IKeyboardQuery
  {
    ModifierKeys Modifiers { get; }
    bool IsDown(Key key);
    bool IsShiftDown => (Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
    bool IsAltDown => (Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
    bool IsControlDown => (Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
    bool IsWindowsDown => (Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows;
    bool IsOnlyShiftDown => Modifiers == ModifierKeys.Shift;
    bool IsOnlyAltDown => Modifiers == ModifierKeys.Alt;
    bool IsOnlyControlDown => Modifiers == ModifierKeys.Control;
    bool IsOnlyWindowsDown => Modifiers == ModifierKeys.Windows;
  }

  public sealed class KeyboardQuery : IKeyboardQuery
  {
    public ModifierKeys Modifiers => Keyboard.Modifiers;
    public bool IsDown(Key key) => Keyboard.IsKeyDown(key);
  }
}