namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public class UsbDeviceProperty
    {
        public UsbDeviceWinApi.DEVPROPKEY Key;
        public Object Value;
        public UInt32 Type;

        public Boolean HasSameKey(UsbDeviceWinApi.DEVPROPKEY devPropKey)
        {
            return (this.Key.Fmtid == devPropKey.Fmtid) && (this.Key.Pid == devPropKey.Pid);
        }

        internal UsbDeviceProperty(UsbDeviceWinApi.DEVPROPKEY key, Object value, UInt32 type)
        {
            this.Key = key;
            this.Value = value;
            this.Type = type;
        }
    }
}
