using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.RootWindows
{
    public interface IRootNavigationWindow
    {
        void SetWindowIconFromResource(string assemblyName, string iconPath);
        void Show();
        object DataContext { get; set; }
    }
    public partial class RootNavigationWindow : Window, IRootNavigationWindow
    {
        public RootNavigationWindow(INavigationWindow viewModel)
        {
            InitializeComponent();
            if (Dispatcher == null) throw new InvalidProgramException("Dispatcher should not be null");
            UiThreadBuilder.RunOnUiThread = Dispatcher.Invoke;
            DataContext = viewModel;
            PreviewMouseDown += CheckForBackNavigateButton;
        }

        private void CheckForBackNavigateButton(object sender, MouseButtonEventArgs e)
        {
            if (!(IsBackNavigationButton(e) && DataContext is INavigationWindow navWin)) return;
            navWin.NavigateToPriorPage();
            e.Handled = true;
        }

        private static bool IsBackNavigationButton(MouseButtonEventArgs e) => 
            e.ChangedButton == MouseButton.XButton1 && e.ClickCount == 1;

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