namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Runtime.InteropServices;

    public static partial class UsbDeviceWinApi
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
        // DEVPROPKEY structure
        //

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVPROPKEY
        {
            public Guid Fmtid;
            public UInt32 Pid;
        }

        #endregion

    }
}
