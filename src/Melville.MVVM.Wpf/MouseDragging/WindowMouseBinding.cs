using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging
{
    public class WindowMouseBinding
    {
        private readonly Action<MouseMessageType, Point> reportMove;
        private readonly FrameworkElement target;
        private readonly DependencyObject clickSource;
        private readonly MouseButton draggedButton;

        public WindowMouseBinding(
            FrameworkElement target, 
            Action<MouseMessageType, Point> reportMove, MouseButton draggedButton)
        {
            this.reportMove = reportMove;
            this.target = target;
            clickSource = TopmostMouseSource();
            this.draggedButton = draggedButton;
            BindToMouse();
        }

        //The verifier cannot verify this is legal, and Rider reports the cast as suspicious.
        // however TopmostMouseSource ensures that the host implements both IInputElement and
        //DependencyObject
        private IInputElement CaptureHost => (IInputElement) clickSource;

        private DependencyObject TopmostMouseSource() =>
            target.Parents()
                .OfType<IInputElement>() // must be both a types
                .OfType<DependencyObject>()
                .Last();

        private void BindToMouse()
        {
            CaptureHost.CaptureMouse();
            Mouse.AddMouseMoveHandler(clickSource, SendMouseMove);
            Mouse.AddMouseUpHandler(clickSource, SendMouseUp);
        }
        
        private void SendMouseMessage(MouseMessageType type, MouseEventArgs args)
        {
            reportMove(type, args.GetPosition(target));
            args.Handled = true;
        }

        private void SendMouseMove(object sender, MouseEventArgs e) => 
            SendMouseMessage(MouseMessageType.Move, e);

        private void SendMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsButtonThatInitiatedDrag(e)) return;
            SendMouseMessage(MouseMessageType.Up, e);
            ReleaseBindings();
        }

        private bool IsButtonThatInitiatedDrag(MouseButtonEventArgs e) => 
            e.ChangedButton == draggedButton;

        public void ReleaseBindings()
        {
            Mouse.RemoveMouseMoveHandler(clickSource, SendMouseMove);
            Mouse.RemoveMouseUpHandler(clickSource, SendMouseUp);
            CaptureHost.ReleaseMouseCapture();
        }
    }
}