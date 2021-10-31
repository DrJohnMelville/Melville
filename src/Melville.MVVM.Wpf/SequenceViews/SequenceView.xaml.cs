using  System;
using System.Windows;
using System.Windows.Controls;

namespace Melville.MVVM.Wpf.SequenceViews;

/// <summary>
/// Interaction logic for SequenceView.xaml
/// </summary>
public partial class SequenceView : UserControl
{
    public SequenceView()
    {
        InitializeComponent();
    }



    public String DisplayMemberPath
    {
        get => (string)GetValue(MyPropertyProperty);
        set => SetValue(MyPropertyProperty, value);
    }
    public static readonly DependencyProperty MyPropertyProperty =
        DependencyProperty.Register("MyProperty", typeof(string), typeof(SequenceView), new PropertyMetadata(null));


}