using System.Threading;
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