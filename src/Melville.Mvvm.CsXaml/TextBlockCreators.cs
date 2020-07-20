using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace Melville.Mvvm.CsXaml
{
    public static class TextBlockCreators
    {
        public static TextBlock ChildTextBlock<TTarget, TDataContext>(
            this in XamlBuilder<TTarget, TDataContext> target, 
            Expression<Func<TDataContext,object>> textFunc, 
            double fontSize = -1,
            HorizontalAlignment? horizontalAlignment = null) where TTarget: DependencyObject =>
            CreateTextBlockChild(target, fontSize, horizontalAlignment, 
                BuildXaml.Create<TextBlock, TDataContext>(i =>
                {
                    i.Bind(TextBlock.TextProperty, textFunc);
                }));

        public static TextBlock ChildTextBlock<TTarget, TDataContext>(
            this in XamlBuilder<TTarget, TDataContext> target, 
            string text, 
            double fontSize = -1,
            HorizontalAlignment? horizontalAlignment = null) where TTarget: DependencyObject =>
            CreateTextBlockChild(target, fontSize, horizontalAlignment, new TextBlock {Text = text});

        private static TextBlock CreateTextBlockChild<TTarget, TDataContext>(
            XamlBuilder<TTarget, TDataContext> target, 
            double fontSize,
            HorizontalAlignment? horizontalAlignment, 
            TextBlock block) where TTarget: DependencyObject
        {
            if (fontSize > 0) block.FontSize = fontSize;
            if (horizontalAlignment.HasValue) block.HorizontalAlignment = horizontalAlignment.Value;
            return target.Child(block);
        }
    }
}