using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Melville.SystemInterface.USB
{
    public static class EnumDevices
    {
        public static IEnumerable<string> Enum()
        {
            var hIDGuid = NativeMethods.HIDGuid;
            IntPtr desiredDevices = NativeMethods.SetupDiGetClassDevs(ref hIDGuid, null, IntPtr.Zero,
                NativeMethods.DIGCF_DEVICEINTERFACE | NativeMethods.DIGCF_PRESENT);
            try
            {
                var deviceData = new NativeMethods.DeviceInterfaceData();
                deviceData.Size = Marshal.SizeOf(deviceData);

                for (uint i = 0; true; i++)
                {
                    if (!NativeMethods.SetupDiEnumDeviceInterfaces(desiredDevices, IntPtr.Zero, ref hIDGuid, i, ref deviceData))
                    {
                        Marshal.GetLastWin32Error();
                        yield break;
                    }
                    var path = GetDevicePath(desiredDevices, ref deviceData);
                    if (path != null) yield return path;
                }
            }
            finally
            {
                NativeMethods.SetupDiDestroyDeviceInfoList(desiredDevices);
            }
        }
        private static string? GetDevicePath(IntPtr hInfoSet, ref NativeMethods.DeviceInterfaceData oInterface)
        {
            uint nRequiredSize = 0;
            // Get the device interface details
            if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize, IntPtr.Zero))
            {
                var oDetail = new NativeMethods.DeviceInterfaceDetailData();
                oDetail.Size = Marshal.SizeOf(typeof (IntPtr)) == 8 ? 8 : 5;
                if (NativeMethods.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, ref oDetail, nRequiredSize, 
                    ref nRequiredSize, IntPtr.Zero))
                {
                    return oDetail.DevicePath;
                }
            }
            return null;
        }

    }
}