namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public class Win32UsbControllerDevice
    {
        public String DeviceId { get; set; }

        public String ControllerId { get; set; }

        public String Hub { get; set; }
        public String Port { get; set; }

        public String Vid { get; set; }
        public String Pid { get; set; }
    }
}
