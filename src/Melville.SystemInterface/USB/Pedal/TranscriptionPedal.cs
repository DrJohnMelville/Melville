using System;
using System.ComponentModel;

namespace Melville.SystemInterface.USB.Pedal;

public enum PedalButton
{
    Left = 1,
    Center = 2,
    Right = 4
}

public interface ITranscriptonPedal : INotifyPropertyChanged
{
    bool Left { get; }
    bool Center { get; }
    bool Right { get; }
    event EventHandler? LeftDown;
    event EventHandler? LeftUp;
    event EventHandler? CenterDown;
    event EventHandler? CenterUp;
    event EventHandler? RightDown;
    event EventHandler? RightUp;
}

public sealed class TranscriptonPedal : UsbDevice, ITranscriptonPedal
{
    private const string PedalId = "vid_05f3&pid_00ff";

    public TranscriptonPedal(IMonitorForDeviceArrival deviceNodificationArrival)
        : base(PedalId, deviceNodificationArrival)
    {
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private bool leftDown;
    public bool Left => leftDown;
    public event EventHandler? LeftDown;
    public event EventHandler? LeftUp;

    private bool centerDown;
    public bool Center => centerDown;
    public event EventHandler? CenterDown;
    public event EventHandler? CenterUp;

    private bool rightDown;
    public bool Right => rightDown;
    public event EventHandler? RightDown;
    public event EventHandler? RightUp;

    protected override void DeviceInputEvent(byte[] data)
    {
        DoButtonDown(ref leftDown, data[1], PedalButton.Left);
        DoButtonDown(ref centerDown, data[1], PedalButton.Center);
        DoButtonDown(ref rightDown, data[1], PedalButton.Right);
    }

    private void DoButtonDown(ref bool oldValue, byte pedalOutput, PedalButton button)
    {
        bool value = (pedalOutput & (byte) button) != 0;
        if (oldValue == value) return;
        oldValue = value;
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(Enum.GetName(typeof(PedalButton), button) ?? ""));
        PickEvent(button, value)?.Invoke(this, EventArgs.Empty);
    }

    private EventHandler? PickEvent(PedalButton button, in bool value) =>
        (button, value) switch
        {
            (PedalButton.Left, false) => LeftUp,
            (PedalButton.Left, true) => LeftDown,
            (PedalButton.Center, false) => CenterUp,
            (PedalButton.Center, true) => CenterDown,
            (PedalButton.Right, false) => RightUp,
            (PedalButton.Right, true) => RightDown,
            _ => null
        };
}