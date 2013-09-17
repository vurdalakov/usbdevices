namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Text;
    using System.Reflection;

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

        public String GetDescription()
        {
            foreach (FieldInfo field in typeof(UsbDeviceWinApi.DevicePropertyKeys).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                UsbDeviceWinApi.DEVPROPKEY devPropKey = (UsbDeviceWinApi.DEVPROPKEY)field.GetValue(null);
                if (this.HasSameKey(devPropKey))
                {
                    return field.Name;
                }
            }

            return "Unknown key";
        }

        public String[] GetValues()
        {
            // TODO: add other types; now covers only all types that are mentioned in devpkey.h
            switch (this.Type)
            {
                case UsbDeviceWinApi.DEVPROP_TYPE_UINT32:
                    return this.MakeArray(String.Format("0x{0:X8}", this.Value));
                case UsbDeviceWinApi.DEVPROP_TYPE_GUID:
                    return this.MakeArray(((Guid)this.Value).ToString("B"));
                case UsbDeviceWinApi.DEVPROP_TYPE_FILETIME:
                    return this.MakeArray(((DateTime)this.Value).ToString("yyyy.MM.dd HH:mm:ss.ffff"));
                case UsbDeviceWinApi.DEVPROP_TYPE_BOOLEAN:
                    return this.MakeArray((Boolean)this.Value ? "True" : "False");
                case UsbDeviceWinApi.DEVPROP_TYPE_STRING:
                    return this.MakeArray(this.Value as String);
                case UsbDeviceWinApi.DEVPROP_TYPE_SECURITY_DESCRIPTOR:
                    return this.MakeArray(String.Format("Unsupported property type: ", this.Type)); // TODO
                case UsbDeviceWinApi.DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING:
                    return this.MakeArray(String.Format("Unsupported property type: ", this.Type)); // TODO
                case UsbDeviceWinApi.DEVPROP_TYPE_BINARY:
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (Byte b in (Byte[])this.Value)
                    {
                        stringBuilder.AppendFormat("{0:X2},", b);
                    }
                    return this.MakeArray(stringBuilder.ToString());
                case UsbDeviceWinApi.DEVPROP_TYPE_STRING_LIST:
                    String[] strings = this.Value as String[];
                    return 0 == strings.Length ? this.MakeArray(String.Empty) : strings;
                default:
                    return this.MakeArray(String.Format("Unknown property type: ", this.Type));
            }
        }

        private String[] MakeArray(String value)
        {
            return new String[] { value };
        }
    }
}
