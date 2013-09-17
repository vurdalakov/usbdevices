namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Runtime.InteropServices;

    public static class UsbDeviceWinApi
    {
        #region usbiodef.h / WDK

        // The GUID_DEVINTERFACE_USB_DEVICE device interface class is defined for USB devices that are attached to a USB hub.
        public const String GUID_DEVINTERFACE_USB_DEVICE = "A5DCBF10-6530-11D2-901F-00C04FB951ED";

        // The GUID_DEVINTERFACE_USB_HOST_CONTROLLER device interface class is defined for USB host controller devices. 
        public const String GUID_DEVINTERFACE_USB_HOST_CONTROLLER = "3ABF6F2D-71C4-462A-8A92-1E6861E6AF27";

        // The GUID_DEVINTERFACE_USB_HUB device interface class is defined for USB hub devices. 
        public const String GUID_DEVINTERFACE_USB_HUB = "F18A0E88-C30C-11D0-8815-00A0C906BED8";

        #endregion

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
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr devInfoHandle, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, ref SP_DEVINFO_DATA deviceInfoData);

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

        public static class DevicePropertyKeys
        {
            public static DEVPROPKEY DEVPKEY_NAME = new DEVPROPKEY() { Fmtid = new Guid(0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac), Pid = 10 };
            public static DEVPROPKEY DEVPKEY_Device_DeviceDesc = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_HardwareIds = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_Device_CompatibleIds = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_Device_Service = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 6 };
            public static DEVPROPKEY DEVPKEY_Device_Class = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 9 };
            public static DEVPROPKEY DEVPKEY_Device_ClassGuid = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 10 };
            public static DEVPROPKEY DEVPKEY_Device_Driver = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 11 };
            public static DEVPROPKEY DEVPKEY_Device_ConfigFlags = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 12 };
            public static DEVPROPKEY DEVPKEY_Device_Manufacturer = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 13 };
            public static DEVPROPKEY DEVPKEY_Device_FriendlyName = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 14 };
            public static DEVPROPKEY DEVPKEY_Device_LocationInfo = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 15 };
            public static DEVPROPKEY DEVPKEY_Device_PDOName = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 16 };
            public static DEVPROPKEY DEVPKEY_Device_Capabilities = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 17 };
            public static DEVPROPKEY DEVPKEY_Device_UINumber = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 18 };
            public static DEVPROPKEY DEVPKEY_Device_UpperFilters = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 19 };
            public static DEVPROPKEY DEVPKEY_Device_LowerFilters = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 20 };
            public static DEVPROPKEY DEVPKEY_Device_BusTypeGuid = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 21 };
            public static DEVPROPKEY DEVPKEY_Device_LegacyBusType = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 22 };
            public static DEVPROPKEY DEVPKEY_Device_BusNumber = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 23 };
            public static DEVPROPKEY DEVPKEY_Device_EnumeratorName = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 24 };
            public static DEVPROPKEY DEVPKEY_Device_Security = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 25 };
            public static DEVPROPKEY DEVPKEY_Device_SecuritySDS = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 26 };
            public static DEVPROPKEY DEVPKEY_Device_DevType = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 27 };
            public static DEVPROPKEY DEVPKEY_Device_Exclusive = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 28 };
            public static DEVPROPKEY DEVPKEY_Device_Characteristics = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 29 };
            public static DEVPROPKEY DEVPKEY_Device_Address = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 30 };
            public static DEVPROPKEY DEVPKEY_Device_UINumberDescFormat = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 31 };
            public static DEVPROPKEY DEVPKEY_Device_PowerData = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 32 };
            public static DEVPROPKEY DEVPKEY_Device_RemovalPolicy = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 33 };
            public static DEVPROPKEY DEVPKEY_Device_RemovalPolicyDefault = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 34 };
            public static DEVPROPKEY DEVPKEY_Device_RemovalPolicyOverride = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 35 };
            public static DEVPROPKEY DEVPKEY_Device_InstallState = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 36 };
            public static DEVPROPKEY DEVPKEY_Device_LocationPaths = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 37 };
            public static DEVPROPKEY DEVPKEY_Device_BaseContainerId = new DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 38 };
            public static DEVPROPKEY DEVPKEY_Device_DevNodeStatus = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_ProblemCode = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_Device_EjectionRelations = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_Device_RemovalRelations = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 5 };
            public static DEVPROPKEY DEVPKEY_Device_PowerRelations = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 6 };
            public static DEVPROPKEY DEVPKEY_Device_BusRelations = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 7 };
            public static DEVPROPKEY DEVPKEY_Device_Parent = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 8 };
            public static DEVPROPKEY DEVPKEY_Device_Children = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 9 };
            public static DEVPROPKEY DEVPKEY_Device_Siblings = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 10 };
            public static DEVPROPKEY DEVPKEY_Device_TransportRelations = new DEVPROPKEY() { Fmtid = new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), Pid = 11 };
            public static DEVPROPKEY DEVPKEY_Device_Reported = new DEVPROPKEY() { Fmtid = new Guid(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_Legacy = new DEVPROPKEY() { Fmtid = new Guid(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_Device_InstanceId = new DEVPROPKEY() { Fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), Pid = 256 };
            public static DEVPROPKEY DEVPKEY_Device_ContainerId = new DEVPROPKEY() { Fmtid = new Guid(0x8c7ed206, 0x3f8a, 0x4827, 0xb3, 0xab, 0xae, 0x9e, 0x1f, 0xae, 0xfc, 0x6c), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_ModelId = new DEVPROPKEY() { Fmtid = new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_FriendlyNameAttributes = new DEVPROPKEY() { Fmtid = new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_Device_ManufacturerAttributes = new DEVPROPKEY() { Fmtid = new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_Device_PresenceNotForDevice = new DEVPROPKEY() { Fmtid = new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), Pid = 5 };
            public static DEVPROPKEY DEVPKEY_Numa_Proximity_Domain = new DEVPROPKEY() { Fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), Pid = 1 };
            public static DEVPROPKEY DEVPKEY_Device_DHP_Rebalance_Policy = new DEVPROPKEY() { Fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_Numa_Node = new DEVPROPKEY() { Fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_Device_BusReportedDeviceDesc = new DEVPROPKEY() { Fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_Device_SessionId = new DEVPROPKEY() { Fmtid = new Guid(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29), Pid = 6 };
            public static DEVPROPKEY DEVPKEY_Device_InstallDate = new DEVPROPKEY() { Fmtid = new Guid(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29), Pid = 100 };
            public static DEVPROPKEY DEVPKEY_Device_FirstInstallDate = new DEVPROPKEY() { Fmtid = new Guid(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29), Pid = 101 };
            public static DEVPROPKEY DEVPKEY_Device_DriverDate = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_DriverVersion = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_Device_DriverDesc = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_Device_DriverInfPath = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 5 };
            public static DEVPROPKEY DEVPKEY_Device_DriverInfSection = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 6 };
            public static DEVPROPKEY DEVPKEY_Device_DriverInfSectionExt = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 7 };
            public static DEVPROPKEY DEVPKEY_Device_MatchingDeviceId = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 8 };
            public static DEVPROPKEY DEVPKEY_Device_DriverProvider = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 9 };
            public static DEVPROPKEY DEVPKEY_Device_DriverPropPageProvider = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 10 };
            public static DEVPROPKEY DEVPKEY_Device_DriverCoInstallers = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 11 };
            public static DEVPROPKEY DEVPKEY_Device_ResourcePickerTags = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 12 };
            public static DEVPROPKEY DEVPKEY_Device_ResourcePickerExceptions = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 13 };
            public static DEVPROPKEY DEVPKEY_Device_DriverRank = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 14 };
            public static DEVPROPKEY DEVPKEY_Device_DriverLogoLevel = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 15 };
            public static DEVPROPKEY DEVPKEY_Device_NoConnectSound = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 17 };
            public static DEVPROPKEY DEVPKEY_Device_GenericDriverInstalled = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 18 };
            public static DEVPROPKEY DEVPKEY_Device_AdditionalSoftwareRequested = new DEVPROPKEY() { Fmtid = new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), Pid = 19 };
            public static DEVPROPKEY DEVPKEY_Device_SafeRemovalRequired = new DEVPROPKEY() { Fmtid = new Guid(0xafd97640, 0x86a3, 0x4210, 0xb6, 0x7c, 0x28, 0x9c, 0x41, 0xaa, 0xbe, 0x55), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_Device_SafeRemovalRequiredOverride = new DEVPROPKEY() { Fmtid = new Guid(0xafd97640, 0x86a3, 0x4210, 0xb6, 0x7c, 0x28, 0x9c, 0x41, 0xaa, 0xbe, 0x55), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_DrvPkg_Model = new DEVPROPKEY() { Fmtid = new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_DrvPkg_VendorWebSite = new DEVPROPKEY() { Fmtid = new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_DrvPkg_DetailedDescription = new DEVPROPKEY() { Fmtid = new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_DrvPkg_DocumentationLink = new DEVPROPKEY() { Fmtid = new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), Pid = 5 };
            public static DEVPROPKEY DEVPKEY_DrvPkg_Icon = new DEVPROPKEY() { Fmtid = new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), Pid = 6 };
            public static DEVPROPKEY DEVPKEY_DrvPkg_BrandingIcon = new DEVPROPKEY() { Fmtid = new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), Pid = 7 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_UpperFilters = new DEVPROPKEY() { Fmtid = new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), Pid = 19 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_LowerFilters = new DEVPROPKEY() { Fmtid = new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), Pid = 20 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_Security = new DEVPROPKEY() { Fmtid = new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), Pid = 25 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_SecuritySDS = new DEVPROPKEY() { Fmtid = new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), Pid = 26 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_DevType = new DEVPROPKEY() { Fmtid = new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), Pid = 27 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_Exclusive = new DEVPROPKEY() { Fmtid = new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), Pid = 28 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_Characteristics = new DEVPROPKEY() { Fmtid = new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), Pid = 29 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_Name = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_ClassName = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_Icon = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_ClassInstaller = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 5 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_PropPageProvider = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 6 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_NoInstallClass = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 7 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_NoDisplayClass = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 8 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_SilentInstall = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 9 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_NoUseClass = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 10 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_DefaultService = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 11 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_IconPath = new DEVPROPKEY() { Fmtid = new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), Pid = 12 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_DHPRebalanceOptOut = new DEVPROPKEY() { Fmtid = new Guid(0xd14d3ef3, 0x66cf, 0x4ba2, 0x9d, 0x38, 0x0d, 0xdb, 0x37, 0xab, 0x47, 0x01), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_DeviceClass_ClassCoInstallers = new DEVPROPKEY() { Fmtid = new Guid(0x713d1703, 0xa2e2, 0x49f5, 0x92, 0x14, 0x56, 0x47, 0x2e, 0xf3, 0xda, 0x5c), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_DeviceInterface_FriendlyName = new DEVPROPKEY() { Fmtid = new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_DeviceInterface_Enabled = new DEVPROPKEY() { Fmtid = new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), Pid = 3 };
            public static DEVPROPKEY DEVPKEY_DeviceInterface_ClassGuid = new DEVPROPKEY() { Fmtid = new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), Pid = 4 };
            public static DEVPROPKEY DEVPKEY_DeviceInterfaceClass_DefaultInterface = new DEVPROPKEY() { Fmtid = new Guid(0x14c83a99, 0x0b3f, 0x44b7, 0xbe, 0x4c, 0xa1, 0x78, 0xd3, 0x99, 0x05, 0x64), Pid = 2 };
            public static DEVPROPKEY DEVPKEY_DeviceDisplay_IsShowInDisconnectedState = new DEVPROPKEY() { Fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), Pid = 0x44 };
            public static DEVPROPKEY DEVPKEY_DeviceDisplay_IsNotInterestingForDisplay = new DEVPROPKEY() { Fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), Pid = 0x4a };
            public static DEVPROPKEY DEVPKEY_DeviceDisplay_Category = new DEVPROPKEY() { Fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), Pid = 0x5a };
            public static DEVPROPKEY DEVPKEY_DeviceDisplay_UnpairUninstall = new DEVPROPKEY() { Fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), Pid = 0x62 };
            public static DEVPROPKEY DEVPKEY_DeviceDisplay_RequiresUninstallElevation = new DEVPROPKEY() { Fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), Pid = 0x63 };
            public static DEVPROPKEY DEVPKEY_DeviceDisplay_AlwaysShowDeviceAsConnected = new DEVPROPKEY() { Fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), Pid = 0x65 };
        }

        #endregion

        #region winnt.h

        public const Int32 REG_NONE                       = 0;   // No value type
        public const Int32 REG_SZ                         = 1;   // Unicode nul terminated string
        public const Int32 REG_EXPAND_SZ                  = 2;   // Unicode nul terminated string
                                                                 // (with environment variable references)
        public const Int32 REG_BINARY                     = 3;   // Free form binary
        public const Int32 REG_DWORD                      = 4;   // 32-bit number
        public const Int32 REG_DWORD_LITTLE_ENDIAN        = 4;   // 32-bit number (same as REG_DWORD)
        public const Int32 REG_DWORD_BIG_ENDIAN           = 5;   // 32-bit number
        public const Int32 REG_LINK                       = 6;   // Symbolic Link (unicode)
        public const Int32 REG_MULTI_SZ                   = 7;   // Multiple Unicode strings
        public const Int32 REG_RESOURCE_LIST              = 8;   // Resource list in the resource map
        public const Int32 REG_FULL_RESOURCE_DESCRIPTOR   = 9;   // Resource list in the hardware description
        public const Int32 REG_RESOURCE_REQUIREMENTS_LIST = 10;
        public const Int32 REG_QWORD                      = 11;  // 64-bit number
        public const Int32 REG_QWORD_LITTLE_ENDIAN        = 11;  // 64-bit number (same as REG_QWORD)

        #endregion
    }
}
