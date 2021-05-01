using System.Windows;
using System.Windows.Input;

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
    }
}