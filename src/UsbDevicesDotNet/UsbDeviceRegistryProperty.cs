namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public class UsbDeviceRegistryProperty
    {
        public UInt32 Key;
        public Object Value;
        public UInt32 Type;

        public Boolean HasSameKey(UInt32 key)
        {
            return this.Key == key;
        }

        internal UsbDeviceRegistryProperty(UInt32 key, Object value, UInt32 type)
        {
            this.Key = key;
            this.Value = value;
            this.Type = type;
        }
    }
}
