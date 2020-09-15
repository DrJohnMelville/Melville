using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Mvvm.CsXaml
{
    public struct DataGridColumnGenerator<T> : IBindingContext<T>
    {
        public DataGridComboBoxColumn ComboBox<TChoice>(
            ValueProxy<object>? value = null,
            ValueProxy<IEnumerable<TChoice>>? choices = null,
            ValueProxy<object>? header = null,
            ValueProxy<string>? displayMemberPath = null) =>
            new DataGridComboBoxColumnWithBinding()
                .WithSelectedValueBinding(value?.ForceBindingBase())
                .WithItemsSource(choices?.As<IEnumerable>())
                .WithHeader(header)
                .WithDisplayMemberPath(displayMemberPath);

        public DataGridCheckBoxColumn CheckBox(
            ValueProxy<bool>? value = null,
            ValueProxy<object>? header = null,
            ValueProxy<bool>? isReadOnly = null) =>
            new DataGridCheckBoxColumn()
                .WithBinding(value?.ForceBindingBase())
                .WithHeader(header)
                .WithIsReadOnly(isReadOnly);
        public DataGridTextColumn Text(
            ValueProxy<string>? text = null,
            ValueProxy<object>? header = null,
            ValueProxy<bool>? isReadOnly = null) =>
            new DataGridTextColumn()
                .WithBinding(text?.ForceBindingBase())
                .WithHeader(header)
                .WithIsReadOnly(isReadOnly);

        public DataGridTemplateColumn Template(
            ValueProxy<object>? header = null,
            Func<BindingContext<T>, object>? displayTemplate = null,
            Func<BindingContext<T>, object>? editingTemplate = null) =>
            new DataGridTemplateColumn()
                .WithHeader(header)
                .WithCellTemplate(MakeTemplate(displayTemplate))
                .WithCellEditingTemplate(MakeTemplate(editingTemplate, displayTemplate));

        private ValueProxy<DataTemplate> MakeTemplate(params Func<BindingContext<T>,object>?[] templates)
        {
            foreach (var template in templates)
            {
                if (template != null) return new ValueProxy<DataTemplate>(Create.DataTemplate(template));
            }
            return new ValueProxy<DataTemplate>(Create.DataTemplate<T>(i=>Create.TextBlock("No TemplateDefined")));
        }

        public Binding FixBinding(Binding source) => source;
    }
}