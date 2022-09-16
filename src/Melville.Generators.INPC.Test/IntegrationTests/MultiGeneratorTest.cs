using Melville.INPC;

namespace Melville.Generators.INPC.Test.IntegrationTests;

public partial class MultiGeneratorTest: IExternalNotifyPropertyChanged
{
    [FromConstructor][DelegateTo] private IExternalNotifyPropertyChanged value;
}