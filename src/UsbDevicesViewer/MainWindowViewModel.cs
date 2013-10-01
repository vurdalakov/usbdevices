namespace UsbDevicesViewer
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    using Vurdalakov;
    using Vurdalakov.UsbDevicesDotNet;

    public class MainWindowViewModel : ViewModelBase
    {
        public ThreadSafeObservableCollection<NameValueViewModel> InterfaceTypes { get; set; }

        private NameValueViewModel interfaceType;
        public NameValueViewModel InterfaceType
        {
            get { return this.interfaceType; }
            set
            {
                if (this.interfaceType != value)
                {
                    this.interfaceType = value;
                    this.OnPropertyChanged(() => this.InterfaceType);

                    this.Refresh();
                }
            }
        }

        public ThreadSafeObservableCollection<UsbDeviceViewModel> UsbDevices { get; private set; }

        private UsbDeviceViewModel selectedUsbDevice;
        public UsbDeviceViewModel SelectedUsbDevice
        {
            get
            {
                return this.selectedUsbDevice;
            }
            set
            {
                if (value != this.selectedUsbDevice)
                {
                    this.selectedUsbDevice = value;
                    this.OnPropertyChanged(() => this.SelectedUsbDevice);
                }
            }
        }

        private NameValueTypeViewModel selectedProperty;
        public NameValueTypeViewModel SelectedProperty
        {
            get
            {
                return this.selectedProperty;
            }
            set
            {
                if (value != this.selectedProperty)
                {
                    this.selectedProperty = value;
                    this.OnPropertyChanged(() => this.SelectedProperty);
                }
            }
        }

        private NameValueTypeViewModel selectedRegistryProperty;
        public NameValueTypeViewModel SelectedRegistryProperty
        {
            get
            {
                return this.selectedRegistryProperty;
            }
            set
            {
                if (value != this.selectedRegistryProperty)
                {
                    this.selectedRegistryProperty = value;
                    this.OnPropertyChanged(() => this.SelectedRegistryProperty);
                }
            }
        }

        private Win32UsbControllerDevices win32UsbControllerDevices = new Win32UsbControllerDevices();

        public MainWindowViewModel()
        {
            this.InterfaceTypes = new ThreadSafeObservableCollection<NameValueViewModel>();
            this.InterfaceTypes.Add(new NameValueViewModel("USB Host Controllers", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HOST_CONTROLLER));
            this.InterfaceTypes.Add(new NameValueViewModel("USB Hubs", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HUB));
            this.InterfaceTypes.Add(new NameValueViewModel("USB Devices", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_DEVICE));
            this.InterfaceTypes.Add(new NameValueViewModel("Windows Phone 1", "0FD3B15C-D457-45d8-A779-C2B2C9F9D0FD"));

            this.UsbDevices = new ThreadSafeObservableCollection<UsbDeviceViewModel>();

            this.WmiEvents = new ThreadSafeObservableCollection<WmiEvent>();

            this.RefreshCommand = new CommandBase(this.OnRefreshCommand);

            this.CopyCommand = new CommandBase<String>(this.OnCopyCommand);
            this.ClearWmiEventsCommand = new CommandBase<String>(this.OnClearWmiEventsCommand);

            this.win32UsbControllerDevices.DeviceConnected += OnWin32UsbControllerDevicesDeviceConnected;
            this.win32UsbControllerDevices.DeviceDisconnected += OnWin32UsbControllerDevicesDeviceDisconnected;
            this.win32UsbControllerDevices.DeviceModified += OnWin32UsbControllerDevicesDeviceModified;
            this.EnableWmiWatcher(true);
        }

        #region WMI events

        private void EnableWmiWatcher(Boolean enable)
        {
            if (enable)
            {
                this.win32UsbControllerDevices.StartWatcher();
            }
            else
            {
                this.win32UsbControllerDevices.StopWatcher();
            }
        }

        private Boolean refreshListOnWmiEvents = true;
        public Boolean RefreshListOnWmiEvents
        {
            get
            {
                return this.refreshListOnWmiEvents;
            }
            set
            {
                if (value != this.refreshListOnWmiEvents)
                {
                    this.refreshListOnWmiEvents = value;
                    this.OnPropertyChanged(() => this.RefreshListOnWmiEvents);
                }
            }
        }

        private void OnWin32UsbControllerDevicesDeviceConnected(Object sender, Win32UsbControllerDeviceEventArgs e)
        {
            this.WmiEvents.Insert(0, new WmiEvent(0, e.Device));

            this.RefreshOnWmiEvent(e.Device);
        }

        private void OnWin32UsbControllerDevicesDeviceDisconnected(Object sender, Win32UsbControllerDeviceEventArgs e)
        {
            this.WmiEvents.Insert(0, new WmiEvent(1, e.Device));

            this.RefreshOnWmiEvent(e.Device);
        }

        private void OnWin32UsbControllerDevicesDeviceModified(object sender, Win32UsbControllerDeviceEventArgs e)
        {
            this.WmiEvents.Insert(0, new WmiEvent(2, e.Device));

            this.RefreshOnWmiEvent(e.Device);
        }

        private void RefreshOnWmiEvent(Win32UsbControllerDevice win32UsbControllerDevice)
        {
            if (this.RefreshListOnWmiEvents && !String.IsNullOrEmpty(win32UsbControllerDevice.DeviceId) && (win32UsbControllerDevice.DeviceId.IndexOf("&MI_", StringComparison.CurrentCultureIgnoreCase) < 0))
            {
                this.Refresh();
            }
        }

        public ThreadSafeObservableCollection<WmiEvent> WmiEvents { get; set; }

        public ICommand ClearWmiEventsCommand { get; private set; }
        public void OnClearWmiEventsCommand(String source)
        {
            this.WmiEvents.Clear();
        }

        #endregion

        public ICommand RefreshCommand { get; private set; }
        public void OnRefreshCommand()
        {
            this.Refresh();
        }

        public void Refresh(String deviceId = null)
        {
            if (null == this.InterfaceType)
            {
                this.InterfaceType = this.InterfaceTypes[2];
                return;
            }

            if (String.IsNullOrEmpty(deviceId) && (this.SelectedUsbDevice != null))
            {
                deviceId = this.SelectedUsbDevice.DeviceId;
            }

            this.UsbDevices.Clear();

            UsbDevice[] usbDevices = UsbDevice.GetDevices(new Guid(this.InterfaceType.Value as String));

            List<UsbDeviceViewModel> usbDeviceViewModels = new List<UsbDeviceViewModel>();
            foreach (UsbDevice usbDevice in usbDevices)
            {
                usbDeviceViewModels.Add(new UsbDeviceViewModel(usbDevice));

                if (this.ShowDeviceInterfaces)
                {
                    foreach (String interfaceId in usbDevice.InterfaceIds)
                    {
                        usbDeviceViewModels.Add(new UsbDeviceViewModel(usbDevice, interfaceId));
                    }
                }
            }

            this.UsbDevices.AddRange(usbDeviceViewModels);

            if (!String.IsNullOrEmpty(deviceId))
            {
                foreach (UsbDeviceViewModel usbDeviceViewModel in this.UsbDevices)
                {
                    if (usbDeviceViewModel.DeviceId.Equals(deviceId, StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.SelectedUsbDevice = usbDeviceViewModel;
                        return;
                    }
                }
            }
            
            if (this.UsbDevices.Count > 0)
            {
                this.SelectedUsbDevice = this.UsbDevices[0];
            }
        }

        public ICommand CopyCommand { get; private set; }
        public void OnCopyCommand(String source)
        {
            switch (source)
            {
                case "1001":
                    Clipboard.SetText(this.SelectedUsbDevice.Vid);
                    break;
                case "1002":
                    Clipboard.SetText(this.SelectedUsbDevice.Pid);
                    break;
                case "1003":
                    Clipboard.SetText(this.SelectedUsbDevice.HubAndPort);
                    break;
                case "1004":
                    Clipboard.SetText(this.SelectedUsbDevice.Description);
                    break;
                case "1005":
                    Clipboard.SetText(this.SelectedUsbDevice.DeviceId);
                    break;
                case "1006":
                    Clipboard.SetText(this.SelectedUsbDevice.DevicePath);
                    break;
                case "2001":
                    Clipboard.SetText(this.SelectedProperty.Name);
                    break;
                case "2002":
                    Clipboard.SetText(this.SelectedProperty.Type as String);
                    break;
                case "2003":
                    Clipboard.SetText(this.SelectedProperty.Value as String);
                    break;
                case "3001":
                    Clipboard.SetText(this.SelectedRegistryProperty.Name);
                    break;
                case "3002":
                    Clipboard.SetText(this.SelectedRegistryProperty.Type as String);
                    break;
                case "3003":
                    Clipboard.SetText(this.SelectedRegistryProperty.Value as String);
                    break;
            }
        }


        private Boolean showDeviceInterfaces = false;
        public Boolean ShowDeviceInterfaces
        {
            get
            {
                return this.showDeviceInterfaces;
            }
            set
            {
                if (value != this.showDeviceInterfaces)
                {
                    this.showDeviceInterfaces = value;
                    this.OnPropertyChanged(() => this.ShowDeviceInterfaces);

                    this.Refresh();
                }
            }
        }
    }
}
