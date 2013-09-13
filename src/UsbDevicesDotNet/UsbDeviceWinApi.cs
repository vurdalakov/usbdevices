namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Runtime.InteropServices;

    public static class UsbDeviceWinApi
    {
        public const Int32 ERROR_SUCCESS = 0;
        public const Int32 ERROR_INVALID_DATA = 13;
        public const Int32 ERROR_INSUFFICIENT_BUFFER = 122;
        public const Int32 ERROR_NO_MORE_ITEMS = 259;

        public const Int32 CR_SUCCESS = 0;

        public const Int32 SPDRP_DEVICEDESC                  = 0x00000000;  // DeviceDesc (R/W)
        public const Int32 SPDRP_HARDWAREID                  = 0x00000001;  // HardwareID (R/W)
        public const Int32 SPDRP_COMPATIBLEIDS               = 0x00000002;  // CompatibleIDs (R/W)
        public const Int32 SPDRP_UNUSED0                     = 0x00000003;  // unused
        public const Int32 SPDRP_SERVICE                     = 0x00000004;  // Service (R/W)
        public const Int32 SPDRP_UNUSED1                     = 0x00000005;  // unused
        public const Int32 SPDRP_UNUSED2                     = 0x00000006;  // unused
        public const Int32 SPDRP_CLASS                       = 0x00000007;  // Class (R--tied to ClassGUID)
        public const Int32 SPDRP_CLASSGUID                   = 0x00000008;  // ClassGUID (R/W)
        public const Int32 SPDRP_DRIVER                      = 0x00000009;  // Driver (R/W)
        public const Int32 SPDRP_CONFIGFLAGS                 = 0x0000000A;  // ConfigFlags (R/W)
        public const Int32 SPDRP_MFG                         = 0x0000000B;  // Mfg (R/W)
        public const Int32 SPDRP_FRIENDLYNAME                = 0x0000000C;  // FriendlyName (R/W)
        public const Int32 SPDRP_LOCATION_INFORMATION        = 0x0000000D;  // LocationInformation (R/W)
        public const Int32 SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E;  // PhysicalDeviceObjectName (R)
        public const Int32 SPDRP_CAPABILITIES                = 0x0000000F;  // Capabilities (R)
        public const Int32 SPDRP_UI_NUMBER                   = 0x00000010;  // UiNumber (R)
        public const Int32 SPDRP_UPPERFILTERS                = 0x00000011;  // UpperFilters (R/W)
        public const Int32 SPDRP_LOWERFILTERS                = 0x00000012;  // LowerFilters (R/W)
        public const Int32 SPDRP_BUSTYPEGUID                 = 0x00000013;  // BusTypeGUID (R)
        public const Int32 SPDRP_LEGACYBUSTYPE               = 0x00000014;  // LegacyBusType (R)
        public const Int32 SPDRP_BUSNUMBER                   = 0x00000015;  // BusNumber (R)
        public const Int32 SPDRP_ENUMERATOR_NAME             = 0x00000016;  // Enumerator Name (R)
        public const Int32 SPDRP_SECURITY                    = 0x00000017;  // Security (R/W, binary form)
        public const Int32 SPDRP_SECURITY_SDS                = 0x00000018;  // Security (W, SDS form)
        public const Int32 SPDRP_DEVTYPE                     = 0x00000019;  // Device Type (R/W)
        public const Int32 SPDRP_EXCLUSIVE                   = 0x0000001A;  // Device is exclusive-access (R/W)
        public const Int32 SPDRP_CHARACTERISTICS             = 0x0000001B;  // Device Characteristics (R/W)
        public const Int32 SPDRP_ADDRESS                     = 0x0000001C;  // Device Address (R)
        public const Int32 SPDRP_UI_NUMBER_DESC_FORMAT       = 0X0000001D;  // UiNumberDescFormat (R/W)
        public const Int32 SPDRP_DEVICE_POWER_DATA           = 0x0000001E;  // Device Power Data (R)
        public const Int32 SPDRP_REMOVAL_POLICY              = 0x0000001F;  // Removal Policy (R)
        public const Int32 SPDRP_REMOVAL_POLICY_HW_DEFAULT   = 0x00000020;  // Hardware Removal Policy (R)
        public const Int32 SPDRP_REMOVAL_POLICY_OVERRIDE     = 0x00000021;  // Removal Policy Override (RW)
        public const Int32 SPDRP_INSTALL_STATE               = 0x00000022;  // Device Install State (R)
        public const Int32 SPDRP_LOCATION_PATHS              = 0x00000023;  // Device Location Paths (R)
        public const Int32 SPDRP_BASE_CONTAINERID            = 0x00000024;  // Base ContainerID (R)

        public const Int32 SPDRP_MAXIMUM_PROPERTY            = 0x00000025;  // Upper bound on ordinals

        [Flags]
        public enum SetupDiGetClassDevsFlags
        {
            DIGCF_DEFAULT = 0x00000001,
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010
        }

        public static IntPtr InvalidHandleValue { get { return new IntPtr(-1); } }

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, SetupDiGetClassDevsFlags flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr devInfoHandle, IntPtr devInfo, ref Guid interfaceClassGuid, UInt32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr devInfoHandle, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, ref SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, UInt32 property, out UInt32 propertyRegDataType, IntPtr propertyBuffer, UInt32 propertyBufferSize, out UInt32 requiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiOpenDeviceInfo(IntPtr deviceInfoSet, String deviceInstanceId, IntPtr hwndParent, Int32 openFlags, out SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Boolean SetupDiGetDevicePropertyKeys(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, IntPtr propertyKeyArray,
            UInt32 propertyKeyCount, out UInt32 requiredPropertyKeyCount, UInt32 flags);
        
        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiGetDevicePropertyW(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, ref DEVPROPKEY propertyKey, out UInt32 propertyType, IntPtr propertyBuffer, UInt32 propertyBufferSize, out UInt32 requiredSize, UInt32 flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Int32 CM_Get_Parent(out UInt32 devInstParent, UInt32 devInst, Int32 flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Int32 CM_Get_Child(out UInt32 devInstChild, UInt32 devInst, Int32 ulFlags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Int32 CM_Get_Sibling(out UInt32 devInstSibling, UInt32 devInst, Int32 ulFlags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 CM_Get_Device_ID(UInt32 devInst, IntPtr buffer, Int32 bufferLen, Int32 flags);

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public UInt32 Size;
            public Guid ClassGuid;
            public UInt32 DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public UInt32 Size;
            public Guid InterfaceClassGuid;
            public UInt32 Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 Size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String DevicePath;
        }

        #region devpropdef.h

        //
        // Property type modifiers.  Used to modify base DEVPROP_TYPE_ values, as
        // appropriate.  Not valid as standalone DEVPROPTYPE values.
        //
        public const UInt32 DEVPROP_TYPEMOD_ARRAY                   = 0x00001000;  // array of fixed-sized data elements
        public const UInt32 DEVPROP_TYPEMOD_LIST                    = 0x00002000;  // list of variable-sized data elements

        //
        // Property data types.
        //
        public const UInt32 DEVPROP_TYPE_EMPTY                      = 0x00000000;  // nothing, no property data
        public const UInt32 DEVPROP_TYPE_NULL                       = 0x00000001;  // null property data
        public const UInt32 DEVPROP_TYPE_SBYTE                      = 0x00000002;  // 8-bit signed int (SBYTE)
        public const UInt32 DEVPROP_TYPE_BYTE                       = 0x00000003;  // 8-bit unsigned int (BYTE)
        public const UInt32 DEVPROP_TYPE_INT16                      = 0x00000004;  // 16-bit signed int (SHORT)
        public const UInt32 DEVPROP_TYPE_UINT16                     = 0x00000005;  // 16-bit unsigned int (USHORT)
        public const UInt32 DEVPROP_TYPE_INT32                      = 0x00000006;  // 32-bit signed int (LONG)
        public const UInt32 DEVPROP_TYPE_UINT32                     = 0x00000007;  // 32-bit unsigned int (ULONG)
        public const UInt32 DEVPROP_TYPE_INT64                      = 0x00000008;  // 64-bit signed int (LONG64)
        public const UInt32 DEVPROP_TYPE_UINT64                     = 0x00000009;  // 64-bit unsigned int (ULONG64)
        public const UInt32 DEVPROP_TYPE_FLOAT                      = 0x0000000A;  // 32-bit floating-point (FLOAT)
        public const UInt32 DEVPROP_TYPE_DOUBLE                     = 0x0000000B;  // 64-bit floating-point (DOUBLE)
        public const UInt32 DEVPROP_TYPE_DECIMAL                    = 0x0000000C;  // 128-bit data (DECIMAL)
        public const UInt32 DEVPROP_TYPE_GUID                       = 0x0000000D;  // 128-bit unique identifier (GUID)
        public const UInt32 DEVPROP_TYPE_CURRENCY                   = 0x0000000E;  // 64 bit signed int currency value (CURRENCY)
        public const UInt32 DEVPROP_TYPE_DATE                       = 0x0000000F;  // date (DATE)
        public const UInt32 DEVPROP_TYPE_FILETIME                   = 0x00000010;  // file time (FILETIME)
        public const UInt32 DEVPROP_TYPE_BOOLEAN                    = 0x00000011;  // 8-bit boolean (DEVPROP_BOOLEAN)
        public const UInt32 DEVPROP_TYPE_STRING                     = 0x00000012;  // null-terminated string
        public const UInt32 DEVPROP_TYPE_STRING_LIST = DEVPROP_TYPE_STRING | DEVPROP_TYPEMOD_LIST; // multi-sz string list
        public const UInt32 DEVPROP_TYPE_SECURITY_DESCRIPTOR        = 0x00000013;  // self-relative binary SECURITY_DESCRIPTOR
        public const UInt32 DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING = 0x00000014;  // security descriptor string (SDDL format)
        public const UInt32 DEVPROP_TYPE_DEVPROPKEY                 = 0x00000015;  // device property key (DEVPROPKEY)
        public const UInt32 DEVPROP_TYPE_DEVPROPTYPE                = 0x00000016;  // device property type (DEVPROPTYPE)
        public const UInt32 DEVPROP_TYPE_BINARY = DEVPROP_TYPE_BYTE | DEVPROP_TYPEMOD_ARRAY;  // custom binary data
        public const UInt32 DEVPROP_TYPE_ERROR                      = 0x00000017;  // 32-bit Win32 system error code
        public const UInt32 DEVPROP_TYPE_NTSTATUS                   = 0x00000018;  // 32-bit NTSTATUS code
        public const UInt32 DEVPROP_TYPE_STRING_INDIRECT            = 0x00000019;  // string resource (@[path\]<dllname>,-<strId>)

        //
        // Max base DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
        //
        public const UInt32 MAX_DEVPROP_TYPE                        = 0x00000019;  // max valid DEVPROP_TYPE_ value
        public const UInt32 MAX_DEVPROP_TYPEMOD                     = 0x00002000;  // max valid DEVPROP_TYPEMOD_ value

        //
        // Bitmasks for extracting DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
        //
        public const UInt32 DEVPROP_MASK_TYPE                       = 0x00000FFF;  // range for base DEVPROP_TYPE_ values
        public const UInt32 DEVPROP_MASK_TYPEMOD                    = 0x0000F000;  // mask for DEVPROP_TYPEMOD_ type modifiers

        //
        // DEVPROPKEY structure
        //

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVPROPKEY
        {
            public Guid Fmtid;
            public UInt32 Pid;
        }

        #endregion

        #region devpkey.h

        public static DEVPROPKEY DEVPKEY_Device_BusReportedDeviceDesc = new DEVPROPKEY() { Fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), Pid = 4 };

        #endregion
    }
}
