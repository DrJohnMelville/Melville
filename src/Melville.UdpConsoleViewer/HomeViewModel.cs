using System;
using System.Text;
using Melville.Lists;
using Melville.MVVM.Wpf.ViewFrames;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.UdpConsoleViewer;

public record DisplayLine (DateTime Time, string Text);
    
[OnDisplayed(nameof(HomeViewModel.InitiateLoop))]
public class HomeViewModel
{
    private UdpReceiver recv = new UdpReceiver(15321);
    public ThreadSafeBindableCollection<DisplayLine> List { get; } = new();
    public async void InitiateLoop()
    {
        await foreach (var line in recv.WaitForReads())
        {
            List.Add(new DisplayLine(DateTime.Now, 
                Encoding.UTF8.GetString(line.Buffer)));
        }
    }

    public void ClearList() => List.Clear();
}