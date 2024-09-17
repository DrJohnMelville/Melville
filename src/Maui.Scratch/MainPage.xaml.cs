using System.Windows.Input;
using Melville.INPC;

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

    public ICommand ClickCommand => new Command(Click);

    private void Click(object o)
    {
        Count++;
    }

    [AutoNotify] public string? NullWhenEven=> Count % 2 == 0 ? null : "Null When Even";
}