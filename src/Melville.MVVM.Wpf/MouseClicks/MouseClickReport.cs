using System;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.VisualTreeLocations;

namespace Melville.MVVM.Wpf.MouseClicks
{
    public interface IMouseClickReport: IVisualTreeLocation<IMouseClickReport, FrameworkElement>
    {
        bool IsLeft();
        bool IsMiddle();
        bool IsRight();
        bool IsDown();
        int ClickCount();
        Point AbsoluteLocation();
        Point RelativeLocation();
        Point PointRelativeTo(IInputElement element);
        Size TargetSize();

        IMouseDataSource DragSource();
    }

    public static class MouseClickReportOperations
    {
        public static IMouseClickReport ExtractSize(this IMouseClickReport mds, out Size size)
        {
            size = mds.TargetSize();
            return mds;
        }
        public static IMouseClickReport ExtractBounds(this IMouseClickReport mds, out Rect bounds)
        {
            bounds = new Rect(new Point(), mds.TargetSize());
            return mds;
        }
        
        [Obsolete("Use AttachToXXX overrides")]
        public static IMouseDataSource DragLeaf(this IMouseClickReport mcr) =>
            mcr.DragSource();
        [Obsolete("Use AttachToXXX overrides")]
        public static IMouseDataSource DragTop(this IMouseClickReport mcr) =>
            mcr.AttachToTop().DragSource();
        [Obsolete("Use AttachToXXX overrides")]
        public static IMouseDataSource DragByName(this IMouseClickReport mcr, string name) =>
            mcr.AttachToName(name).DragSource();
        [Obsolete("Use AttachToXXX overrides")]
        public static IMouseDataSource DragByViewType<T>(this IMouseClickReport mcr) where T : FrameworkElement =>
            mcr.AttachToType(typeof(T)).DragSource();
        [Obsolete("Use AttachToXXX overrides")]
        public static IMouseDataSource DragByViewType(this IMouseClickReport mcr, params Type[] dragTypes) =>
            mcr.AttachToDataContextHolder(dragTypes).DragSource();
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

        public IMouseDataSource DragSource()
        {
            var ret = new WindowMouseDataSource(target, eventArgs.GetPosition(target));
            ret.BindToPhysicalMouse(eventArgs);
            return ret;
        }

        FrameworkElement IVisualTreeLocation<IMouseClickReport, FrameworkElement>.Target => target;

        IMouseClickReport IVisualTreeLocation<IMouseClickReport, FrameworkElement>.
            CreateNewChild(FrameworkElement? otherTarget) => 
            otherTarget == null? this:new MouseClickReport(otherTarget, eventArgs);

        public Size TargetSize() => new Size(target.ActualWidth, target.ActualHeight);
    }
}