namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class UsbDevices : UsbDeviceBase, IDisposable
    {
        private Guid classGuid;
        private IntPtr handle;

        public UsbDevices(Guid classGuid)
        {
            this.classGuid = classGuid;
            this.handle = UsbDeviceWinApi.SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero,
                UsbDeviceWinApi.SetupDiGetClassDevsFlags.DIGCF_PRESENT | UsbDeviceWinApi.SetupDiGetClassDevsFlags.DIGCF_DEVICEINTERFACE);

            if (this.IsInvalid())
            {
                this.TraceError("SetupDiGetClassDevs");
            }
        }

        public UsbDevice[] GetDevices()
        {
            UInt32 index = 0;

            List<UsbDevice> devices = new List<UsbDevice>();

            while (true)
            {
                UsbDeviceWinApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new UsbDeviceWinApi.SP_DEVICE_INTERFACE_DATA();
                deviceInterfaceData.Size = (UInt32)Marshal.SizeOf(deviceInterfaceData);

                Boolean success = UsbDeviceWinApi.SetupDiEnumDeviceInterfaces(this.handle, IntPtr.Zero, ref this.classGuid, index, ref deviceInterfaceData);

                if (!success)
                {
                    if (UsbDeviceWinApi.ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                    {
                        break;
                    }

                    this.TraceError("SetupDiEnumDeviceInterfaces");
                }

                index++;

                devices.Add(UsbDeviceInterfaceData.GetUsbDevice(this.handle, deviceInterfaceData));
            }

            return devices.ToArray();
        }

        private Boolean IsInvalid()
        {
            return UsbDeviceWinApi.InvalidHandleValue == this.handle;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsInvalid())
            {
                UsbDeviceWinApi.SetupDiDestroyDeviceInfoList(this.handle);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
