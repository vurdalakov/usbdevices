namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Management;
    using Microsoft.Win32;

    public class Win32UsbControllerDevices
    {
        private ManagementEventWatcher managementEventWatcherDeviceConnected;
        private ManagementEventWatcher managementEventWatcherDeviceDisconnected;
        private ManagementEventWatcher managementEventWatcherDeviceModified;

        public void StartWatcher()
        {
            try
            {
                ManagementScope managementScope = new ManagementScope("root\\CIMV2");
                managementScope.Options.EnablePrivileges = true;

                const String query = @"TargetInstance ISA 'Win32_USBControllerDevice'";

                WqlEventQuery wqlEventQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), query);
                this.managementEventWatcherDeviceConnected = new ManagementEventWatcher(managementScope, wqlEventQuery);
                this.managementEventWatcherDeviceConnected.EventArrived += this.OnDeviceConnected;
                this.managementEventWatcherDeviceConnected.Start();

                wqlEventQuery = new WqlEventQuery("__InstanceDeletionEvent", new TimeSpan(0, 0, 1), query);
                this.managementEventWatcherDeviceDisconnected = new ManagementEventWatcher(managementScope, wqlEventQuery);
                this.managementEventWatcherDeviceDisconnected.EventArrived += this.OnDeviceDisconnected;
                this.managementEventWatcherDeviceDisconnected.Start();

                wqlEventQuery = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), query);
                this.managementEventWatcherDeviceModified = new ManagementEventWatcher(managementScope, wqlEventQuery);
                this.managementEventWatcherDeviceModified.EventArrived += this.OnDeviceModified;
                this.managementEventWatcherDeviceModified.Start();
            }
            catch (Exception ex)
            {
                Tracer.Trace(ex, "Error starting WMI watcher");
            }
        }

        public void StopWatcher()
        {
            try
            {
                if (this.managementEventWatcherDeviceConnected != null)
                {
                    this.managementEventWatcherDeviceConnected.Stop();
                    this.managementEventWatcherDeviceConnected = null;
                }

                if (this.managementEventWatcherDeviceDisconnected != null)
                {
                    this.managementEventWatcherDeviceDisconnected.Stop();
                    this.managementEventWatcherDeviceDisconnected = null;
                }

                if (this.managementEventWatcherDeviceModified != null)
                {
                    this.managementEventWatcherDeviceModified.Stop();
                    this.managementEventWatcherDeviceModified = null;
                }
            }
            catch (Exception ex)
            {
                Tracer.Trace(ex, "Error stopping WMI watcher");
            }
        }

        public delegate void Win32UsbControllerDeviceEventHandler(Object sender, Win32UsbControllerDeviceEventArgs e);
        public event Win32UsbControllerDeviceEventHandler DeviceConnected;

        private void OnDeviceConnected(Object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject targetInstance = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;

                Win32UsbControllerDevice win32UsbControllerDevice = this.GetDevice(targetInstance);

                if (null == win32UsbControllerDevice)
                {
                    return;
                }

                if (this.DeviceConnected != null)
                {
                    this.DeviceConnected(this, new Win32UsbControllerDeviceEventArgs(win32UsbControllerDevice));
                }
            }
            catch (Exception ex)
            {
                Tracer.Trace(ex, "Error handling WMI creation event");
            }
        }

        public event Win32UsbControllerDeviceEventHandler DeviceDisconnected;

        private void OnDeviceDisconnected(Object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject targetInstance = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;

                Win32UsbControllerDevice win32UsbControllerDevice = this.GetDevice(targetInstance);

                if (null == win32UsbControllerDevice)
                {
                    return;
                }

                if (this.DeviceDisconnected != null)
                {
                    this.DeviceDisconnected(this, new Win32UsbControllerDeviceEventArgs(win32UsbControllerDevice));
                }
            }
            catch (Exception ex)
            {
                Tracer.Trace(ex, "Error handling WMI deletion event");
            }
        }

        public event Win32UsbControllerDeviceEventHandler DeviceModified;

        private void OnDeviceModified(Object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject targetInstance = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;

                Win32UsbControllerDevice win32UsbControllerDevice = this.GetDevice(targetInstance);

                if (null == win32UsbControllerDevice)
                {
                    return;
                }

                if (this.DeviceModified != null)
                {
                    this.DeviceModified(this, new Win32UsbControllerDeviceEventArgs(win32UsbControllerDevice));
                }
            }
            catch (Exception ex)
            {
                Tracer.Trace(ex, "Error handling WMI modification event");
            }
        }

        private Win32UsbControllerDevice GetDevice(ManagementBaseObject managementObject)
        {
            try
            {
                Win32UsbControllerDevice win32UsbControllerDevice = new Win32UsbControllerDevice();

                try
                {
                    String dependent = managementObject.GetPropertyValue("Dependent").ToString();
                    win32UsbControllerDevice.DeviceId = this.ExtractSeviceId(dependent);
                }
                catch (Exception ex)
                {
                }

                try
                {
                    String antecedent = managementObject.GetPropertyValue("Antecedent").ToString();
                    win32UsbControllerDevice.ControllerId = this.ExtractSeviceId(antecedent);
                }
                catch (Exception ex)
                {
                }

                try
                {
                    String locationInformation = this.GetLocationInformation(win32UsbControllerDevice.DeviceId);

                    Int32 hubIndex = locationInformation.IndexOf("HUB_#", StringComparison.OrdinalIgnoreCase);
                    Int32 portIndex = locationInformation.IndexOf("PORT_#", StringComparison.OrdinalIgnoreCase);

                    if ((hubIndex < 0) || (portIndex < 0))
                    {
                        throw new Exception("Wrong location information format");
                    }

                    win32UsbControllerDevice.Hub = locationInformation.Substring(hubIndex + 5, 4);
                    win32UsbControllerDevice.Port = locationInformation.Substring(portIndex + 6, 4);
                }
                catch (Exception ex)
                {
                }

                try
                {
                    Int32 vidIndex = win32UsbControllerDevice.DeviceId.IndexOf("VID", StringComparison.OrdinalIgnoreCase);
                    Int32 pidIndex = win32UsbControllerDevice.DeviceId.IndexOf("PID", StringComparison.OrdinalIgnoreCase);

                    if ((vidIndex < 0) || (pidIndex < 0))
                    {
                        throw new Exception("Wrong device ID format");
                    }

                    win32UsbControllerDevice.Vid = win32UsbControllerDevice.DeviceId.Substring(vidIndex + 4, 4);
                    win32UsbControllerDevice.Pid = win32UsbControllerDevice.DeviceId.Substring(pidIndex + 4, 4);
                }
                catch (Exception ex)
                {
                }

                return win32UsbControllerDevice;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        private String ExtractSeviceId(String instanceReference)
        {
            return instanceReference.Replace("\"", "").Split('=')[1].Replace("\\\\", "\\");
        }

        private String GetLocationInformation(String deviceId)
        {
            RegistryKey resultKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\" + deviceId);
            if (null == resultKey)
            {
                throw new Exception("Can't open Registry key");
            }

            String locationInformation = resultKey.GetValue("LocationInformation") as String;
            if (String.IsNullOrEmpty(locationInformation))
            {
                throw new Exception("Wrong Registry value format");
            }

            return locationInformation;
        }
    }
}
