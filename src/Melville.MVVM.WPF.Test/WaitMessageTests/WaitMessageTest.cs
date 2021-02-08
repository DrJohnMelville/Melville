#nullable disable warnings
using  System.Threading;
using Melville.MVVM.Wpf.WaitingServices;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.MVVM.WPF.Test.WaitMessageTests
{
  public sealed class WaitMessageTest
  {

    private readonly WaitMessageDriver sut = new WaitMessageDriver();

    [Fact]
    public void Properties()
    {
      new BusinessObjectTester<WaitMessageDriver>(() => new WaitMessageDriver())
        .Property(i => i.WaitMessage, "xxx")
        .Property(i => i.ErrorMessage, "xxx")
        .Property(i => i.Progress, 10.0)
        .Property(i => i.Total, 10.0, i => i.ShowProgress)
        .Property(i => i.CancellationTokenSource, new CancellationTokenSource(),
          i => i.CancellationToken, i => i.ShowCancelButton)
        .DoTests();
    }

    [Fact]
    public void WaitWithMessage()
    {
      Assert.Null(sut.WaitMessage);
      using (sut.WaitBlock("Foo Bar"))
      {
        Assert.Equal("Foo Bar", sut.WaitMessage);

      }
      Assert.Null(sut.WaitMessage);
    }

    [Fact]
    public void WaitCancelsError()
    {
      sut.ErrorMessage = "Foo Bar";
      sut.WaitBlock("Baz");
      Assert.Null(sut.ErrorMessage);
    }

    [Fact]
    public void WAitWithMaximum()
    {
      Assert.Equal(double.MinValue, sut.Total);
      Assert.Null(sut.CancellationTokenSource);
      using (sut.WaitBlock("", 120))
      {
        Assert.Equal(120.0, sut.Total);
        Assert.Null(sut.CancellationTokenSource);
      }
      Assert.Equal(double.MinValue, sut.Total);
      Assert.Null(sut.CancellationTokenSource);
    }

    [Fact]
    public void WaitBlockResetsTotal()
    {
      sut.Progress = 10;
      sut.WaitBlock("XX", 15);
      Assert.Equal(0.0, sut.Progress);

    }


    [Fact]
    public void WaitWithCancellation()
    {
      Assert.Null(sut.CancellationTokenSource);
      using (sut.WaitBlock("", 10, true))
      {
        Assert.NotNull(sut.CancellationTokenSource);
      }
      Assert.Null(sut.CancellationTokenSource);
    }

    [Fact]
    public void MakeProgress()
    {
      Assert.Equal(0.0, sut.Progress);
      sut.MakeProgress();
      Assert.Equal(1.0, sut.Progress);
    }


  }
}