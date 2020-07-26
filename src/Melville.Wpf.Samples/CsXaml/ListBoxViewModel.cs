using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Melville.MVVM.BusinessObjects;
using Melville.Mvvm.CsXaml;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Wpf.Samples.CsXaml
{
    public class ListBoxElt
    {
        public string Word { get; }
        public int Number { get; }

        public ListBoxElt(string word, int number)
        {
            Word = word;
            Number = number;
        }
    }
    public class ListBoxViewModel: NotifyBase
    {
        private ListBoxElt? current;
        public ListBoxElt? Current
        {
            get => current;
            set => AssignAndNotify(ref current, value);
        }

        

        public ListBoxElt[] Source { get; } = new []
        {
            new ListBoxElt("One", 1),
            new ListBoxElt("Two", 2),
            new ListBoxElt("Three", 3),
        };
    }

    public class ListBoxView : UserControl
    {
        public ListBoxView()
        {
            AddChild(BuildXaml.Create<StackPanel, ListBoxViewModel>((e, b) =>
                e   .WithChild(Create.TextBlock("List Box Demo", 18, HorizontalAlignment.Center))
                    .WithChild(Create.ListBox(b.Bind(i=>i.Source, BindingMode.OneWay), b.Bind(i=>i.Current),
                        CreateListView))
                    .WithChild(Create.TextBlock("The Text is:").WithChild(Create.TextBlock(b.Bind(i=>i.Current!.Word))))
                    .WithChild(Create.TextBlock("The Number is:").WithChild(Create.TextBlock(
                        b.Bind(i=>i.Current!.Number).As<string>())))
            ));
        }
        private object CreateListView(BindingContext<ListBoxElt> dc)
        {
            return new StackPanel()
                .WithOrientation(Orientation.Horizontal)
                .WithChild(Create.TextBlock(dc.Bind(i => i.Word))
                        .WithMargin(new Thickness(0, 0, 20, 0))
                ).WithChild(Create.TextBlock(dc.Bind(i => i.Number).As<string>()));
        }
    }
}