using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using Melville.Mvvm.CsXaml.ValueSource;

namespace Melville.Mvvm.CsXaml
{
    public static class TextBlockCreators
    {
        public static TextBlock ChildTextBlock<TTarget, TDataContext>(
            this in XamlBuilder<TTarget, TDataContext> target, 
            ValueProxy<string> text, 
            ValueProxy<double>? fontSize = null,
            ValueProxy<HorizontalAlignment>? horizontalAlignment = null) where TTarget: DependencyObject
        {
            var textBlock = new TextBlock();
            text.SetValue(textBlock, TextBlock.TextProperty);
            fontSize?.SetValue(textBlock, TextBlock.FontSizeProperty);
            horizontalAlignment?.SetValue(textBlock, TextBlock.HorizontalAlignmentProperty);
            return target.Child(textBlock);
        }

    }
}