using  System.Windows.Input;

namespace Melville.MVVM.Wpf.KeyboardFacade
{
  public interface IKeyboardQuery
  {
    ModifierKeys Modifiers { get; }
    bool IsDown(Key key);
  }

  public sealed class KeyboardQuery : IKeyboardQuery
  {
    public ModifierKeys Modifiers => Keyboard.Modifiers;
    public bool IsDown(Key key) => Keyboard.IsKeyDown(key);
  }
}