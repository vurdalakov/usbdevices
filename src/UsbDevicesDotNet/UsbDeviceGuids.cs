namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff545972%28v=vs.85%29.aspx
    public static class UsbDeviceGuids
    {
        // The GUID_DEVINTERFACE_USB_DEVICE device interface class is defined for USB devices that are attached to a USB hub.
        public const String GUID_DEVINTERFACE_USB_DEVICE = "A5DCBF10-6530-11D2-901F-00C04FB951ED";

        // The GUID_DEVINTERFACE_USB_HOST_CONTROLLER device interface class is defined for USB host controller devices. 
        public const String GUID_DEVINTERFACE_USB_HOST_CONTROLLER = "3ABF6F2D-71C4-462A-8A92-1E6861E6AF27";

        // The GUID_DEVINTERFACE_USB_HUB device interface class is defined for USB hub devices. 
        public const String GUID_DEVINTERFACE_USB_HUB = "F18A0E88-C30C-11D0-8815-00A0C906BED8";
    }
}
