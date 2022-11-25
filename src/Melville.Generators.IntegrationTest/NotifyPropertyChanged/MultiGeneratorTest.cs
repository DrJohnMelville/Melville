using Melville.INPC;

namespace Melville.Generators.IntegrationTest.NotifyPropertyChanged;

public partial class MultiGeneratorTest: IExternalNotifyPropertyChanged
{
    [FromConstructor][DelegateTo] private IExternalNotifyPropertyChanged value;
}