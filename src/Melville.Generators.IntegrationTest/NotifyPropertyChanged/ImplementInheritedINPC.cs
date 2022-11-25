using Melville.INPC;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Generators.IntegrationTest.NotifyPropertyChanged;

public partial class HasNotifyMethod
{
    protected string lastProp = "";
    protected void OnPropertyChanged(string property) => lastProp = property;

    [AutoNotify] private int x;

    [Fact]
    public void TestName()
    {
        Assert.Equal("", lastProp);
        X = 10;
        Assert.Equal("X", lastProp);
    }

}

public partial class ParentHasNotifyMethod : HasNotifyMethod
{
    [AutoNotify] private string y = "sss";
    [Fact]
    public void YProp()
    {
        Assert.Equal("", lastProp);
        Y = "";
        Assert.Equal("Y", lastProp);
    }

}
public partial class ParentINPC
{
    [AutoNotify] private int x;
}
public partial class ImplementInheritedINPC: ParentINPC
{
    [AutoNotify] private int y;
    
    [Fact]
    public void BothVarsNotify()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X, i => i.Y);
        X = 10;
        Y = 11;
    }
}