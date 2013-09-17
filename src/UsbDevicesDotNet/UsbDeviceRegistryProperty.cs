namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Text;

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
            switch (this.Key)
            {
                case UsbDeviceWinApi.SPDRP_DEVICEDESC:
                    return "SPDRP_DEVICEDESC";
                case UsbDeviceWinApi.SPDRP_HARDWAREID:
                    return "SPDRP_HARDWAREID";
                case UsbDeviceWinApi.SPDRP_COMPATIBLEIDS:
                    return "SPDRP_COMPATIBLEIDS";
                case UsbDeviceWinApi.SPDRP_SERVICE:
                    return "SPDRP_SERVICE";
                case UsbDeviceWinApi.SPDRP_CLASS:
                    return "SPDRP_CLASS";
                case UsbDeviceWinApi.SPDRP_CLASSGUID:
                    return "SPDRP_CLASSGUID";
                case UsbDeviceWinApi.SPDRP_DRIVER:
                    return "SPDRP_DRIVER";
                case UsbDeviceWinApi.SPDRP_CONFIGFLAGS:
                    return "SPDRP_CONFIGFLAGS";
                case UsbDeviceWinApi.SPDRP_MFG:
                    return "SPDRP_MFG";
                case UsbDeviceWinApi.SPDRP_FRIENDLYNAME:
                    return "SPDRP_FRIENDLYNAME";
                case UsbDeviceWinApi.SPDRP_LOCATION_INFORMATION:
                    return "SPDRP_LOCATION_INFORMATION";
                case UsbDeviceWinApi.SPDRP_PHYSICAL_DEVICE_OBJECT_NAME:
                    return "SPDRP_PHYSICAL_DEVICE_OBJECT_NAME";
                case UsbDeviceWinApi.SPDRP_CAPABILITIES:
                    return "SPDRP_CAPABILITIES";
                case UsbDeviceWinApi.SPDRP_UI_NUMBER:
                    return "SPDRP_UI_NUMBER";
                case UsbDeviceWinApi.SPDRP_UPPERFILTERS:
                    return "SPDRP_UPPERFILTERS";
                case UsbDeviceWinApi.SPDRP_LOWERFILTERS:
                    return "SPDRP_LOWERFILTERS";
                case UsbDeviceWinApi.SPDRP_BUSTYPEGUID:
                    return "SPDRP_BUSTYPEGUID";
                case UsbDeviceWinApi.SPDRP_LEGACYBUSTYPE:
                    return "SPDRP_LEGACYBUSTYPE";
                case UsbDeviceWinApi.SPDRP_BUSNUMBER:
                    return "SPDRP_BUSNUMBER";
                case UsbDeviceWinApi.SPDRP_ENUMERATOR_NAME:
                    return "SPDRP_ENUMERATOR_NAME";
                case UsbDeviceWinApi.SPDRP_SECURITY:
                    return "SPDRP_SECURITY";
                case UsbDeviceWinApi.SPDRP_SECURITY_SDS:
                    return "SPDRP_SECURITY_SDS";
                case UsbDeviceWinApi.SPDRP_DEVTYPE:
                    return "SPDRP_DEVTYPE";
                case UsbDeviceWinApi.SPDRP_EXCLUSIVE:
                    return "SPDRP_EXCLUSIVE";
                case UsbDeviceWinApi.SPDRP_CHARACTERISTICS:
                    return "SPDRP_CHARACTERISTICS";
                case UsbDeviceWinApi.SPDRP_ADDRESS:
                    return "SPDRP_ADDRESS";
                case UsbDeviceWinApi.SPDRP_UI_NUMBER_DESC_FORMAT:
                    return "SPDRP_UI_NUMBER_DESC_FORMAT";
                case UsbDeviceWinApi.SPDRP_DEVICE_POWER_DATA:
                    return "SPDRP_DEVICE_POWER_DATA";
                case UsbDeviceWinApi.SPDRP_REMOVAL_POLICY:
                    return "SPDRP_REMOVAL_POLICY";
                case UsbDeviceWinApi.SPDRP_REMOVAL_POLICY_HW_DEFAULT:
                    return "SPDRP_REMOVAL_POLICY_HW_DEFAULT";
                case UsbDeviceWinApi.SPDRP_REMOVAL_POLICY_OVERRIDE:
                    return "SPDRP_REMOVAL_POLICY_OVERRIDE";
                case UsbDeviceWinApi.SPDRP_INSTALL_STATE:
                    return "SPDRP_INSTALL_STATE";
                case UsbDeviceWinApi.SPDRP_LOCATION_PATHS:
                    return "SPDRP_LOCATION_PATHS";
                case UsbDeviceWinApi.SPDRP_BASE_CONTAINERID:
                    return "SPDRP_BASE_CONTAINERID";
                default:
                    return "<unknown>";
            }
        }

        public String[] GetValue()
        {
            switch (this.Type)
            {
                case UsbDeviceWinApi.REG_NONE:
                    return this.MakeArray(String.Empty);
                case UsbDeviceWinApi.REG_SZ:
                case UsbDeviceWinApi.REG_LINK:
                    return this.MakeArray(this.Value as String);
                case UsbDeviceWinApi.REG_EXPAND_SZ:
                    return this.MakeArray(Environment.ExpandEnvironmentVariables(this.Value as String));
                case UsbDeviceWinApi.REG_BINARY:
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (Byte b in (Byte[])this.Value)
                    {
                        stringBuilder.AppendFormat("{0:X2},", b);
                    }
                    return this.MakeArray(stringBuilder.ToString());
                case UsbDeviceWinApi.REG_DWORD:
              //case UsbDeviceWinApi.REG_DWORD_LITTLE_ENDIAN:
                case UsbDeviceWinApi.REG_DWORD_BIG_ENDIAN:
                    return this.MakeArray(String.Format("0x{0:X8}", this.Value));
                case UsbDeviceWinApi.REG_MULTI_SZ:
                    String[] strings = this.Value as String[];
                    return 0 == strings.Length ? this.MakeArray(String.Empty) : strings;
                case UsbDeviceWinApi.REG_RESOURCE_LIST:
                case UsbDeviceWinApi.REG_FULL_RESOURCE_DESCRIPTOR:
                case UsbDeviceWinApi.REG_RESOURCE_REQUIREMENTS_LIST:
                    return this.MakeArray(String.Format("Unsupported property type: ", this.Type)); // TODO
                case UsbDeviceWinApi.REG_QWORD:
              //case UsbDeviceWinApi.REG_QWORD_LITTLE_ENDIAN:
                    return this.MakeArray(String.Format("0x{0:X16}", this.Value));
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
