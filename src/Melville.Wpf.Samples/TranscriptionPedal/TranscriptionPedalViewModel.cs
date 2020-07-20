using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.USB.Pedal;
using Melville.MVVM.Wpf.ViewFrames;
using Melville.WpfControls.Bindings;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

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

    public class TranscriptionPedalViewModel: NotifyBase
    {
        public ITranscriptonPedal Pedal { get; }
        private string leftEvents ="";
        public string LeftEvents
        {
            get => leftEvents;
            set => AssignAndNotify(ref leftEvents, value);
        }

        private string centerEvents = "";
        public string CenterEvents
        {
            get => centerEvents;
            set => AssignAndNotify(ref centerEvents, value);
        }

        private string rightEvents = "";
        public string RightEvents
        {
            get => rightEvents;
            set => AssignAndNotify(ref rightEvents, value);
        }

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