using Melville.INPC;
using Melville.MVVM.Maui.Commands;

namespace Melville.Mvvm.Maui.Test.Commands;

public partial class CommandVisibilityTest
{
    private class ConcreteCommandBase : CommandBase
    {
        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }

    private readonly CommandBase sut = new ConcreteCommandBase();
    private int changeFiredCount = 0;

    public CommandVisibilityTest()
    {
        sut.CanExecuteChanged += (_, _) => changeFiredCount++;
    }
    [Fact]
    public void CommandIsEnabledByDefault()
    {
        sut.IsEnabled.Should().BeTrue();
        changeFiredCount.Should().Be(0);
    }

    [Fact]
    public void SetCommandEnabled()
    {
        sut.IsEnabled = false;
        sut.IsEnabled.Should().BeFalse();
        changeFiredCount.Should().Be(1);
    }

    private partial class Container
    {
        [AutoNotify] private int intA;
        [AutoNotify] private int intB;
        [AutoNotify] private string stringA;
        [AutoNotify] private int intAQueryCount;

        [AutoNotify]
        public int QueryCountedIntA
        {
            get
            {
                intAQueryCount++;
                return IntA;
            }
        }
    }

    [Fact]
    public void SetCommandDisabled()
    {
        var container = new Container();
        sut.MapEnabledTo(container, i => i.QueryCountedIntA > 9);
        
        sut.IsEnabled.Should().BeFalse();
        changeFiredCount.Should().Be(1);
        container.IntAQueryCount.Should().Be(1);

        container.IntA = 10;
        sut.IsEnabled.Should().BeTrue();
        changeFiredCount.Should().Be(2);
        container.IntAQueryCount.Should().Be(2);

        container.IntB = 10;
        sut.IsEnabled.Should().BeTrue();
        changeFiredCount.Should().Be(2);
        container.IntAQueryCount.Should().Be(2);
    }
}