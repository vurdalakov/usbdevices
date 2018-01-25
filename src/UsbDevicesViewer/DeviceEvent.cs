namespace UsbDevicesViewer
{
    using System;
    using Vurdalakov;
    using Vurdalakov.UsbDevicesDotNet;

    public class DeviceEvent : ViewModelBase
    {
        public DateTime Time { get; private set; }

        public Int32 EventType { get; private set; }

        public String Vid { get; private set; }

        public String Pid { get; private set; }

        public String HubAndPort { get; private set; }

        public String DeviceId { get; private set; }

        public String ControllerId { get; private set; }

        public DeviceEvent(Int32 eventType, Win32UsbControllerDevice win32UsbControllerDevice)
        {
            this.Time = DateTime.Now;
            this.EventType = eventType;
            this.Vid = win32UsbControllerDevice.Vid;
            this.Pid = win32UsbControllerDevice.Pid;
            this.HubAndPort = Helpers.MakeHubAndPort(win32UsbControllerDevice.Hub, win32UsbControllerDevice.Port);
            this.DeviceId = win32UsbControllerDevice.DeviceId;
            this.ControllerId = win32UsbControllerDevice.ControllerId;
        }

        public DeviceEvent(Int32 eventType, String devicePath)
        {
            this.Time = DateTime.Now;
            this.EventType = eventType;
            this.DeviceId = devicePath;
        }
    }
}
