using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Melville.SystemInterface.WindowMessages;
using Microsoft.Win32.SafeHandles;

namespace Melville.SystemInterface.USB;

public abstract class UsbDevice: IDisposable
{
  private Stream? deviceStream;
  private int readBufferLength;
  private readonly string deviceId;
  private readonly IMonitorForDeviceArrival newDeviceNodification;

  protected abstract void DeviceInputEvent(byte[] data);
  protected UsbDevice(string deviceId, IMonitorForDeviceArrival newDeviceNodification)
  {
    this.deviceId = deviceId;
    this.newDeviceNodification = newDeviceNodification;
    newDeviceNodification.DeviceArrived += TryConnect;
    TryConnect();
  }

  private void TryConnect(object? sender, WindowMessageEventArgs e) => TryConnect();

  public void TryConnect()
  {
    if (IsConnected) return;
      
    var devicePath = EnumDevices.Enum().FirstOrDefault(i => i.Contains(deviceId));
    if (devicePath == null) return;
      
    var fileHandle = OpenDeviceAsFile(devicePath);
    if (fileHandle == NativeMethods.InvalidHandleValue) return;
      
    if (!TryGetReadBufferLength(fileHandle)) return;
    deviceStream = new FileStream(fileHandle, FileAccess.ReadWrite, readBufferLength,true);
    ReadDevice();
  }

  private bool TryGetReadBufferLength(SafeFileHandle fileHandle)
  {
    if (!NativeMethods.HidD_GetPreparsedData(fileHandle, out var deviceData)) return false;
    NativeMethods.HidP_GetCaps(deviceData, out var caps);
    readBufferLength = caps.InputReportByteLength;
    return true;
  }

  private static SafeFileHandle OpenDeviceAsFile(string devicePath)
  {
    var fileHandle = NativeMethods.CreateFile(devicePath,
      NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE,
      NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE,
      IntPtr.Zero, NativeMethods.OPEN_EXISTING, NativeMethods.FILE_FLAG_OVERLAPPED, IntPtr.Zero);
    return fileHandle;
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
      newDeviceNodification.DeviceArrived -= TryConnect;
    }
  }
}