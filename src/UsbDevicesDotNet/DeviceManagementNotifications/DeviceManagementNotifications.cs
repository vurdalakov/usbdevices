namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Runtime.InteropServices;

    public class DeviceManagementNotifications : MessageOnlyWindow
    {
        private IntPtr notificationHandle = IntPtr.Zero;

        public Boolean Start(Guid classGuid)
        {
            if (!this.CreateWindow())
            {
                return false;
            }
            
            DEV_BROADCAST_DEVICEINTERFACE dbi = new DEV_BROADCAST_DEVICEINTERFACE
            {
                dbch_devicetype = DBT_DEVTYP_DEVICEINTERFACE,
                dbch_reserved = 0,
                dbcc_classguid = classGuid,
                dbcc_name = ""
            };

            dbi.dbch_size = (UInt32)Marshal.SizeOf(dbi);
            IntPtr buffer = Marshal.AllocHGlobal((int)dbi.dbch_size);
            Marshal.StructureToPtr(dbi, buffer, true);

            this.notificationHandle = RegisterDeviceNotification(this.WindowHandle, buffer, 0);
            if (IntPtr.Zero == this.notificationHandle)
            {
                Tracer.Trace("RegisterDeviceNotification failed with error {0}", Marshal.GetLastWin32Error());
                return false;
            }

            return true;
        }

        public void Stop()
        {
            if (this.notificationHandle != IntPtr.Zero)
            {
                if (!UnregisterDeviceNotification(this.notificationHandle))
                {
                    Tracer.Trace("UnregisterDeviceNotification failed with error {0}", Marshal.GetLastWin32Error());
                }

                this.notificationHandle = IntPtr.Zero;
            }

            this.DestroyWindow();
        }

        protected override IntPtr OnWindowProc(IntPtr hWnd, UInt32 msg, UIntPtr wParam, IntPtr lParam)
        {
            if (WM_DEVICECHANGE == msg)
            {
                this.OnWmDeviceChange(wParam, lParam);
            }

            return base.OnWindowProc(hWnd, msg, wParam, lParam);
        }

        public delegate void DeviceManagementNotificationsEventHandler(Object sender, DeviceManagementNotificationsEventArgs e);
        public event DeviceManagementNotificationsEventHandler DeviceConnected;
        public event DeviceManagementNotificationsEventHandler DeviceDisconnected;

        private void OnWmDeviceChange(UIntPtr wParam, IntPtr lParam)
        {
            DEV_BROADCAST_HDR dbh = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_HDR));
            if (dbh.dbch_devicetype != DBT_DEVTYP_DEVICEINTERFACE)
            {
                return;
            }

            DEV_BROADCAST_DEVICEINTERFACE dbi = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_DEVICEINTERFACE));

            UInt32 eventType = wParam.ToUInt32();

            if (DBT_DEVICEARRIVAL == eventType)
            {
                if (this.DeviceConnected != null)
                {
                    this.DeviceConnected(this, new DeviceManagementNotificationsEventArgs(true, dbi.dbcc_classguid, dbi.dbcc_name));
                }
            }
            else if (DBT_DEVICEREMOVECOMPLETE == eventType)
            {
                if (this.DeviceDisconnected != null)
                {
                    this.DeviceDisconnected(this, new DeviceManagementNotificationsEventArgs(false, dbi.dbcc_classguid, dbi.dbcc_name));
                }
            }
        }

        public const UInt32 DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        public const UInt32 DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        public const UInt32 DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, UInt32 flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean UnregisterDeviceNotification(IntPtr handle);

        public const UInt32 DBT_DEVTYP_OEM = 0x00000000;
        public const UInt32 DBT_DEVTYP_VOLUME = 0x00000002;
        public const UInt32 DBT_DEVTYP_PORT = 0x00000003;
        public const UInt32 DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;
        public const UInt32 DBT_DEVTYP_HANDLE = 0x00000006;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HDR
        {
            public UInt32 dbch_size;
            public UInt32 dbch_devicetype;
            public UInt32 dbch_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public UInt32 dbch_size;
            public UInt32 dbch_devicetype;
            public UInt32 dbch_reserved;
            public Guid dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 261)]
            public String dbcc_name;
        }

        public const UInt32 WM_DEVICECHANGE = 0x0219;

        public const UInt32 DBT_DEVICEARRIVAL = 0x8000;
        public const UInt32 DBT_DEVICEREMOVECOMPLETE = 0x8004;
    }
}
