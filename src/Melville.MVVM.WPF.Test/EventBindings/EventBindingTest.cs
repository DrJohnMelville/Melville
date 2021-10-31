#nullable disable warnings
using  System;
using System.Windows;
using System.Windows.Controls;
using Melville.MVVM.Wpf.EventBindings;
using Melville.MVVM.Wpf.EventBindings.SearchTree;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.EventBindings;

public sealed class EventBindingTest: TestWithServiceProvider
{
  public interface ITarget
  {
    bool NoParams();
    void NoParamsNoReturn();
    void HasEventArg(EventArgs e);
    void HasSource(FrameworkElement elt);
    void HasDataContext(int i);
    void HasDataContext(DateTime dt);
    void RequiresInt(int i);
    void OptionalInt(int i = 14);
  }

  public class Target2
  {
    public ITarget Target { get; set; }
  }
  private Mock<ITarget> target = new Mock<ITarget>();
  public EventBindingTest()
  {
    Elt.DataContext = target.Object;
  }

  private void FireMethod(string method)
  {
    var markup = new EventBinding(method);
    FireEvent(markup);
  }

  [WpfTheory]
  [InlineData(false)]
  [InlineData(true)]
  public void NoParams(bool funcResult)
  {
    target.Setup(i => i.NoParams()).Returns(funcResult);
    FireMethod("NoParams");
    target.Verify(i=>i.NoParams(), Times.Once);
    Assert.Equal(funcResult, RoutedEventArgs.Handled);
  }

  [WpfTheory]
  [InlineData(0)]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(10)]
  public void LimitCalls(int max)
  {
    var markup = new EventBinding("NoParams");
    if (max < 3)
    {
      markup.MaxCalls = max;
    }

    var del = markup.ProvideValue(CreateServiceProvider()) as Delegate;
    for (int i = 0; i < 10; i++)
    {
      del.DynamicInvoke(Elt, RoutedEventArgs);
    }
    target.Verify(i=>i.NoParams(), Times.Exactly(max));
  }


  [StaFact]
  public void CanCallVoidMethod()
  {
    FireMethod(nameof(ITarget.NoParamsNoReturn));
    target.Verify(i=>i.NoParamsNoReturn(), Times.Once);
  }

  [StaFact]
  public void CanMethodWithPath()
  {
    var newTarget = new Target2 {Target = target.Object};
    Elt.DataContext = newTarget;
    FireMethod("Target."+nameof(ITarget.NoParamsNoReturn));
    target.Verify(i=>i.NoParamsNoReturn(), Times.Once);
  }

  [StaFact]
  public void CanPushEventArgs()
  {
    FireMethod(nameof(ITarget.HasEventArg));
    target.Verify(i=>i.HasEventArg(RoutedEventArgs), Times.Once);
  }
  [StaFact]
  public void CanPushSource()
  {
    FireMethod(nameof(ITarget.HasSource));
    target.Verify(i=>i.HasSource(Elt), Times.Once);
  }
  [StaFact]
  public void CanPushDateModel()
  {
    var data = new DateTime(1975, 07, 28);
    Elt.DataContext = data;
    TargetSelector.SetTarget(Elt, target.Object);
    FireMethod(nameof(ITarget.HasDataContext));
    target.Verify(i=>i.HasDataContext(data), Times.Once);
  }
  [StaFact]
  public void CanPushParentDateModel()
  {
    var grid = new Grid();
    grid.Children.Add(Elt);
    grid.DataContext = 10;
    FireMethod(nameof(ITarget.HasDataContext));
    target.Verify(i=>i.HasDataContext(10), Times.Once);
  }
  [StaFact]
  public void CallOnParent()
  {
    var grid = new Grid();
    grid.Children.Add(Elt);
    grid.DataContext = target.Object;
    Elt.DataContext = "Fake";
    FireMethod(nameof(ITarget.NoParamsNoReturn));
    target.Verify(i=>i.NoParamsNoReturn(), Times.Once);
  }
  [StaFact]
  public void CallOnParentWithPath()
  {
    var grid = new Grid();
    grid.Children.Add(Elt);
    grid.DataContext = new Target2 {Target = target.Object};
    Elt.DataContext = "Fake";
    FireMethod("Target."+nameof(ITarget.NoParamsNoReturn));
    target.Verify(i=>i.NoParamsNoReturn(), Times.Once);
  }

  [StaFact]
  public void CannotFireWithoutParam()
  {
    FireMethod(nameof(ITarget.RequiresInt));
    target.Verify(i=>i.RequiresInt(It.IsAny<int>()), Times.Never);
  }
  [StaFact]
  public void CanFireOptionaleWithoutParam()
  {
    FireMethod(nameof(ITarget.OptionalInt));
    target.Verify(i=>i.OptionalInt(14), Times.Once);
  }

  [StaFact]
  public void RunArbitraryFunc()
  {
    var realTb = new TextBlock {Text = "Foo"};
    var ret = new VisualTreeRunner(realTb).Run( (TextBlock tb) => tb.Text);
    Assert.Equal(ret, ret);
  }
  [StaFact]
  public void RunArbitraryAction()
  {
    var realTB = new TextBlock {Text = "Foo"};
    string s = "";
    new VisualTreeRunner(realTB).Run((TextBlock tb) =>
    {
      s = tb.Text;
    });
    Assert.Equal("Foo", s);
  }

  [StaFact]
  public void RunStaticArbitraryAction()
  {
    var realTB = new TextBlock();
    new VisualTreeRunner(realTB).Run<TextBlock>(StaticModifyTb);
    Assert.Equal("Static Set", realTB.Text);
  }

  private static void StaticModifyTb(TextBlock tb) => tb.Text = "Static Set";
}