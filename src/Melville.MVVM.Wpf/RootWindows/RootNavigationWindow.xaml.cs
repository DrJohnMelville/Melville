using System;
using System.Windows;
using System.Windows.Media.Imaging;
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

        /// <summary>
        /// To set the window icon include an ICO file in the project as a RESOURCE.
        /// the call this method.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly containing the icon resource</param>
        /// <param name="iconPath">path and file name of the icom resource</param>
        public void SetWindowIconFromResource(string assemblyName, string iconPath) => 
            Icon = BitmapFrame.Create(new Uri($"pack://application:,,,/{assemblyName};component/{iconPath}"));
        
    }
    
}