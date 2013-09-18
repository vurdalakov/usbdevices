namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static partial class UsbDeviceWinApi
    {

        // winnt.h

        public static class DeviceRegistryPropertyTypes
        {
            public const Int32 REG_NONE = 0;
            public const Int32 REG_SZ = 1;
            public const Int32 REG_EXPAND_SZ = 2;
            public const Int32 REG_BINARY = 3;
            public const Int32 REG_DWORD = 4;
            public const Int32 REG_DWORD_LITTLE_ENDIAN = 4;
            public const Int32 REG_DWORD_BIG_ENDIAN = 5;
            public const Int32 REG_LINK = 6;
            public const Int32 REG_MULTI_SZ = 7;
            public const Int32 REG_RESOURCE_LIST = 8;
            public const Int32 REG_FULL_RESOURCE_DESCRIPTOR = 9;
            public const Int32 REG_RESOURCE_REQUIREMENTS_LIST = 10;
            public const Int32 REG_QWORD = 11;
            public const Int32 REG_QWORD_LITTLE_ENDIAN = 11;
        }
    }
}
