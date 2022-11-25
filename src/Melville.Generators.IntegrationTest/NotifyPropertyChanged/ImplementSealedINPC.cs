using Melville.INPC;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Generators.IntegrationTest.NotifyPropertyChanged;

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