using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.IntegrationTest.NotifyPropertyChanged;

public partial class GenericTest
{
    [AutoNotify] private List<int> numbers = new List<int>();
    
    [Fact]
    public void PropExists()
    {
        Assert.NotNull(Numbers);
    }

    [AutoNotify] private IDictionary<Lazy<OuterClass.InnerClass?>,
        Task<IComparable<CodeWriterTests>>>? complexProp;

    [Fact]
    public void TestName()
    {
        Assert.Null(ComplexProp);
        Assert.Equal(ComplexProp, complexProp); // will always succeed, but if it compiles, they
        // the two items are the same static type

    }

    [AutoNotify] public partial int Prop { get; set; }

    [Fact]
    public void PartialPropertyTest()
    {
        var calls = 0;
        PropertyChanged += (s, e) =>
        {
            Assert.Equal("Prop", e.PropertyName);
            calls++;
        };

        Assert.Equal(0, calls);
        Assert.Equal(0, Prop);
        Prop = 15;
        Assert.Equal(1, calls);
        Assert.Equal(15, Prop);
    }
}

public class OuterClass
{
    public class InnerClass
    {
            
    }
}