using System.IO;
using System.Threading;
using Melville.INPC;
using Melville.MVVM.Maui.Commands;
using Melville.MVVM.WaitingServices;
using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.WaitingService;

public class ShowProgressImplementation(INavigation navigation): IShowProgress
{
    public IShowProgressContext ShowProgress(string waitMessage,
        double total = double.MinValue, CancellationTokenSource? cancel = null)
    {
        var model = new ShowProgressContext(navigation)
        {
            WaitMessage = waitMessage,
            Total = total,
            CancellationTokenSource = cancel
        };
        navigation.PushModalAsync(new ShowProgressView(model));
        return model;
    }  
}

public partial class AppNavigationSource(Application app): INavigation
{
    [DelegateTo] INavigation Target => app.NavigationProxy ?? 
                                       app.MainPage?.Navigation ??
                                      throw new InvalidDataException("Need a navigation proxy");
}

public class WaitableApplication : Application
{
    public WaitableApplication()
    {
        InheritedCommand.SetInheritedCommandParameter(
            this, new ShowProgressImplementation(
                new AppNavigationSource(this)));
    }
}