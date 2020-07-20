using Melville.MVVM.BusinessObjects;
using Melville.MVVM.USB.Joysticks;

namespace Melville.Wpf.Samples.TranscriptionPedal
{
    public class JoystickViewModel: NotifyBase
    {
        public IJoystick Stick { get; }

        public JoystickViewModel(IJoystick stick)
        {
            Stick = stick;
            Stick.StickChanged += (s, e) => OnPropertyChanged(nameof(Stick));
        }
    }
}