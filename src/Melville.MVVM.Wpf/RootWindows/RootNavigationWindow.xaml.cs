using System;
using System.Windows;
using System.Windows.Threading;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.RootWindows
{
    public partial class RootNavigationWindow : Window
    {
        public RootNavigationWindow(INavigationWindow viewModel)
        {
            InitializeComponent();
            if (Dispatcher == null) throw new InvalidProgramException("Dispatcher should not be null");
            UiThreadBuilder.RunOnUiThread = Dispatcher.Invoke;
            DataContext = viewModel;
        }
    }
    
}