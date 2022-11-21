using Melville.INPC;
using Moq;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;

public interface ITestInt
{
    void DoTest();
}


public partial class ActualDelegateToTest:ITestInt
{
    private readonly Mock<ITestInt> intMock = new();

    [DelegateTo(true)]
    private ITestInt Mock() => intMock.Object;

    [Fact]
    public void CallMethod()
    {
        intMock.Verify(i=>i.DoTest(), Times.Never);
        ((ITestInt)this).DoTest();
        intMock.Verify(i=>i.DoTest(), Times.Once);
    }
}