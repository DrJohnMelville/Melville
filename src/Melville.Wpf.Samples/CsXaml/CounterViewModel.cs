using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Markup;
using System.Windows.Media;
using Accord;
using Melville.MVVM.BusinessObjects;
using Melville.Mvvm.CsXaml;
using Melville.Mvvm.CsXaml.XamlBuilders;

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
            AddChild(BuildXaml.Create<StackPanel, CounterViewModel>((i, dc) =>
            {
                i.WithChild(Create.TextBlock("Simple Counter Test", 18, HorizontalAlignment.Center));
                i.WithChild(new TextBlock()
                    .WithChild(Create.TextBlock("The current count is: "))
                    .WithChild(Create.TextBlock(dc.Bind(j=>j.Counter).As<string>()))
                );
                i.WithChild(Create.TextBlock(dc.Bind(i => i.Counter, ToRoman)));
                i.WithChild(Create.Button("Increment Counter", nameof(CounterViewModel.IncrementCounter)))
                    .HorizontalAlignment = HorizontalAlignment.Center;
            }));
        }
        
        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;            
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); 
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);            
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }
    }
}