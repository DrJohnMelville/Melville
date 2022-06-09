using System.Threading.Tasks;
using System.Windows;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RunOnWindowThreads;
using Melville.MVVM.Wpf.ViewFrames;

namespace Melville.Wpf.Samples.DialogBox;

public partial class PopupWindowSheetViewModel: ICreateView
{
    [AutoNotify] private string result = "No Dialog Triggered";

    public void LaunchDialog([FromServices] IMvvmDialog dlg, [FromServices] IRunOnWindowThread thread) =>
        dlg.ShowPopupWindow(new DialogViewModel(), 300, 150, "This is a DialogBox");

    public UIElement View()
    {
        return new DialogBoxSheetView();
    }
}