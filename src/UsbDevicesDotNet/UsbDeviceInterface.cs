namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

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

            this.UsbDevice.DeviceId = this.GetDeviceId();
            this.UsbDevice.InterfaceIds = this.GetInterfaceIds(this.devInfoData.DevInst, this.UsbDevice.DeviceId);

            this.UsbDevice.Vid = this.ExtractStringAfterPrefix(this.UsbDevice.DeviceId, "VID_", 4).ToUpper();
            this.UsbDevice.Pid = this.ExtractStringAfterPrefix(this.UsbDevice.DeviceId, "PID_", 4).ToUpper();

            this.GetProperties();
            this.UsbDevice.BusReportedDeviceDescription = this.ReadBusReportedDeviceDescription();

            this.GetRegistryProperties();
            this.GetHubAndPort();
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

        private String GetDeviceId()
        {
            String deviceId = null;

            Int32 bufferSize = 512;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

            Int32 errorCode = UsbDeviceWinApi.CM_Get_Device_ID(this.devInfoData.DevInst, buffer, bufferSize, 0);

            if (UsbDeviceWinApi.ERROR_SUCCESS == errorCode)
            {
                deviceId = Marshal.PtrToStringAuto(buffer);

                int slash = deviceId.LastIndexOf('\\');
                if ((slash > 0) && (deviceId.LastIndexOf('-') > slash))
                {
                    UInt32 devInstParent;
                    errorCode = UsbDeviceWinApi.CM_Get_Parent(out devInstParent, this.devInfoData.DevInst, 0);

                    if (UsbDeviceWinApi.ERROR_SUCCESS == errorCode)
                    {
                        errorCode = UsbDeviceWinApi.CM_Get_Device_ID(devInstParent, buffer, bufferSize, 0);

                        if (UsbDeviceWinApi.ERROR_SUCCESS == errorCode)
                        {
                            deviceId = Marshal.PtrToStringAuto(buffer);
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

            return deviceId;
        }

        private String[] GetInterfaceIds(UInt32 devInst, String deviceId)
        {
            List<String> ids = new List<String>();             

            UInt32 devInstChild;
            Int32 errorCode = UsbDeviceWinApi.CM_Get_Child(out devInstChild, devInst, 0);
            if (UsbDeviceWinApi.CR_SUCCESS != errorCode)
            {
                this.TraceError("CM_Get_Child", errorCode);

                ids.Add(deviceId);
                return ids.ToArray();
            }

            String interfaceId = this.GetDeviceId(devInstChild);

            if (!String.IsNullOrEmpty(interfaceId))
            {
                ids.Add(interfaceId);

                UInt32 devInstSibling = devInstChild;
                while (true)
                {
                    errorCode = UsbDeviceWinApi.CM_Get_Sibling(out devInstSibling, devInstSibling, 0);
                    if (UsbDeviceWinApi.CR_SUCCESS != errorCode)
                    {
                        this.TraceError("CM_Get_Sibling", errorCode);
                        break;
                    }

                    interfaceId = this.GetDeviceId(devInstSibling);

                    if (!String.IsNullOrEmpty(interfaceId))
                    {
                        ids.Add(interfaceId);
                    }
                }
            }

            return ids.ToArray();
        }

        private String GetDeviceId(UInt32 devInst)
        {
            Int32 bufferSize = 1024;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

            String deviceId = null;

            Int32 errorCode = UsbDeviceWinApi.CM_Get_Device_ID(devInst, buffer, bufferSize, 0);
            if (UsbDeviceWinApi.CR_SUCCESS == errorCode)
            {
                deviceId = Marshal.PtrToStringAuto(buffer);
            }
            else
            {
                this.TraceError("CM_Get_Device_ID", errorCode);
            }

            Marshal.FreeHGlobal(buffer);

            return deviceId;
        }

        private void GetProperties()
        {
            UInt32 propertyKeyCount;
            Boolean success = UsbDeviceWinApi.SetupDiGetDevicePropertyKeys(this.handle, ref this.devInfoData, IntPtr.Zero, 0, out propertyKeyCount, 0);

            if (success || (Marshal.GetLastWin32Error() != UsbDeviceWinApi.ERROR_INSUFFICIENT_BUFFER))
            {
                this.TraceError("SetupDiGetDevicePropertyKeys");
                return;
            }

            if (0 == propertyKeyCount)
            {
                throw new Exception(); // TODO
            }

            UsbDeviceWinApi.DEVPROPKEY[] propertyKeyArray = new UsbDeviceWinApi.DEVPROPKEY[propertyKeyCount];
            GCHandle propertyKeyArrayPinned = GCHandle.Alloc(propertyKeyArray , GCHandleType.Pinned);

            IntPtr buffer = propertyKeyArrayPinned.AddrOfPinnedObject();

            success = UsbDeviceWinApi.SetupDiGetDevicePropertyKeys(this.handle, ref this.devInfoData, buffer, propertyKeyCount, out propertyKeyCount, 0);

            if (success)
            {
                for (UInt32 propertyKeyIndex = 0; propertyKeyIndex < propertyKeyCount; propertyKeyIndex++)
                {
                    UInt32 propertyType;

                    UInt32 requiredSize;

                    success = UsbDeviceWinApi.SetupDiGetDevicePropertyW(this.handle, ref this.devInfoData,
                        ref propertyKeyArray[propertyKeyIndex], out propertyType, IntPtr.Zero, 0, out requiredSize, 0);

                    if (success || (Marshal.GetLastWin32Error() != UsbDeviceWinApi.ERROR_INSUFFICIENT_BUFFER))
                    {
                        this.TraceError("SetupDiGetDeviceProperty");
                        success = false;
                    }
                    else
                    {
                        buffer = Marshal.AllocHGlobal((Int32)requiredSize);

                        success = UsbDeviceWinApi.SetupDiGetDevicePropertyW(this.handle, ref this.devInfoData,
                            ref propertyKeyArray[propertyKeyIndex], out propertyType, buffer, requiredSize, out requiredSize, 0);

                        if (success)
                        {
                            Object value = this.MarshalDeviceProperty(buffer, (Int32)requiredSize, propertyType);
                            this.UsbDevice.Properties.Add(new UsbDeviceProperty(propertyKeyArray[propertyKeyIndex], value, propertyType));
                        }
                        else
                        {
                            this.TraceError("SetupDiGetDevicePropertyW");
                        }

                        Marshal.FreeHGlobal(buffer);
                    }

                    if (!success) // don't combine with previous "if", covers 2 cases
                    {
                        this.UsbDevice.Properties.Add(new UsbDeviceProperty(propertyKeyArray[propertyKeyIndex], null, UsbDeviceWinApi.DEVPROP_TYPE_EMPTY));
                    }
                }
            }
            else
            {
                this.TraceError("SetupDiGetDevicePropertyKeys");
            }

            propertyKeyArrayPinned.Free();
        }

        private Object MarshalDeviceProperty(IntPtr source, Int32 length, UInt32 type)
        {
            // TODO: add other types; now covers only all types that are mentioned in devpkey.h
            switch (type)
            {
                case UsbDeviceWinApi.DEVPROP_TYPE_UINT32:
                    return (UInt32)Marshal.ReadInt32(source);
                case UsbDeviceWinApi.DEVPROP_TYPE_GUID:
                    return MarshalEx.ReadGuid(source, length);
                case UsbDeviceWinApi.DEVPROP_TYPE_FILETIME:
                    return MarshalEx.ReadFileTime(source);
                case UsbDeviceWinApi.DEVPROP_TYPE_BOOLEAN:
                    return Marshal.ReadByte(source) != 0;
                case UsbDeviceWinApi.DEVPROP_TYPE_STRING:
                    return Marshal.PtrToStringUni(source);
                case UsbDeviceWinApi.DEVPROP_TYPE_SECURITY_DESCRIPTOR:
                    return MarshalEx.ReadSecurityDescriptor(source, length);
                case UsbDeviceWinApi.DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING:
                    return Marshal.PtrToStringUni(source);
                case UsbDeviceWinApi.DEVPROP_TYPE_BINARY:
                    return MarshalEx.ReadByteArray(source, length);
                case UsbDeviceWinApi.DEVPROP_TYPE_STRING_LIST:
                    return MarshalEx.ReadMultiSzStringList(source, length);
                default:
                    return null;
            }
        }

        private void GetRegistryProperties()
        {
            for (UInt32 property = 0; property < UsbDeviceWinApi.SpDrpMaximumProperty; property++)
            {
                UInt32 regtype;
                UInt32 requiredSize;

                Boolean success = UsbDeviceWinApi.SetupDiGetDeviceRegistryProperty(this.handle, ref this.devInfoData,
                    property, out regtype, IntPtr.Zero, 0, out requiredSize);

                if (success || (Marshal.GetLastWin32Error() != UsbDeviceWinApi.ERROR_INSUFFICIENT_BUFFER))
                {
                    this.UsbDevice.RegistryProperties.Add(property, null);
                    if (Marshal.GetLastWin32Error() != UsbDeviceWinApi.ERROR_INVALID_DATA)
                    {
                        this.TraceError("SetupDiGetDeviceRegistryProperty");
                    }
                }
                else
                {
                    IntPtr buffer = Marshal.AllocHGlobal((Int32)requiredSize);

                    success = UsbDeviceWinApi.SetupDiGetDeviceRegistryProperty(this.handle, ref this.devInfoData,
                        property, out regtype, buffer, requiredSize, out requiredSize);

                    if (success)
                    {
                        String value = Marshal.PtrToStringAuto(buffer);
                        this.UsbDevice.RegistryProperties.Add(property, value);
                    }
                    else
                    {
                        this.UsbDevice.RegistryProperties.Add(property, null);
                        this.TraceError("SetupDiGetDeviceRegistryProperty");
                    }

                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

        private void GetHubAndPort()
        {
            String value = this.UsbDevice.RegistryProperties[UsbDeviceWinApi.SpDrpLocationInformation];

            this.UsbDevice.Hub = String.IsNullOrEmpty(value) ? null : ExtractStringAfterPrefix(value, "Hub_#", 4);
            this.UsbDevice.Port = String.IsNullOrEmpty(value) ? null : ExtractStringAfterPrefix(value, "Port_#", 4);
        }

        private String ExtractStringAfterPrefix(String text, String prefix, Int32 length)
        {
            Int32 index = text.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
            return index >= 0 ? text.Substring(index + prefix.Length, length) : "";
        }

        private String ReadBusReportedDeviceDescription()
        {
            UsbDeviceProperty usbDeviceProperty = this.UsbDevice.GetProperty(UsbDeviceWinApi.DEVPKEY_Device_BusReportedDeviceDesc);

            return null == usbDeviceProperty ? null : usbDeviceProperty.Value as String;
        }
    }
}
