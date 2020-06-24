using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.MVVM.USB
{
  public abstract class UsbDevice: IDisposable
  {
    private Stream? deviceStream;
    private int readBufferLength;
    private readonly string deviceId;

    protected abstract void DeviceInputEvent(byte[] data);
    protected UsbDevice(string deviceId)
    {
      this.deviceId = deviceId;
    }

    public void TryConnect()
    {
      if (IsConnected) return;
      var devices = EnumDevices.Enum().ToArray();
      var devicePath = devices.FirstOrDefault(i => i.Contains(deviceId));
      if (devicePath == null) return;
      var fileHandle = NativeMethods.CreateFile(devicePath,
        NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE,
        NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE, 
        IntPtr.Zero, NativeMethods.OPEN_EXISTING, NativeMethods.FILE_FLAG_OVERLAPPED, IntPtr.Zero);
      if (fileHandle == NativeMethods.InvalidHandleValue)
      {
        return;
      }
      IntPtr deviceData;
      if (!NativeMethods.HidD_GetPreparsedData(fileHandle, out deviceData)) return;
      NativeMethods.HidCaps caps;
      NativeMethods.HidP_GetCaps(deviceData, out caps);
      readBufferLength = caps.InputReportByteLength;
      deviceStream = new FileStream(fileHandle, FileAccess.ReadWrite, readBufferLength,true);
      ReadDevice();
    }
    private bool IsConnected => deviceStream != null;

    private async void ReadDevice()
    {
      if (deviceStream == null) throw new InvalidOperationException("USB Device not opened.");
      try
      {
        var buf = new byte[readBufferLength];
        while (true)
        {
          await deviceStream.ReadAsync(buf, 0, readBufferLength);
          DeviceInputEvent(buf);
        }
      }
      catch (TaskCanceledException tce)
      {
        throw new IOException("Task cancelled exception here usually means that the" +
                              "USB device got disposed prematurely", tce);
      }
      catch (IOException e)
      {
        if (e.HResult != -2147023729) // device not connected
        {
          throw;
        }
        deviceStream.Close();
        deviceStream = null;
        TryConnect();
      }
    }
    public void Dispose() => Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && deviceStream != null)
      {
        deviceStream.Close();
      }
    }
  }
}