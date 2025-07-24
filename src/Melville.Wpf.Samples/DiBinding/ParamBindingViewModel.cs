using Melville.INPC;

namespace Melville.Wpf.Samples.DiBinding;

public partial class ParamBindingViewModel
{
    [AutoNotify] private string output = "Click a button to set the output";

    public void SetOutput(string input) => Output = input;
}