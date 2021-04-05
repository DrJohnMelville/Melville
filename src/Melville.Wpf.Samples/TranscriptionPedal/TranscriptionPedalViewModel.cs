using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Melville.INPC;
using Melville.MVVM.USB.Pedal;
using Melville.MVVM.Wpf.Bindings;
using Melville.MVVM.Wpf.ViewFrames;

namespace Melville.Wpf.Samples.TranscriptionPedal
{
    public class JoystickTranscriptionPedalViewModel: TranscriptionPedalViewModel, ICreateView
    {
        public JoystickTranscriptionPedalViewModel(JoystickPedal pedal) : base(pedal)
        {
        }

        public UIElement View()
        {
            return new TranscriptionPedalView();
        }
    }

    public partial class TranscriptionPedalViewModel
    {
        public ITranscriptonPedal Pedal { get; }
        [AutoNotify] private string leftEvents ="";
        
        [AutoNotify] private string centerEvents = "";
        
        [AutoNotify] private string rightEvents = "";

        public TranscriptionPedalViewModel(ITranscriptonPedal pedal)
        {
            Pedal = pedal;
            pedal.LeftDown += (s,e)=>LeftEvents += "Down\r\n";
            pedal.LeftUp += (s,e)=>LeftEvents += "Up\r\n";
            pedal.CenterDown += (s,e)=>CenterEvents += "Down\r\n";
            pedal.CenterUp += (s,e)=>CenterEvents += "Up\r\n";
            pedal.RightDown += (s,e)=>RightEvents += "Down\r\n";
            pedal.RightUp += (s,e)=>RightEvents += "Up\r\n";
        }

        public static readonly IValueConverter FillConverter = LambdaConverter.Create(
            (bool down) => down ? Brushes.Green:Brushes.Red);
    }
} 