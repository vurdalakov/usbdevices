namespace UsbDevicesViewer
{
    using System;
    using Vurdalakov;

    public class WmiEvent : ViewModelBase
    {
        public DateTime Time { get; private set; }

        public Int32 EventType { get; private set; }

        public Win32UsbControllerDevice Device { get; private set; }

        public String HubAndPort { get; private set; }

        public WmiEvent(Int32 eventType, Win32UsbControllerDevice win32UsbControllerDevice)
        {
            this.Time = DateTime.Now;
            this.EventType = eventType;
            this.Device = win32UsbControllerDevice;
            this.HubAndPort = Helpers.MakeHubAndPort(win32UsbControllerDevice.Hub, win32UsbControllerDevice.Port);
        }
    }
}
