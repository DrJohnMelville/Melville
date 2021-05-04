using System.Windows.Input;
using Melville.MVVM.Wpf.KeyboardFacade;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class KeySwitcherDragger<T> : ILocalDragger<T> where T : struct
    {
        private readonly IKeyboardQuery keyboard;
        private readonly ModifierKeys activatingKeys;
        private readonly ILocalDragger<T> pressedTarget;
        private readonly ILocalDragger<T> defaultTarget;

        public KeySwitcherDragger(IKeyboardQuery keyboard, ModifierKeys activatingKeys, 
            ILocalDragger<T> pressedTarget, ILocalDragger<T> defaultTarget)
        {
            this.keyboard = keyboard;
            this.pressedTarget = pressedTarget;
            this.defaultTarget = defaultTarget;
            this.activatingKeys = activatingKeys;
        }

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
}