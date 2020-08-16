#nullable disable warnings
using  System;
using System.Reflection;
using Melville.MVVM.BusinessObjects;
using Melville.TestHelpers.Assertions;
using Melville.TestHelpers.InpcTesting;
using Xunit;
using Xunit.Sdk;

namespace Melville.TestHelpers.Test.InpcTesting
{
  public sealed partial class BusinessObjectTesterTest
  {
    private class HasDefaultConstructor { }
    public enum TryAnEnum { Zero, One, Two, Three};
    private class DoesNotNotify:NotifyBase
    {
      public int IntProperty { get; set; }
    }
    [Fact]
    public void ImputeSimpleProperties()
    {
      BusinessObjectTester.Create<MondoBusinessObject>().DoTests();
    }

    [Fact]
    public void SerialieSucceed()
    {
      BusinessObjectTester.Create(()=>new MondoBusinessObject(), i=>i).DoTests();
    }
    [Fact]
    public void SerialieFail()
    {
      Assert.Throws<AssertActualExpectedException>(()=>
      BusinessObjectTester.Create(()=>new MondoBusinessObject(), i=>new MondoBusinessObject()).DoTests());
    }


    [Fact]
    public void ImputedPropertyFailsInpc()
    {
      Assert.Throws<EqualException>(()=>BusinessObjectTester.Create<DoesNotNotify>().DoTests());
    }
    [Fact]
    public void SimpleObjectFail()
    {
      Assert.Throws<EqualException>(()=>BusinessObjectTester.Create<DoesNotNotify>()
        .Property(i=>i.IntProperty, 2)
        .DoTests());
    }

    [Fact]
    public void CheckSimpleObject()
    {
      BusinessObjectTester.Create<MondoBusinessObject>()
        .Property(i => i.Int32Property, 12)
        .DoTests();
    }

    private class NonDefaultConstructor:NotifyBase
    {
      public NonDefaultConstructor Other { get; set; }
      public NonDefaultConstructor(int x) { }
    }

    [Fact]
    public void CannotConstructField()
    {
      AssertEx.Throws<TrueException>("Other is not tested in the property test.\r\nExpected: True\r\nActual:   False", () => 
          BusinessObjectTester.Create(()=>new NonDefaultConstructor(1)).DoTests());
    }

    [Fact]
    public void IgnoreField()
    {
      BusinessObjectTester.Create(()=>new NonDefaultConstructor(1))
        .ExcludedProperty(i=>i.Other)
        .DoTests();
    }

    [Fact]
    public void Explicit()
    {
      Assert.Throws<EqualException>(()=>
      BusinessObjectTester.Create(()=>new NonDefaultConstructor(1))
        .Property(i=>i.Other, new NonDefaultConstructor(2))
        .DoTests());
    }


    private class DependantProp : NotifyBase
    {
      private int prop1;
      public int Prop1
      {
        get => prop1;
        set => AssignAndNotify(ref prop1, value, nameof(Prop1), nameof(Prop2), nameof(Prop3));
      }

      public int Prop2 { get; }
      public int Prop3 { get; }

    }

    [Fact]
    public void MultiPropFail()
    {
      Assert.Throws<EqualException>(()=>BusinessObjectTester.Create(() => new DependantProp()).DoTests());
    }
    [Fact]
    public void MultiPropFail2()
    {
      Assert.Throws<EqualException>(()=>BusinessObjectTester.Create(() => new DependantProp())
        .Property(i=>i.Prop1, 1, i=>i.Prop2)
        .DoTests());
    }

    [Fact]
    public void MultiPropSucceed()
    {
      BusinessObjectTester.Create(() => new DependantProp())
        .Property(i=>i.Prop1, 1, i=>i.Prop2, i=>i.Prop3)
        .DoTests();
    }

  }
}