using System.Windows.Input;
using ABI.Windows.Foundation.Collections;
using Melville.INPC;

namespace Melville.Wpf.Samples.MouseClicks
{
    public sealed partial class MouseClickViewModel
    {
        [AutoNotify] private string message = "Mouse Clicks";

        public void MouseDownHandler(MouseButtonEventArgs e, MouseClickView view)
        {
            Message = "Clicked " + e.GetPosition(view);
        }
    }
}