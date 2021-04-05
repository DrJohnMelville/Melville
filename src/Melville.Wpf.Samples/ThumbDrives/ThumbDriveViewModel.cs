using Melville.INPC;
using Melville.MVVM.USB.ThumbDrives;

namespace Melville.Wpf.Samples.ThumbDrives
{
    public partial class ThumbDriveViewModel
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

        [AutoNotify]private string events = "";
    }
}