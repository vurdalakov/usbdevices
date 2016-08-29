namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Collections.Generic;

    public class UsbDevice
    {
        public String Vid { get; internal set; }
        public String Pid { get; internal set; }

        public String Hub { get; internal set; }
        public String Port { get; internal set; }

        public String DevicePath { get; internal set; }
        public String DeviceId { get; internal set; }

        public UsbDeviceInterface[] Interfaces { get; internal set; }

        public String BusReportedDeviceDescription { get; internal set; }

        public UsbDeviceProperty[] Properties { get; internal set; }

        public UsbDeviceRegistryProperty[] RegistryProperties { get; internal set; }

        internal UsbDevice()
        {
        }

        public UsbDeviceProperty GetProperty(UsbDeviceWinApi.DEVPROPKEY devPropKey)
        {
            for (Int32 i = 0; i < this.Properties.Length; i++)
            {
                if (this.Properties[i].HasSameKey(devPropKey))
                {
                    return this.Properties[i];
                }
            }

            return null;
        }

        public Object GetPropertyValue(UsbDeviceWinApi.DEVPROPKEY devPropKey)
        {
            UsbDeviceProperty usbDeviceProperty = this.GetProperty(devPropKey);
            return null == usbDeviceProperty ? null : usbDeviceProperty.Value;
        }

        public UsbDeviceRegistryProperty GetRegistryProperty(UInt32 key)
        {
            for (Int32 i = 0; i < this.RegistryProperties.Length; i++)
            {
                if (this.RegistryProperties[i].HasSameKey(key))
                {
                    return this.RegistryProperties[i];
                }
            }

            return null;
        }

        public Object GetRegistryPropertyValue(UInt32 key)
        {
            UsbDeviceRegistryProperty usbDeviceRegistryProperty = this.GetRegistryProperty(key);
            return null == usbDeviceRegistryProperty ? null : usbDeviceRegistryProperty.Value;
        }

        public static UsbDevice[] GetDevices()
        {
            return UsbDevice.GetDevices(new Guid(UsbDeviceWinApi.GUID_DEVINTERFACE_USB_DEVICE));
        }

        public static UsbDevice[] GetDevices(Guid classGuid)
        {
            using (UsbDevices usbDevices = new UsbDevices(classGuid))
            {
                return usbDevices.GetDevices();
            }
        }
    }
}
