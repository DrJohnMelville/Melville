#nullable disable warnings
using  System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Melville.TestHelpers.Assertions;
using Melville.TestHelpers.MockConstruction;
using Moq;
using Xunit;

namespace Melville.TestHelpers.Test.MockConstruction;

public sealed class CreateWithMocksTest
{
  private class SimpleObject
  {
    public int Value { get; set; }

    public SimpleObject(int value)
    {
      Value = value;
    }
  }

  [Fact]
  public void CanAddValueParam()
  {
    CreateWithMocks.Construct<SimpleObject>(12).InputValue<int>(out var val);
    Assert.Equal(12, val);
  }

  [Fact]
  public void CanAddValueParam2()
  {
    var val = CreateWithMocks.Construct<SimpleObject>(12).InputValue<int>();
    Assert.Equal(12, val);
  }

  [Fact]
  public void NoValueThrows()
  {
    AssertEx.Throws<InvalidOperationException>("No value of type String found.",
      () => CreateWithMocks.Construct<SimpleObject>().InputValue<string>());
  }

  [Fact]
  public void CreateSimpleObject()
  {
    var val = CreateWithMocks.Construct<SimpleObject>(10).Result(out var val2).Result();
    Assert.Equal(10, val.Value);
    Assert.Equal(10, val2.Value);
    Assert.NotEqual(val, val2);
  }

  [Fact]
  public void GenerateDefaultValueType()
  {
    var val = CreateWithMocks.Construct<SimpleObject>().InputValue(out int i).Result();
    Assert.Equal(0, val.Value);
    Assert.Equal(0, i);
  }

  public class WithInterface
  {
    public WithInterface(IDisposable value)
    {
      Value = value;
    }

    public IDisposable Value { get; }
  }

  [Fact]
  public void ConsumeMock()
  {
    var mockIn = new Mock<IDisposable>();
    var val = CreateWithMocks.Construct<WithInterface>(mockIn).Mock<IDisposable>(out var dispMock).Result();
    Assert.Equal(mockIn, dispMock);
    val.Value.Dispose();
    mockIn.Verify(i => i.Dispose(), Times.Once);
  }

  [Fact]
  public void CreateMock()
  {
    var mo = new Mock<IComparable<int>>();
    var val = CreateWithMocks.Construct<WithInterface>().Mock<IDisposable>(out var dispMock).Result();
    val.Value.Dispose();
    dispMock.Verify(i => i.Dispose(), Times.Once);
  }

  public interface IInnerProp
  {
    IDisposable InnerProp { get; }
  }

  private class InnerNotify
  {
    public InnerNotify(IInnerProp prop)
    {
      Prop = prop;
    }

    public IInnerProp Prop { get; set; }
  }

  [Fact]
  public void PropertyFilled()
  {
    var obj = CreateWithMocks.Construct<InnerNotify>().Mock<IDisposable>(out var disp).Result();
    obj.Prop.InnerProp.Dispose();
    disp.Verify(i => i.Dispose(), Times.Once);
  }


  private class HasEquatable
  {
    public HasEquatable(IEquatable<int> ints)
    {
      Ints = ints;
    }

    public IEquatable<int> Ints { get; }
  }

  [Fact]
  public void CreateCompositeMock()
  {
    var m = CreateWithMocks.Construct<HasEquatable>().Mock<IEquatable<int>>(out var enumerator).Result();
    enumerator.Setup(i => i.Equals(1)).Returns(true);
     
    Assert.True(enumerator.Object.Equals(1));
    Assert.False(enumerator.Object.Equals(2));
  }

  public interface IDependendInterface
  {
    bool IsConcrete { get; }
  }

  private class DependantClass : IDependendInterface
  {
    public bool IsConcrete => true;
  }

  public interface IOuterInt
  {
    IDependendInterface Value { get; }
  }

  [Fact]
  public void ComposeInterfaces()
  {
    Assert.False(CreateWithMocks.Mock<IOuterInt>().Result().Object.Value.IsConcrete);
  }
  [Fact]
  public void ComposeWithConcrete()
  {
    Assert.True(CreateWithMocks.Mock<IOuterInt>(CreateWithMocks.Concrete<DependantClass>()).Result().Object.Value.IsConcrete);
  }

  public interface IHolder<T>
  {
    T Value { get; }
  }

  public class ConcreteClass<T>
  {
    public ConcreteClass(IHolder<T> holder) { }
  }

  public T GenerateT<T>()

  {
    CreateWithMocks.Construct<ConcreteClass<T>>()
      .Mock<IHolder<T>>(out var holder);
    return holder.Object.Value;
  }
  [Fact]
  public void MockIntHolder()
  {
    Assert.Equal(0, GenerateT<int>());
      
  }

  [Theory]
  [InlineData(typeof(ArrayList), typeof(ArrayList))]
  [InlineData(typeof(List<int>), typeof(List<int>))]
  [InlineData(typeof(IList<int>), typeof(List<int>))]
  [InlineData(typeof(IEnumerable<int>), typeof(List<int>))]
  [InlineData(typeof(Collection<int>), typeof(Collection<int>))]
  [InlineData(typeof(Stack<int>), typeof(Stack<int>))]
  [InlineData(typeof(HashSet<int>), typeof(HashSet<int>))]
  [InlineData(typeof(SortedDictionary<int, string>), typeof(SortedDictionary<int, string>))]
  [InlineData(typeof(Dictionary<int, string>), typeof(Dictionary<int, string>))]
  [InlineData(typeof(ICollection<int>), typeof(Collection<int>))]
  [InlineData(typeof(ISet<int>), typeof(HashSet<int>))]
  [InlineData(typeof(IDictionary<int, string>), typeof(Dictionary<int, string>))]
  [InlineData(typeof(int[]), typeof(int[]))]
  public void ConstructIntArray(Type source, Type dest)
  {
    var ret = GetType().GetMethods().First(i => i.Name == "GenerateT")
      .MakeGenericMethod(new[] {source}).Invoke(this, new object[0]);
    Assert.Equal(dest, ret.GetType());
      
  }


}