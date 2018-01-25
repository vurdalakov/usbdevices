namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public class DeviceManagementNotificationsEventArgs : EventArgs
    {
        public Boolean Connected { get; private set; }
        public Guid DeviceClass { get; private set; }
        public String DevicePath { get; private set; }

        public DeviceManagementNotificationsEventArgs(Boolean connected, Guid deviceClass, String devicePath)
        {
            this.Connected = connected;
            this.DeviceClass = deviceClass;
            this.DevicePath = devicePath;
        }
    }
}
