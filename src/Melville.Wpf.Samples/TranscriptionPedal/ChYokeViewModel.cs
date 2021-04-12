using Melville.MVVM.BusinessObjects;
using Melville.SystemInterface.USB.Joysticks;

namespace Melville.Wpf.Samples.TranscriptionPedal
{
    public class ChYokeViewModel: NotifyBase
    {
        public IChYoke Yoke{ get; }

        public ChYokeViewModel(IChYoke yoke)
        {
            Yoke = yoke;
            Yoke.StateChanged += (s, e) => OnPropertyChanged(nameof(Yoke));
        }
    }
}