using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Melville.MVVM.BusinessObjects;
using Melville.Mvvm.CsXaml;

namespace Melville.Wpf.Samples.CsXaml
{
    public class CounterViewModel:NotifyBase
    {
        private int counter = 10;
        public int Counter
        {
            get => counter;
            set => AssignAndNotify(ref counter, value);
        }

        public void IncrementCounter() => Counter++;
    }

    public class CounterView : UserControl
    {
        public CounterView()
        {
            AddChild(BuildXaml.Create<StackPanel, CounterViewModel>(i =>
            {
                i.ChildTextBlock("Simple Counter Test", 18, HorizontalAlignment.Center);
                i.Child<TextBlock>(k =>
                {
                    k.ChildTextBlock("The current count is: ");
                    k.ChildTextBlock(j => j.Counter);
                });
                i.ChildButton("Increment Counter", nameof(CounterViewModel.IncrementCounter))
                    .HorizontalAlignment = HorizontalAlignment.Center;
            }));
        }
    }
}