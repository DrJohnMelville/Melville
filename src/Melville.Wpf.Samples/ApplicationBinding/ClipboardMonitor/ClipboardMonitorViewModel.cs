using System;
using Melville.INPC;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.Clipboards;

namespace Melville.Wpf.Samples.ApplicationBinding.ClipboardMonitor
{
    public partial class ClipboardMonitorViewModel
    {
        [AutoNotify] private string clipboardValue = "";
        private readonly IReadFromClipboard reader;

        public ClipboardMonitorViewModel(IClipboardNotification notifier, IReadFromClipboard reader)
        {
            this.reader = reader;
            notifier.ClipboardUpdate += GetContents;
        }

        private void GetContents(object? sender, EventArgs e)
        {
            ClipboardValue = reader.GetText();
        }
    }
}