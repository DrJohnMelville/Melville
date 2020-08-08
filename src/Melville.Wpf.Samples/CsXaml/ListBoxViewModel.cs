using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    public class A { public string AProp => "A Prop";}
    public class B { public string BProp => "B Prop";}
    public class ListBoxViewModel: NotifyBase
    {
        private ListBoxElt? current;
        public ListBoxElt? Current
        {
            get => current;
            set => AssignAndNotify(ref current, value);
        }

        public List<object> AllTypes => new List<object>()
        {
            new A(),
            new B(),
            new ListBoxElt("Elt item",4),
            new A(),
            new B(),
            new ListBoxElt("Elt item",4),
        };
        

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
                    .WithChild(Create.ListBox(b.BindList(i=>i.Source, BindingMode.OneWay), b.Bind(i=>i.Current!), 
                        CreateListView))
                    .WithChild(Create.TextBlock("The Text is:").WithChild(Create.TextBlock(b.Bind(i=>i.Current!.Word))))
                    .WithChild(Create.TextBlock("The Number is:").WithChild(Create.TextBlock(
                        b.Bind(i=>i.Current!.Number).As<string>())))
                    .WithChild(Create.TextBlock("Multi types").WithMargin(30))
                    .WithResource(Create.DataTemplate<A>(f=>Create.TextBlock("This is the A").WithForeground(Brushes.Red)))
                    .WithResource(Create.DataTemplate<B>(f=>Create.TextBlock("This is the B").WithForeground(Brushes.Blue)))
                    .WithResource(Create.DataTemplate<ListBoxElt>(CreateListView2))
                    .WithChild(Create.ItemsControl(b.BindList(i=>i.AllTypes)))
                )
            );
        }
        private object CreateListView(BindingContext<ListBoxElt> dc) =>
            new StackPanel()
                .WithOrientation(Orientation.Horizontal)
                .WithChild(Create.TextBlock(dc.Bind(i => i.Word))
                    .WithMargin(new Thickness(0, 0, 20, 0))
                ).WithChild(Create.TextBlock(dc.Bind(i => i.Number).As<string>()));

        private object CreateListView2(BindingContext<ListBoxElt> dc) =>
            Create.Border((2,2,1,1), Brushes.Gold)
                .WithChild(
                    new StackPanel()
                        .WithOrientation(Orientation.Horizontal)
                        .WithChild(Create.TextBlock(dc.Bind(i => i.Word))
                            .WithMargin(new Thickness(0, 0, 20, 0))
                        ).WithChild(Create.TextBlock(dc.Bind(i => i.Number).As<string>()))
                );
    }
}