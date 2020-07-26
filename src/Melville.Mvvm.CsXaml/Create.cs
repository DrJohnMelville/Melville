using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Mvvm.CsXaml
{
    public static class Create
    {
        public static Button Button(
            ValueProxy<object> content,
            EventBinder clickBinder,
            ValueProxy<bool>? enabled = null) =>
            new Button()
                .WithContent(content)
                .WithIsEnabled(enabled)
                .Bind(ButtonBase.ClickEvent, clickBinder);

        public static TextBlock TextBlock(
            ValueProxy<string> text, 
            ValueProxy<double>? fontSize = null,
            ValueProxy<HorizontalAlignment>? horizontalAlignment = null) =>
            new TextBlock()
                .WithText(text).WithStyle(new Style())
                .WithHorizontalAlignment(horizontalAlignment)
                .WithTextBlock_FontSize(fontSize);

        /*
        public static ListBox ListBox<T>(
            ValueProxy<T[]> source,
            ValueProxy<T>? selectedItem,
            Func<BindingContext<T>, object>? dataTemplate) =>
            ListBox(source.As<IEnumerable<T>>(), selectedItem, dataTemplate);
        */
        public static ListBox ListBox<T>(
            ValueProxy<IEnumerable<T>> source,
            ValueProxy<T>? selectedItem,
            Func<BindingContext<T>, object>? dataTemplate)
        {
            var ret = new ListBox()
                .WithItemsSource(source.As<IEnumerable>())
                .WithSelectedItem(selectedItem?.As<Object>())
                .WithItemTemplate(dataTemplate == null ? null : 
                    new ValueProxy<DataTemplate>(TemplateGenerator.CreateDataTemplate<T>(dataTemplate)));
            return ret;
        }
        
    }
    public static class TemplateGenerator
    {
        private sealed class TemplateGeneratorControl:
            ContentControl
        {
            internal static readonly DependencyProperty FactoryProperty = DependencyProperty.Register("Factory", typeof(Func<object>), typeof(TemplateGeneratorControl), new PropertyMetadata(null, _FactoryChanged));

            private static void _FactoryChanged(DependencyObject instance, DependencyPropertyChangedEventArgs args)
            {
                var control = (TemplateGeneratorControl)instance;
                var factory = (Func<object>)args.NewValue;
                control.Content = factory();
            }
        }

        public static DataTemplate CreateDataTemplate<T>(Func<BindingContext<T>, object> factory) =>
            CreateDataTemplate(() => factory(new BindingContext<T>()));
        
        
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