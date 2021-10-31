using System;
using System.Runtime.InteropServices;
using Melville.SystemInterface.WindowMessages;

namespace Melville.SystemInterface.USB.ThumbDrives;

public class ThumbDriveEventArgs : EventArgs
{
    public string RootPath { get; }

    public ThumbDriveEventArgs(string rootPath)
    {
        RootPath = rootPath;
    }
}

public interface IDetectThumbDrive
{
    event EventHandler<ThumbDriveEventArgs>? Attached;
    event EventHandler<ThumbDriveEventArgs>? Removed;
}
public sealed class DetectThumbDrive: IDetectThumbDrive, IDisposable
{
    public event EventHandler<ThumbDriveEventArgs>? Attached;
    public event EventHandler<ThumbDriveEventArgs>? Removed;
    private readonly ISingleWindowEventHolder messageSource;
        
    private const int WmDeviceChanged = 537;
    private const int VolumeArrived = 0x8000;
    private const int VolumeLeft = 0x8004;

    public DetectThumbDrive(IWindowMessageSource source)
    {
        messageSource = source.RegisterForMessage(WmDeviceChanged);
        messageSource.MessageReceived += DeviceStatusChanged;
    }

    public void Dispose()
    {
        messageSource.MessageReceived -= DeviceStatusChanged;
    }

    private void DeviceStatusChanged(object? sender, WindowMessageEventArgs e)
    {
        // may eventually be able to detect "USB card in reader
        //http://stackoverflow.com/questions/14607564/detect-sd-card-insertion-from-a-windows-service

        switch ((int) e.WParam)
        {
            case VolumeArrived:
                NotifyVolumeChange(e.LParam, Attached);
                break;
            case VolumeLeft:
                NotifyVolumeChange(e.LParam, Removed);
                break;
        }
    }

    private void NotifyVolumeChange(in IntPtr roots, EventHandler<ThumbDriveEventArgs>? action)
    {
        if (BroadcastVolumeParser.Roots(roots) is {} root)
        {
            action?.Invoke(this, new ThumbDriveEventArgs(root));
        }
    }
}

public static class BroadcastVolumeParser
{
    public static string? Roots(IntPtr broadcastVolumePtr)
    {
        // http://stackoverflow.com/questions/6003822/how-to-detect-a-usb-drive-has-been-plugged-in

        var vol = BroadcastVolumeFromPointer(broadcastVolumePtr);
        return vol.dbcv_devicetype == 0x00000002 &&
               DriveMaskToLetter(vol.dbcv_unitmask) is {} driveLetter &&
               driveLetter != '?'
            ? $"{DriveMaskToLetter(vol.dbcv_unitmask)}:\\"
            : null;
    }

    private static DEV_BROADCAST_VOLUME BroadcastVolumeFromPointer(IntPtr broadcastVolumePtr)
    {
        DEV_BROADCAST_VOLUME vol = (DEV_BROADCAST_VOLUME) (
            Marshal.PtrToStructure(broadcastVolumePtr, typeof(DEV_BROADCAST_VOLUME)) ??
            new DEV_BROADCAST_VOLUME());
        return vol;
    }

    [StructLayout(LayoutKind.Sequential)] //Same layout in mem
    public struct DEV_BROADCAST_VOLUME
    {
        public int dbcv_size;
        public int dbcv_devicetype;
        public int dbcv_reserved;
        public int dbcv_unitmask;
    }

    private static char DriveMaskToLetter(int mask)
    {
        char letter;
        string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //1 = A, 2 = B, 3 = C
        int cnt = 0;
        int pom = mask / 2;
        while (pom != 0)    // while there is any bit set in the mask shift it right        
        {
            pom = pom / 2;
            cnt++;
        }
        letter = cnt < drives.Length ? drives[cnt] : '?';
        return letter;
    }

}