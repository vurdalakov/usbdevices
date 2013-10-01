namespace UsbDevicesViewer
{
    using System;
    using Vurdalakov;

    public class WmiEvent : ViewModelBase
    {
        public DateTime Time { get; private set; }

        public Boolean ConnectEvent { get; private set; }

        public Win32UsbControllerDevice Device { get; private set; }

        public String HubAndPort { get; private set; }

        public WmiEvent(Boolean connectEvent, Win32UsbControllerDevice win32UsbControllerDevice)
        {
            this.Time = DateTime.Now;
            this.ConnectEvent = connectEvent;
            this.Device = win32UsbControllerDevice;
            this.HubAndPort = String.Format("{0}:{1}", win32UsbControllerDevice.Hub, win32UsbControllerDevice.Port);
        }
    }
}
