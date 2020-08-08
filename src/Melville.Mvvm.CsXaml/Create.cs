using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.Mvvm.CsXaml.XamlBuilders;
using Melville.MVVM.Functional;

namespace Melville.Mvvm.CsXaml
{
    public static class 
        Create
    {
        public static Button Button(
            ValueProxy<object> content,
            EventBinder clickBinder,
            ValueProxy<bool>? enabled = null) =>
            new Button()
                .WithContent(content)
                .WithIsEnabled(enabled)
                .BindEvent(ButtonBase.ClickEvent, clickBinder);

        public static TextBlock TextBlock(
            ValueProxy<string> text, 
            ValueProxy<double>? fontSize = null,
            ValueProxy<HorizontalAlignment>? horizontalAlignment = null) =>
            new TextBlock()
                .WithText(text).WithStyle(new Style())
                .WithHorizontalAlignment(horizontalAlignment)
                .WithTextBlock_FontSize(fontSize);
        public static TextBox TextBox(
            ValueProxy<string> text, 
            ValueProxy<double>? fontSize = null,
            ValueProxy<bool>? isReadOnly = null) =>
            new TextBox()
                .WithText(text).WithStyle(new Style())
                .WithTextBlock_FontSize(fontSize)
                .WithIsReadOnly(isReadOnly);


        public static ListBox ListBox<T>(
            ValueProxy<IEnumerable<T>> source,
            ValueProxy<T>? selectedItem = null,
            Func<BindingContext<T>, object>? dataTemplate = null) =>
            new ListBox()
                .WithItemsSource(source.As<IEnumerable>())
                .WithSelectedItem(selectedItem?.As<Object>())
                .WithItemTemplate(dataTemplate == null ? 
                    (ValueProxy<DataTemplate>?)null : 
                    new ValueProxy<DataTemplate>(DataTemplate(dataTemplate)));

        public static ItemsControl ItemsControl<T>(
            ValueProxy<IEnumerable<T>> source,
            Func<BindingContext<T>, object>? dataTemplate = null) =>
            new ItemsControl()
                .WithItemsSource(source.As<IEnumerable>())
                .WithItemTemplate(dataTemplate == null ? 
                    (ValueProxy<DataTemplate>?)null : 
                    new ValueProxy<DataTemplate>(DataTemplate(dataTemplate)));

        public static ItemsControl DataGrid<T>(
            ValueProxy<IEnumerable<T>> source,
            Func<DataGridColumnGenerator<T>, IEnumerable<DataGridColumn>>? columns = null)
        {
            var ret = new DataGrid()
                .WithItemsSource(source.As<IEnumerable>());
            if (columns != null)
            {
                ret.AutoGenerateColumns = false;
                ret.Columns.AddRange(columns(new DataGridColumnGenerator<T>()));
            }
            return ret;
        }

        public static Border Border(
            ThicknessValueProxy? thickness = null,
            ValueProxy<Brush>? borderColor = null,
            ValueProxy<Brush>? backgroundBrush = null) =>
            new Border()
                .WithBorderThickness(thickness)
                .WithBorderBrush(borderColor)
                .WithBackground(backgroundBrush);

        public static DataTemplate DataTemplate<T>(Func<BindingContext<T>, object> factory)
        {
            var dataTemplate = TemplateGenerator.CreateDataTemplate(() => factory(new BindingContext<T>()));
            dataTemplate.DataType = typeof(T);
            return dataTemplate;
        }
    }
}
