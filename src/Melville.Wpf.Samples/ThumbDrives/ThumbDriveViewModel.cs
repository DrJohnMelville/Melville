using Melville.MVVM.BusinessObjects;
using Melville.MVVM.USB.ThumbDrives;

namespace Melville.Wpf.Samples.ThumbDrives
{
    public class ThumbDriveViewModel: NotifyBase
    {
        public ThumbDriveViewModel(IDetectThumbDrive driveNotifier)
        {
            driveNotifier.Attached += (s, e) => Notify(e.RootPath, "Attached");
            driveNotifier.Removed += (s, e) => Notify(e.RootPath, "Removed");
        }

        private void Notify(string root, string which)
        {
            Events += $"\r\n {which} -- {root}";
        }

        private string events = "";
        public string Events
        {
            get => events;
            set => AssignAndNotify(ref events, value);
        }
    }
}