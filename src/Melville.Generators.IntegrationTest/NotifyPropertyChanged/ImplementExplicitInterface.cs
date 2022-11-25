using Melville.INPC;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Generators.IntegrationTest.NotifyPropertyChanged;

public partial class ImplementExplicitInterface : IExternalNotifyPropertyChanged
{
    [AutoNotify] private int x;

    [Fact]
    public void INPCWorks()
    {
        using var counter = INPCCounter.VerifyInpcFired(this, i => i.X);
        X = 10;
        Assert.Equal(10, X);
    }

    [Fact]
    public void ExternalNotifyImplemented()
    {
        using var counter = new INPCCounter(this, new []{"foo"});
        ((IExternalNotifyPropertyChanged)this).OnPropertyChanged("foo");
    }


}
[AutoNotify]
public partial class ImplementExplicitInterfaceWithoutProp : IExternalNotifyPropertyChanged
{
    [Fact]
    public void ExternalNotifyImplemented()
    {
        using var counter = new INPCCounter(this, new []{"foo"});
        ((IExternalNotifyPropertyChanged)this).OnPropertyChanged("foo");
    }
    
    
}