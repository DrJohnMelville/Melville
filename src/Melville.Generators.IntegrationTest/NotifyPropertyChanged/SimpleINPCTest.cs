using Melville.INPC;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Generators.IntegrationTest.NotifyPropertyChanged;

public partial class SimpleINPCTest
{
    private partial class SimpleObject
    {
        [AutoNotify] private int x;
    }

    [Fact]
    public void TestName()
    {
        var obj = new SimpleObject();
        using var counter = INPCCounter.VerifyInpcFired(obj, i => i.X);
        obj.X = 10;
        Assert.Equal(10, obj.X);
    }

    private partial class HasChangeMethod
    {
        [AutoNotify] private string notifyChanged = "";
        [AutoNotify] private int x;

        void OnXChanged(int oldValue, int newValue)
        {
            NotifyChanged = $"{oldValue} -> {newValue}";
        }
    }
    [Fact]
    public void TestOnChangedMethod()
    {
        var obj = new HasChangeMethod();
        using var counter = INPCCounter.VerifyInpcFired(obj, i=>i.NotifyChanged, i => i.X);
        obj.X = 10;
        Assert.Equal(10, obj.X);
        Assert.Equal((string?)"0 -> 10", (string?)obj.NotifyChanged);
            
    }
}