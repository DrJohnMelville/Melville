using System.Windows.Input;
using ABI.Windows.Foundation.Collections;
using Melville.INPC;
using Melville.MVVM.Wpf.MouseClicks;

namespace Melville.Wpf.Samples.MouseClicks
{
    public sealed partial class MouseClickViewModel
    {
        [AutoNotify] private string message = "Mouse Clicks";

        public void MouseDownHandler(IMouseClickReport report)
        {
            Message = $"Clicked ({report.AbsoluteLocation()}) / ({report.RelativeLocation()})";
        }
    }
}