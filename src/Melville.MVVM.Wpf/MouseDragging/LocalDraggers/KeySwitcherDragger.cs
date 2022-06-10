using System.Windows.Input;
using Melville.INPC;
using Melville.MVVM.Wpf.KeyboardFacade;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public partial class KeySwitcherDragger<T> : ILocalDragger<T> where T : struct
{
    [FromConstructor]private readonly IKeyboardQuery keyboard;
    [FromConstructor]private readonly ModifierKeys activatingKeys;
    [FromConstructor]private readonly ILocalDragger<T> pressedTarget;
    [FromConstructor]private readonly ILocalDragger<T> defaultTarget;
    
    public void NewPoint(MouseMessageType type, T point)
    {
        if (type == MouseMessageType.Down)
        {
            // Several draggers cache the initial point to work their magic.  We send the
            // mouse down to both draggers, but to the nonpreferred dragger first, so the
            // message to the preferred dragger will overwrite the nonpreferred.
            SendMouseDownToBothDraggers(type, point);
        }
        else
        {
            SendMouseToActiveDragger(type, point);
        }
    }

    private void SendMouseToActiveDragger(MouseMessageType type, T point) => 
        PickDragger().Active.NewPoint(type, point);

    private void SendMouseDownToBothDraggers(MouseMessageType type, T point)
    {
        var (inactive, active) = PickDragger();
        inactive.NewPoint(type, point);
        active.NewPoint(type, point);
    }

    private (ILocalDragger<T> Inactive, ILocalDragger<T> Active) PickDragger() =>
        IsActivatingKeyComboPressed() ? 
            (defaultTarget, pressedTarget) : (pressedTarget, defaultTarget);

    private bool IsActivatingKeyComboPressed() => keyboard.Modifiers == activatingKeys;
}