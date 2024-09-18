using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.WaitingService;

public partial class ShowProgressView : ContentPage
{
	public ShowProgressView(ShowProgressContext context)
	{
		InitializeComponent();
		BindingContext = context;
    }
}