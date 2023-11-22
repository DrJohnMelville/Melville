#nullable disable warnings
using  System;
using Melville.Mvvm.TestHelpers.TestWrappers;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Melville.Mvvm.Test.TestWrappers;

public sealed class TestWrapperTest
{
  private interface ISimpleInt
  {
    int Swap(int a, int b);
  }
  private class LambdaWrapper: ISimpleInt
  {
    private readonly ISimpleInt inner;
    private readonly Func<ISimpleInt, int, int, int> body;

    public LambdaWrapper(ISimpleInt inner, Func<ISimpleInt, int, int, int> body)
    {
      this.inner = inner;
      this.body = body;
    }

    public int Swap(int a, int b)
    {
      return body(inner, a, b);
    }
  }

  private void AssertException(Action code, string message)
  {
    try
    {
      code();
      Assert.Fail("Did not throw and exception");
    }
    catch (XunitException e)
    {
      Assert.Contains(message, e.Message);
        
    }
  }

  private void RunSimpleIntTest(Func<ISimpleInt, int, int, int> body, string result)
  {
    AssertException(() =>
    {
      var sut = new WrapperTest<ISimpleInt>(i => new LambdaWrapper(i, body));
      sut.AssertAllMethodsForward();
    }, result);
  }

  [Fact]
  public void WorkingExample()
  {
    var sut = new WrapperTest<ISimpleInt>(i => new LambdaWrapper(i, (target, a, b) => target.Swap(a, b)));
    sut.AssertAllMethodsForward();
  }

  [Fact]
  public void DoesNothing() => RunSimpleIntTest((target, a, b) => 1, "Call to Swap resulted in 0 target calls.");
  [Fact]
  public void RunTwice() => RunSimpleIntTest((target, a, b) => 
    { target.Swap(a, b); return target.Swap(a, b);}, "Call to Swap resulted in 2 target calls.");
  [Fact] public void SwapArgs() => RunSimpleIntTest((target, a, b) =>  
    target.Swap(b,a), "Method 'Swap' argument 0 mismatch.");
  [Fact] public void ReturnValue() => RunSimpleIntTest((target, a, b) =>  
    11+target.Swap(a,b), "Method 'Swap' return value mismatch.");
    
  public interface IWithProperties
  {
    int InOut { get; set; }
    int In { set; }
    int Out { get; }
  }

  [Fact]
  public void GetWorks()
  {
    var sut = new WrapperTest<IWithProperties>(target=>
    {
      var m = new Mock<IWithProperties>();
      m.SetupGet(i => i.Out).Returns(() => target.Out);
      m.SetupGet(i => i.InOut).Returns(() => target.InOut);
      m.SetupSet(i => i.InOut = It.IsAny<int>()).Callback((int v) => target.InOut = v);
      m.SetupSet(i => i.In = It.IsAny<int>()).Callback((int v) => target.In = v);
      return m.Object;
    });
      
    sut.AssertAllMethodsForward();
  }

}