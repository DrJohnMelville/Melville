using System;
using System.Windows.Input;
using Melville.INPC;
using Melville.MVVM.Wpf.EventBindings;

namespace Melville.MVVM.Wpf.KeyboardFacade;

public class KeyEventWrapper: IParameterListExpander
{
    public void Push(object? value, Action<object?> target)
    {
        TryWrapKeyboardEvent(value, target);
        target(value);
    }

    private void TryWrapKeyboardEvent(object? value, Action<object?> target)
    {
        if (value is KeyEventArgs kea)
            target(new KeyEventReport(kea));
    }
}

public interface IKeyEventReport: IHasModifiers
{
    Key RawKey { get; }
    Key Key { get; }
    int TimeStamp { get; }
    bool IsDown { get; }
    bool IsUp { get; }
    bool IsRepeat { get; }
    bool IsToggled { get; }
}
public partial class KeyEventReport: IKeyEventReport
{
    [FromConstructor] private readonly KeyEventArgs keyEvent;

    public ModifierKeys Modifiers => keyEvent.KeyboardDevice.Modifiers;
    public Key RawKey => keyEvent.Key;

    public Key Key => keyEvent.Key switch
    {
        Key.ImeProcessed => keyEvent.ImeProcessedKey,
        Key.System => keyEvent.SystemKey,
        Key.DeadCharProcessed => keyEvent.DeadCharProcessedKey,
        var x => x
    };

    public int TimeStamp => keyEvent.Timestamp;
    public bool IsDown => keyEvent.IsDown;
    public bool IsUp => keyEvent.IsUp;
    public bool IsRepeat => keyEvent.IsRepeat;
    public bool IsToggled => keyEvent.IsToggled;
}