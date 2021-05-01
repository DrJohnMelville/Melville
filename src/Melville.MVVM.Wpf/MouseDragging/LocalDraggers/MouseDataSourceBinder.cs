using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public static class MouseDataSourceBinder 
    {
        public static void BindDragger(this IMouseDataSource source, ILocalDragger<Point> dragger)
        {
            source.MouseMoved += (s, e) => dragger.NewPoint(e.MessageType, e.RawPoint);
        }
    }
}