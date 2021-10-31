#nullable disable warnings
using  System.Windows.Input;
using Melville.MVVM.Wpf.EventBindings;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.EventBindings;

public sealed class CommandBindingTest : TestWithServiceProvider
{
  private readonly Mock<ICommand> target;
  private readonly EventBinding binding;
 
  public CommandBindingTest()
  {
    target = new Mock<ICommand>();
    Elt.DataContext = target.Object;
    target.Setup(i => i.CanExecute("Foo")).Returns(true);

    binding = new EventBinding("", "Foo");

  }

  private void FireEvent(string parameter)
  {
    binding.Parameters = parameter;
    FireEvent(binding);
  }

  [StaFact]
  public void Constructors()
  {
    var ecb2 = new EventBinding();
    Assert.Equal("", ecb2.MethodName);
    Assert.Equal("", ecb2.Parameters);
  }

  [StaFact]
  public void PlayACommand()
  {
    Assert.False(RoutedEventArgs.Handled);
    FireEvent("Foo");
    target.Verify(i => i.Execute("Foo"), Times.Once);
    Assert.True(RoutedEventArgs.Handled);
  }

  [StaFact]
  public void PassEventArgs()
  {
    target.Setup(i => i.CanExecute(RoutedEventArgs)).Returns(true);
    FireEvent("$arg");
    target.Verify(i => i.Execute(RoutedEventArgs), Times.Once);

  }
  [StaFact]
  public void PassAProperty()
  {
    FireEvent("$this.Tag");
    target.Verify(i => i.Execute("Foo"), Times.Once);
  }
  [StaFact]
  public void PassSender()
  { 
    target.Setup(i => i.CanExecute(Elt)).Returns(true);
    FireEvent("$this");
    target.Verify(i => i.Execute(Elt), Times.Once);
  }

  [StaFact]
  public void DoNotRunDisabledCommands()
  {
    var target = new Mock<ICommand>();
    FireEvent("Is Not Foo");

    target.Verify(i => i.Execute("Foo"), Times.Never);
  }
}