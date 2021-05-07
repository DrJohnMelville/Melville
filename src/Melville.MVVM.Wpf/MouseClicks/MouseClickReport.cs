using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseClicks
{
    public interface IMouseClickReport
    {
        bool IsLeft();
        bool IsMiddle();
        bool IsRight();
        bool IsDown();
        int ClickCount();
        Point AbsoluteLocation();
        Point RelativeLocation();
        Point PointRelativeTo(IInputElement element);

        IMouseDataSource DragBy(Func<FrameworkElement, FrameworkElement> dragItemSelector);
    }

    public static class MouseClickReportOperations
    {
        public static IMouseDataSource DragLeaf(this IMouseClickReport mcr) =>
            mcr.DragBy(i => i);
        public static IMouseDataSource DragTop(this IMouseClickReport mcr) =>
            mcr.DragBy(i => i.Parents().OfType<FrameworkElement>().Last());
        public static IMouseDataSource DragByName(this IMouseClickReport mcr, string name) =>
            mcr.DragBy(i => i.Parents()
                .OfType<FrameworkElement>()
                .First(i => i.Name.Equals(name, StringComparison.Ordinal)));
        public static IMouseDataSource DragByViewType<T>(this IMouseClickReport mcr) where T : FrameworkElement =>
            mcr.DragBy(i => i.Parents().OfType<T>().First());

        public static IMouseDataSource DragByViewType(this IMouseClickReport mcr, params Type[] dragTypes) =>
            mcr.DragBy(fe =>
                fe.Parents().OfType<FrameworkElement>().First(i => dragTypes.Any(j => j.IsInstanceOfType(i))));
    }
    
    public class MouseClickReport : IMouseClickReport
    {
        private readonly FrameworkElement target;
        private readonly MouseButtonEventArgs eventArgs;

        public MouseClickReport(FrameworkElement target, MouseButtonEventArgs eventArgs)
        {
            this.target = target;
            this.eventArgs = eventArgs;
        }

        public bool IsLeft() => eventArgs.ChangedButton == MouseButton.Left;
        public bool IsMiddle() => eventArgs.ChangedButton == MouseButton.Middle;
        public bool IsRight()  => eventArgs.ChangedButton == MouseButton.Right;
        public bool IsDown() => eventArgs.ButtonState == MouseButtonState.Pressed;
        public int ClickCount() => eventArgs.ClickCount;
        public Point AbsoluteLocation() => PointRelativeTo(target);
        public Point RelativeLocation()
        {
            var absPoint = AbsoluteLocation();
            return new Point(absPoint.X / target.ActualWidth, absPoint.Y / target.ActualHeight);
        }
        public Point PointRelativeTo(IInputElement element) => eventArgs.GetPosition(element);

        public IMouseDataSource DragBy(Func<FrameworkElement, FrameworkElement> dragItemSelector)
        {
            var ret = new WindowMouseDataSource(dragItemSelector(target));
            ret.BindToPhysicalMouse(eventArgs);
            return ret;
        }
    }
}