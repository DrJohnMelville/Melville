using System.Windows.Input;
using Melville.INPC;
using Melville.MVVM.Maui.Commands;
using Melville.MVVM.WaitingServices;

namespace Maui.Scratch;

public partial class MainPage : ContentPage
{ 
    public MainPage(MainPageViewModel viewModel)
    { 
        InitializeComponent();
        BindingContext = viewModel;
    }
}

public partial class MainPageViewModel
{
    [AutoNotify] private int count;

    [AutoNotify] public string CounterText => $"Clicked {Count} time{(Count == 1 ? "" : "s")}";

    public ICommand ClickCommand => InheritedCommandFactory.Create(Click);

    private async Task Click(IShowProgress sp)
    {
        var cancel = new CancellationTokenSource();
        using var wait = sp.ShowProgress("Waiting for Timer", 5, cancel);
        for (int i = 0; i < 5; i++)
        {
            if (cancel.Token.IsCancellationRequested) return;
            wait.MakeProgress($"Item # {i}");
            await Task.Delay(1000);
        }
        Count++;
    }

    [AutoNotify] public string? NullWhenEven=> Count % 2 == 0 ? null : "Null When Even";
}