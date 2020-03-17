using System.Windows;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.RootWindows
{
    public partial class RootNavigationWindow : Window
    {
        public RootNavigationWindow(INavigationWindow viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}