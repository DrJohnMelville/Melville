using Melville.MVVM.BusinessObjects;

namespace Melville.Wpf.Samples.WebView2Integration;

public class WebView2ViewModel: NotifyBase
{
    private bool loading;
    public bool Loading
    {
        get => loading;
        set => AssignAndNotify(ref loading, value);
    }

    public void NavigationStarting() => Loading = true;
    public void NavigationEnding() => Loading = false;

}