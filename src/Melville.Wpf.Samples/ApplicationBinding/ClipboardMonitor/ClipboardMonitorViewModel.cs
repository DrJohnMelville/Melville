using System;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.Clipboards;

namespace Melville.Wpf.Samples.ApplicationBinding.ClipboardMonitor
{
    public class ClipboardMonitorViewModel: NotifyBase
    {
        private string clipboardValue = "";

        public string ClipboardValue
        {
            get => clipboardValue;
            set => AssignAndNotify(ref clipboardValue, value);
        }

        private readonly IClipboardNotification notifier;
        private readonly IReadFromClipboard reader;

        public ClipboardMonitorViewModel(IClipboardNotification notifier, IReadFromClipboard reader)
        {
            this.notifier = notifier;
            this.reader = reader;
            notifier.ClipboardUpdate += GetContents;
        }

        private void GetContents(object? sender, EventArgs e)
        {
            ClipboardValue = reader.GetText();
        }
    }
}