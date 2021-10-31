using System.Threading.Tasks;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RunOnWindowThreads;

namespace Melville.Wpf.Samples.DialogBox;

public partial class DialogBoxSheetViewModel
{
    [AutoNotify] private string result = "No Dialog Triggered";

    public void LaunchDialog([FromServices] IMvvmDialog dlg, [FromServices]IRunOnWindowThread thread) =>
        Task.Run(() =>
            Result = thread.Run(()=> 
                    dlg.ShowModalDialog(new DialogViewModel(), 300, 150, "This is a DialogBox")) switch
                {
                    null => "Result is Null",
                    var x => $"Result is {x}"
                });
}

public class DialogViewModel
{
        
}