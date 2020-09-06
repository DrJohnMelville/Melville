using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Accessibility;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Mvvm.CsXaml
{
    // stolen from:  https://www.codeproject.com/Tips/808808/Create-Data-and-Control-Templates-using-Delegates
    public static class TemplateGenerator
    {
        public static TPar WithResource<TPar>(this TPar elt, object key, object value) where TPar: FrameworkElement
        {
            elt.Resources.Add(key, value);
            return elt;
        }
        public static TPar WithResource<TPar>(this TPar elt, DataTemplate value) where TPar : FrameworkElement => 
            WithResource(elt, value.DataTemplateKey, value);

        public static TPar WithResource<TPar,TStyle>(this TPar elt, Style<TStyle> value) where TPar : FrameworkElement => 
            WithResource(elt, typeof(TStyle), value);

        private sealed class TemplateGeneratorControl: ContentControl
        {
            internal static readonly DependencyProperty FactoryProperty = DependencyProperty.Register("Factory", 
                typeof(Func<object>), typeof(TemplateGeneratorControl), new PropertyMetadata(null, _FactoryChanged));

            private static void _FactoryChanged(DependencyObject instance, DependencyPropertyChangedEventArgs args)
            {
                var control = (TemplateGeneratorControl)instance;
                var factory = args.NewValue as Func<object>;
                if (factory == null) return;
                control.Content = factory();
            }
        }


        /// <summary>
        /// Creates a data-template that uses the given delegate to create new instances.
        /// </summary>
        public static DataTemplate CreateDataTemplate(Func<object> factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            var frameworkElementFactory = new FrameworkElementFactory(typeof(TemplateGeneratorControl));
            frameworkElementFactory.SetValue(TemplateGeneratorControl.FactoryProperty, factory);
      
            var dataTemplate = new DataTemplate(typeof(DependencyObject));
            dataTemplate.VisualTree = frameworkElementFactory;
            return dataTemplate;
        }

        /// <summary>
        /// Creates a control-template that uses the given delegate to create new instances.
        /// </summary>
        public static ControlTemplate CreateControlTemplate<T>(
            Type controlType, Func<BindingContext<T>, object> factory) =>
            CreateControlTemplate(controlType, () => factory(new BindingContext<T>()));
        public static ControlTemplate CreateControlTemplate(Type controlType, Func<object> factory)
        {
            if (controlType == null)
                throw new ArgumentNullException("controlType");

            if (factory == null)
                throw new ArgumentNullException("factory");

            var frameworkElementFactory = new FrameworkElementFactory(typeof(TemplateGeneratorControl));
            frameworkElementFactory.SetValue(TemplateGeneratorControl.FactoryProperty, factory);
      
            var controlTemplate = new ControlTemplate(controlType);
            controlTemplate.VisualTree = frameworkElementFactory;
            return controlTemplate;
        }
    }
}