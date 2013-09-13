namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Collections.Generic;

    public class UsbDevice
    {
        public String Vid { get; set; }
        public String Pid { get; set; }

        public String Hub { get; set; }
        public String Port { get; set; }

        public String DevicePath { get; set; }
        public String DeviceId { get; set; }

        public String[] InterfaceIds { get; set; }

        public String BusReportedDeviceDescription { get; set; }

        public List<UsbDeviceProperty> Properties { get; private set; }

        public List<UsbDeviceRegistryProperty> RegistryProperties { get; private set; }

        public UsbDevice()
        {
            this.Properties = new List<UsbDeviceProperty>();
            this.RegistryProperties = new List<UsbDeviceRegistryProperty>();
        }

        public UsbDeviceProperty GetProperty(UsbDeviceWinApi.DEVPROPKEY devPropKey)
        {
            for (Int32 i = 0; i < this.Properties.Count; i++)
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
            for (Int32 i = 0; i < this.RegistryProperties.Count; i++)
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
            return UsbDevice.GetDevices(new Guid(UsbDeviceGuids.GUID_DEVINTERFACE_USB_DEVICE));
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
