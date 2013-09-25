namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Text;
    using System.Reflection;

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

        public String GetDescription()
        {
            foreach (FieldInfo field in typeof(UsbDeviceWinApi.DeviceRegistryPropertyKeys).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                UInt32 key = (UInt32)field.GetValue(null);
                if (this.HasSameKey(key))
                {
                    return field.Name;
                }
            }

            return String.Format("0x{0:X8}", this.Key);
        }

        public String[] GetValue()
        {
            switch (this.Type)
            {
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_NONE:
                    return this.MakeArray(String.Empty);
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_SZ:
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_LINK:
                    return this.MakeArray(this.Value as String);
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_EXPAND_SZ:
                    return this.MakeArray(Environment.ExpandEnvironmentVariables(this.Value as String));
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_BINARY:
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (Byte b in (Byte[])this.Value)
                    {
                        stringBuilder.AppendFormat("{0:X2},", b);
                    }
                    return this.MakeArray(stringBuilder.ToString());
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_DWORD:
              //case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_DWORD_LITTLE_ENDIAN:
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_DWORD_BIG_ENDIAN:
                    return this.MakeArray(String.Format("0x{0:X8}", this.Value));
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_MULTI_SZ:
                    String[] strings = this.Value as String[];
                    return 0 == strings.Length ? this.MakeArray(String.Empty) : strings;
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_RESOURCE_LIST:
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_FULL_RESOURCE_DESCRIPTOR:
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_RESOURCE_REQUIREMENTS_LIST:
                    return this.MakeArray(String.Format("Unsupported property type: ", this.Type)); // TODO
                case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_QWORD:
              //case UsbDeviceWinApi.DeviceRegistryPropertyTypes.REG_QWORD_LITTLE_ENDIAN:
                    return this.MakeArray(String.Format("0x{0:X16}", this.Value));
                default:
                    return this.MakeArray(String.Format("Unknown property type: ", this.Type));
            }
        }

        public String GetType()
        {
            foreach (FieldInfo field in typeof(UsbDeviceWinApi.DeviceRegistryPropertyTypes).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                Int32 type = (Int32)field.GetValue(null);
                if (this.Type == type)
                {
                    return field.Name;
                }
            }

            return String.Format("{0}", this.Type);
        }

        private String[] MakeArray(String value)
        {
            return new String[] { value };
        }
    }
}
