namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static partial class UsbDeviceWinApi
    {

        // setupapi.h

        public static class DeviceRegistryPropertyKeys
        {
            public static UInt32 SPDRP_DEVICEDESC = 0x00000000;
            public static UInt32 SPDRP_HARDWAREID = 0x00000001;
            public static UInt32 SPDRP_COMPATIBLEIDS = 0x00000002;
            public static UInt32 SPDRP_UNUSED0 = 0x00000003;
            public static UInt32 SPDRP_SERVICE = 0x00000004;
            public static UInt32 SPDRP_UNUSED1 = 0x00000005;
            public static UInt32 SPDRP_UNUSED2 = 0x00000006;
            public static UInt32 SPDRP_CLASS = 0x00000007;
            public static UInt32 SPDRP_CLASSGUID = 0x00000008;
            public static UInt32 SPDRP_DRIVER = 0x00000009;
            public static UInt32 SPDRP_CONFIGFLAGS = 0x0000000A;
            public static UInt32 SPDRP_MFG = 0x0000000B;
            public static UInt32 SPDRP_FRIENDLYNAME = 0x0000000C;
            public static UInt32 SPDRP_LOCATION_INFORMATION = 0x0000000D;
            public static UInt32 SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E;
            public static UInt32 SPDRP_CAPABILITIES = 0x0000000F;
            public static UInt32 SPDRP_UI_NUMBER = 0x00000010;
            public static UInt32 SPDRP_UPPERFILTERS = 0x00000011;
            public static UInt32 SPDRP_LOWERFILTERS = 0x00000012;
            public static UInt32 SPDRP_BUSTYPEGUID = 0x00000013;
            public static UInt32 SPDRP_LEGACYBUSTYPE = 0x00000014;
            public static UInt32 SPDRP_BUSNUMBER = 0x00000015;
            public static UInt32 SPDRP_ENUMERATOR_NAME = 0x00000016;
            public static UInt32 SPDRP_SECURITY = 0x00000017;
            public static UInt32 SPDRP_SECURITY_SDS = 0x00000018;
            public static UInt32 SPDRP_DEVTYPE = 0x00000019;
            public static UInt32 SPDRP_EXCLUSIVE = 0x0000001A;
            public static UInt32 SPDRP_CHARACTERISTICS = 0x0000001B;
            public static UInt32 SPDRP_ADDRESS = 0x0000001C;
            public static UInt32 SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D;
            public static UInt32 SPDRP_DEVICE_POWER_DATA = 0x0000001E;
            public static UInt32 SPDRP_REMOVAL_POLICY = 0x0000001F;
            public static UInt32 SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020;
            public static UInt32 SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021;
            public static UInt32 SPDRP_INSTALL_STATE = 0x00000022;
            public static UInt32 SPDRP_LOCATION_PATHS = 0x00000023;
            public static UInt32 SPDRP_BASE_CONTAINERID = 0x00000024;
            public static UInt32 SPDRP_MAXIMUM_PROPERTY = 0x00000025;
        }
    }
}
