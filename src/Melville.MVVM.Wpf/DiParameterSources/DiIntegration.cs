using System.Windows;

namespace Melville.MVVM.Wpf.DiParameterSources
{
    public static class DiIntegration
    {
        private static readonly DependencyProperty CoontainerProperty = DependencyProperty.RegisterAttached("Container",
            typeof(IDIIntegration), typeof(DiIntegration),
            new FrameworkPropertyMetadata(new EmptyScopeFactory(), FrameworkPropertyMetadataOptions.Inherits));

        private const string rootDiKey = "Melville.MVVM.WPF.IDIIntegration";
        public static IDIIntegration GetContainer(DependencyObject obj) =>
            (IDIIntegration) obj.GetValue(CoontainerProperty);

        public static void SetContainer(DependencyObject obj, IDIIntegration value) =>
            obj.SetValue(CoontainerProperty, value);
        
        public static IDIIntegration SearchForContainer(DependencyObject obj)
        {
            var ret = GetContainer(obj);
            return ret is EmptyScopeFactory ? SearchApplicationRoot(Application.Current) : ret;
        }

        public static void AttachDiRoot(this Application app, IDIIntegration value) => 
            app.Resources.Add(rootDiKey, value);

        public static IDIIntegration SearchApplicationRoot(Application app) => 
            app?.Resources[rootDiKey] as IDIIntegration ?? new EmptyScopeFactory();
    }
}