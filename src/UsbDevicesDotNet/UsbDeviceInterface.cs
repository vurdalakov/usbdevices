namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Runtime.InteropServices;

    internal class UsbDeviceInterface : UsbDeviceBase
    {
        public static UsbDevice GetUsbDevice(IntPtr handle, UsbDeviceWinApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData)
        {
            UsbDeviceInterface usbDeviceInterface = new UsbDeviceInterface(handle, deviceInterfaceData);
            return usbDeviceInterface.UsbDevice;
        }

        private IntPtr handle;
        private UsbDeviceWinApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData;

        private UsbDeviceWinApi.SP_DEVINFO_DATA devInfoData;

        public UsbDevice UsbDevice { get; private set; }

        private UsbDeviceInterface(IntPtr handle, UsbDeviceWinApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData)
        {
            this.handle = handle;
            this.deviceInterfaceData = deviceInterfaceData;

            this.UsbDevice = new UsbDevice();

            if (!this.GetDeviceInterfaceDetail())
            {
                return;
            }

            this.GetDeviceId();

            this.UsbDevice.Vid = this.ExtractStringAfterPrefix(this.UsbDevice.DeviceId, "VID_", 4).ToUpper();
            this.UsbDevice.Pid = this.ExtractStringAfterPrefix(this.UsbDevice.DeviceId, "PID_", 4).ToUpper();

            this.UsbDevice.BusReportedDeviceDescription = this.ReadBusReportedDeviceDescription(); // TODO: unify
            this.GetHubAndPort(); // TODO: unify
        }

        private Boolean GetDeviceInterfaceDetail()
        {
            UsbDeviceWinApi.SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData = new UsbDeviceWinApi.SP_DEVICE_INTERFACE_DETAIL_DATA();
            deviceInterfaceDetailData.Size = (UInt32)(8 == IntPtr.Size ? 8 : 6);

            UInt32 requiredSize = 0;

            this.devInfoData = new UsbDeviceWinApi.SP_DEVINFO_DATA();
            this.devInfoData.Size = (UInt32)Marshal.SizeOf(devInfoData);

            Boolean success = UsbDeviceWinApi.SetupDiGetDeviceInterfaceDetail(this.handle, ref this.deviceInterfaceData,
                ref deviceInterfaceDetailData, 512, out requiredSize, ref this.devInfoData);

            if (success)
            {
                this.UsbDevice.DevicePath = deviceInterfaceDetailData.DevicePath;
            }
            else
            {
                this.TraceError("SetupDiGetDeviceInterfaceDetail");
            }

            return success;
        }

        private void GetDeviceId()
        {
            Int32 bufferSize = 512;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

            Int32 errorCode = UsbDeviceWinApi.CM_Get_Device_ID(this.devInfoData.DevInst, buffer, bufferSize, 0);

            if (UsbDeviceWinApi.ERROR_SUCCESS == errorCode)
            {
                this.UsbDevice.DeviceId = Marshal.PtrToStringAuto(buffer);

                int slash = this.UsbDevice.DeviceId.LastIndexOf('\\');
                if ((slash > 0) && (this.UsbDevice.DeviceId.LastIndexOf('-') > slash))
                {
                    UInt32 devInstParent;
                    errorCode = UsbDeviceWinApi.CM_Get_Parent(out devInstParent, this.devInfoData.DevInst, 0);

                    if (UsbDeviceWinApi.ERROR_SUCCESS == errorCode)
                    {
                        errorCode = UsbDeviceWinApi.CM_Get_Device_ID(devInstParent, buffer, bufferSize, 0);

                        if (UsbDeviceWinApi.ERROR_SUCCESS == errorCode)
                        {
                            this.UsbDevice.DeviceId = Marshal.PtrToStringAuto(buffer);
                        }
                        else
                        {
                            this.TraceError("CM_Get_Device_ID", errorCode);
                        }
                    }
                    else
                    {
                        this.TraceError("CM_Get_Parent", errorCode);
                    }
                }
            }
            else
            {
                this.TraceError("CM_Get_Device_ID", errorCode);
            }

            Marshal.FreeHGlobal(buffer);
        }

        private void GetHubAndPort()
        {
            UInt32 regtype;
            UInt32 bufferSize = 512;
            IntPtr buffer = Marshal.AllocHGlobal((Int32)bufferSize);
            UInt32 requiredSize;

            Boolean success = UsbDeviceWinApi.SetupDiGetDeviceRegistryProperty(this.handle, ref this.devInfoData,
                UsbDeviceWinApi.SpDrpLocationInformation, out regtype, buffer, bufferSize, out requiredSize);

            if (success)
            {
                String value = Marshal.PtrToStringAuto(buffer);

                if (value != null)
                {
                    this.UsbDevice.Hub = ExtractStringAfterPrefix(value, "Hub_#", 4);
                    this.UsbDevice.Port = ExtractStringAfterPrefix(value, "Port_#", 4);
                }
            }
            else
            {
                this.TraceError("SetupDiGetDeviceRegistryProperty");
            }

            Marshal.FreeHGlobal(buffer);
        }

        private String ReadBusReportedDeviceDescription()
        {
            UsbDeviceWinApi.DEVPROPKEY devpropkey = new UsbDeviceWinApi.DEVPROPKEY();
            devpropkey.Fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2);
            devpropkey.Pid = 4;

            UInt32 proptype;
            
            UInt32 bufferSize = 512;
            IntPtr buffer = Marshal.AllocHGlobal((Int32)bufferSize);

            UInt32 outsize;

            Boolean success = UsbDeviceWinApi.SetupDiGetDevicePropertyW(this.handle, ref this.devInfoData,
                ref devpropkey, out proptype, buffer, bufferSize, out outsize, 0);

            if (!success)
            {
                this.TraceError("SetupDiGetDevicePropertyW");
            }

            String value = success ? Marshal.PtrToStringAuto(buffer) : null;

            Marshal.FreeHGlobal(buffer);

            return value;
        }

        private String ExtractStringAfterPrefix(String text, String prefix, Int32 length)
        {
            Int32 index = text.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
            return index >= 0 ? text.Substring(index + prefix.Length, length) : "";
        }
    }
}
