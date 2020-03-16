using System.Windows;

namespace Melville.MVVM.Wpf.DiParameterSources
{
    public static class DiIntegration
    {
        private static readonly DependencyProperty CoontainerProperty = DependencyProperty.RegisterAttached("Container",
            typeof(IDIIntegration), typeof(DiIntegration),
            new FrameworkPropertyMetadata(new EmptyScopeFactory(), FrameworkPropertyMetadataOptions.Inherits));

        public static IDIIntegration GetContainer(DependencyObject obj) =>
            (IDIIntegration) obj.GetValue(CoontainerProperty);

        public static void SetContainer(DependencyObject obj, IDIIntegration value) =>
            obj.SetValue(CoontainerProperty, value);
    }
}