namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public class Win32UsbControllerDeviceEventArgs : EventArgs
    {
        public Win32UsbControllerDevice Device { get; private set; }

        public Win32UsbControllerDeviceEventArgs(Win32UsbControllerDevice win32UsbControllerDevice)
        {
            this.Device = win32UsbControllerDevice;
        }
    }
}
