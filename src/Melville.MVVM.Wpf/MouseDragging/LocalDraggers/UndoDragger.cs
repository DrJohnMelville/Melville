using System.Windows;
using Melville.MVVM.Undo;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class UndoDragger<T>: ILocalDragger<T> where T : struct
    {
        private readonly IUndoEngine undo;
        private readonly ILocalDragger<T> effector;
        private T initialPoint;

        public UndoDragger(IUndoEngine undo, ILocalDragger<T> effector)
        {
            this.undo = undo;
            this.effector = effector;
        }

        public void NewPoint(MouseMessageType type, T point)
        {
            switch (type)
            {
                case MouseMessageType.Down:
                    initialPoint = point;
                    break;
                case MouseMessageType.Move:
                    break;
                case MouseMessageType.Up:
                    undo.PushAndDoAction(
                        ()=>effector.NewPoint(MouseMessageType.Up, point),
                        ()=>effector.NewPoint(MouseMessageType.Up, initialPoint));
                    return;
            }
            effector.NewPoint(type, point);
        }
    }
}