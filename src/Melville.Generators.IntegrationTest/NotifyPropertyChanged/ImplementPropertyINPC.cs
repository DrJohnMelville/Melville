using System.ComponentModel;
using Melville.INPC;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Generators.IntegrationTest.NotifyPropertyChanged;

public partial class ImplementPropertyInpc
{
    [AutoNotify] private int x;
    [AutoNotify] public int X1 => X;

    [Fact]
    public void DoPropertyTest()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X, i => i.X1);
        X = 22;
    }
}
public partial class ImplementPropertyInpcWithThis
{
    [AutoNotify] private int x;
    [AutoNotify] public int X1 => this.X;

    [Fact]
    public void DoPropertyTest()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X, i => i.X1);
        X = 22;
    }
}
public partial class DoNotFireForPropertyOnOtherObject
{
    [AutoNotify] private int x;
    private DoNotFireForPropertyOnOtherObject? other;
    [AutoNotify] public int X1 => other?.X ?? 1;

    [Fact]
    public void DoPropertyTest()
    {
        other = new DoNotFireForPropertyOnOtherObject();// fixes a warning
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X); // does not notify X1
        X = 22;
    }
}
public partial class ImplementProperytBodyInpc
{
    [AutoNotify] private int x;
    [AutoNotify] public int X1
    {
        get { return X; }
    }

    [Fact]
    public void DoPropertyTest()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X, i => i.X1);
        X = 22;
    }
}
public partial class ImplementProperytBodyWithArrowFunc
{
    [AutoNotify] private int x;
    [AutoNotify] public int X1
    {
        get =>X; 
    }

    [Fact]
    public void DoPropertyTest()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X, i => i.X1);
        X = 22;
    }
}
public partial class IgnoreSetBodyInpc
{
    [AutoNotify] private int x;
    [AutoNotify] private int y;
    [AutoNotify] public int X1
    {
        get { return X; }
        set { x = Y + value; }
    }

    [Fact]
    public void DoPropertyTest()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X, i => i.X1); // does not notify Y
        X = 22;
    }
}

public interface ICustomImplementationInterface : INotifyPropertyChanged
{
    void OnPropertyChanged(string propName);
}

public partial class CustomImplementation : ICustomImplementationInterface
{
    [AutoNotify] private int x;
    [Fact]
    public void DoPropertyTest()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X);
        X = 22;
    }

    [Fact]
    public void NotUsingBuiltInInerface()
    {
        Assert.False(this is Melville.INPC.IExternalNotifyPropertyChanged);
    }
}
public partial class DoNotOverrideCustomImplementation : ICustomImplementationInterface
{
    private int notifications = 0;
    [AutoNotify] private int x;
    [Fact]
    public void DoPropertyTest()
    {
        Assert.Equal(0, notifications);
        using var _ = INPCCounter.VerifyInpcFired(this);
        X = 22;
        Assert.Equal(1, notifications);
    }
        
    public event PropertyChangedEventHandler? PropertyChanged;
    void ICustomImplementationInterface.OnPropertyChanged(string propName)
    {
        notifications++;
    }

    private void PreventUnUsedEventCompilerWarning() => 
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(""));
}