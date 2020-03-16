using System.Windows;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.RootWindows
{
    public partial class RootNavigationWindow : Window
    {
        public RootNavigationWindow(INavigationWindow viewModel, IDIIntegration? diScopeFactory = null)
        {
            InitializeComponent();
            if (diScopeFactory != null)
            {
                DiIntegration.SetContainer(this,diScopeFactory);
            }
            DataContext = viewModel;
        }
    }
}