#nullable disable warnings
using  System;
using Melville.TestHelpers.Assertions;
using Melville.TestHelpers.DeepComparisons;
using Xunit;

namespace Melville.TestHelpers.Test.DeepComparisons;

public sealed class DeepComparisonTest
{

    [Fact]
    public void EnumerableTest()
    {
        Assert.True(DeepComparison.AreSame(new int[0], new int[0]));
        Assert.True(DeepComparison.AreSame(new[] { 1 }, new[] { 1 }));
        Assert.True(DeepComparison.AreSame(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 }));
        Assert.False(DeepComparison.AreSame(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3 }));
        Assert.False(DeepComparison.AreSame(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 5 }));
    }

    [Fact]
    public void NullBehavior()
    {
        Assert.False(DeepComparison.AreSame(null, "true"));
        Assert.False(DeepComparison.AreSame("true", null));
        Assert.True(DeepComparison.AreSame(null, null));
    }

    [Fact]
    public void DifferentTypesAreDifferent()
    {
        Assert.False(DeepComparison.AreSame(true, "true"));
    }

    [Fact]
    public void OrdinalCompare()
    {
        Assert.True(DeepComparison.AreSame(1, 1));
        Assert.True(DeepComparison.AreSame(1.0, 1.0));
        Assert.True(DeepComparison.AreSame(StringComparison.CurrentCulture, StringComparison.CurrentCulture));
        Assert.True(DeepComparison.AreSame("Hello", "Hello"));
        Assert.True(DeepComparison.AreSame(true, true));

        Assert.False(DeepComparison.AreSame(1, 10));
        Assert.False(DeepComparison.AreSame(1.0, 1.09));
        Assert.False(DeepComparison.AreSame(StringComparison.InvariantCulture, StringComparison.CurrentCulture));
        Assert.False(DeepComparison.AreSame("Hello", "Hello1"));
        Assert.False(DeepComparison.AreSame(true, false));
    }

    private class SimpleCompareClass
    {
        public string Field1;
        public string Prop1 { get; set; }
    }

    private class FieldCompareClass
    {
        public string Prop1 { get; set; }
    }

    [Fact]
    public void SimplePropertyCompare()
    {
        var f1 = new SimpleCompareClass { Prop1 = "a" };
        var f2 = new SimpleCompareClass { Prop1 = "a" };
        var f3 = new SimpleCompareClass { Prop1 = "b" };

        Assert.True(DeepComparison.AreSame(f1, f2));
        Assert.False(DeepComparison.AreSame(f1, f3));
    }
    [Fact]
    public void SimpleFieldCompare()
    {
        var f1 = new FieldCompareClass { Prop1 = "a" };
        var f2 = new FieldCompareClass { Prop1 = "a" };
        var f3 = new FieldCompareClass { Prop1 = "b" };

        Assert.True(DeepComparison.AreSame(f1, f2));
        Assert.False(DeepComparison.AreSame(f1, f3));
    }

    [Fact]
    public void AssertSimpleObjectDifferent()
    {
        AssertEx.Throws<Exception>("{Item} (IComparable Differs)\r\nExpected: 1\r\nActual:   2", () => DeepComparison.AreSame(1, 2, true));
    }
    [Fact]
    public void PropCompare()
    {
        var f2 = new SimpleCompareClass { Prop1 = "a" };
        var f3 = new SimpleCompareClass { Prop1 = "b" };
        AssertEx.Throws<Exception>("{Item}.Prop1 (IComparable Differs)\r\nExpected: a\r\nActual:   b", () => DeepComparison.AreSame(f2,f3, true));
    }
    [Fact]
    public void ArrayPropCompare()
    {
        var f2 = new []{new SimpleCompareClass { Prop1 = "a" }};
        var f3 = new[] {new SimpleCompareClass { Prop1 = "b" }};
        AssertEx.Throws<Exception>("{Item}[0].Prop1 (IComparable Differs)\r\nExpected: a\r\nActual:   b", () => DeepComparison.AreSame(f2,f3, true));
    }
    [Fact]
    public void FieldCompare()
    {
        var f2 = new SimpleCompareClass { Field1 = "a" };
        var f3 = new SimpleCompareClass { Field1 = "b" };
        AssertEx.Throws<Exception>("{Item}.Field1 (IComparable Differs)\r\nExpected: a\r\nActual:   b", () => DeepComparison.AreSame(f2,f3, true));
    }

    [Fact]
    public void EnumLengthDiffers()
    {
        AssertEx.Throws<Exception>("{Item} (Lengths Unequal)\r\nExpected: Int32[] [1]\r\nActual:   Int32[] [1, 2]", ()=> DeepComparison.AreSame(new[] {1}, new[] {1,2}, true));
    }
    [Fact]
    public void CountTheEnums()
    {
        AssertEx.Throws<Exception>("{Item}[0] (IComparable Differs)\r\nExpected: 1\r\nActual:   2", ()=> DeepComparison.AreSame(new[] {1, 1}, new[] {2,1}, true));
        AssertEx.Throws<Exception>("{Item}[1] (IComparable Differs)\r\nExpected: 1\r\nActual:   2", ()=> DeepComparison.AreSame(new[] {1, 1}, new[] {1,2}, true));
    }

}