using Melville.TestHelpers.InpcTesting;
using Xunit;
using Melville.INPC;

namespace Melville.Generators.INPC.Test.IntegrationTests;

public sealed partial class ImplementSealedINPC
{
    [AutoNotify] private int x;

    [Fact]
    public void ThisShouldCompileWithoutWarnings()
    {
        using var _ = INPCCounter.VerifyInpcFired(this, i => i.X);
        X = 20;
    }

}