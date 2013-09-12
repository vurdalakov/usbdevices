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

        public Dictionary<UInt32, String> RegistryProperties { get; private set; }

        public UsbDevice()
        {
            this.RegistryProperties = new Dictionary<UInt32, String>();
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
