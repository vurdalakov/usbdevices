using System;

namespace Vurdalakov.UsbDevicesDotNet
{
    public class UsbDevice
    {
        public String Vid { get; set; }
        public String Pid { get; set; }

        public String Hub { get; set; }
        public String Port { get; set; }

        public String DevicePath { get; set; }
        public String DeviceId { get; set; }

        public String BusReportedDeviceDescription { get; set; }

        public static UsbDevice[] GetDevices(Guid classGuid)
        {
            using (UsbDevices usbDevices = new UsbDevices(classGuid))
            {
                return usbDevices.GetDevices();
            }
        }
    }
}
