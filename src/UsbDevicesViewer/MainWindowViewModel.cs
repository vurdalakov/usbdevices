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
        private Int32 selectedDeviceType;
        public Int32 SelectedDeviceType
        {
            get { return this.selectedDeviceType; }
            set
            {
                if (this.selectedDeviceType != value)
                {
                    this.selectedDeviceType = value;
                    this.OnPropertyChanged(() => this.SelectedDeviceType);
                }

                switch (this.selectedDeviceType)
                {
                    case 0:
                        this.SelectedDevice = this.SelectedUsbDevice;
                        break;
                    case 1:
                        this.SelectedDevice = this.SelectedUsbHub;
                        break;
                    case 2:
                        this.SelectedDevice = this.SelectedUsbHostController;
                        break;
                }

                this.PropertiesHeight = 3 == this.selectedDeviceType ? "0" : "3*";
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

                    this.SelectedDevice = this.selectedUsbDevice;
                }
            }
        }

        public ThreadSafeObservableCollection<UsbDeviceViewModel> UsbHubs { get; private set; }

        private UsbDeviceViewModel selectedUsbHub;
        public UsbDeviceViewModel SelectedUsbHub
        {
            get
            {
                return this.selectedUsbHub;
            }
            set
            {
                if (value != this.selectedUsbHub)
                {
                    this.selectedUsbHub = value;
                    this.OnPropertyChanged(() => this.SelectedUsbHub);

                    this.SelectedDevice = this.selectedUsbHub;
                }
            }
        }

        public ThreadSafeObservableCollection<UsbDeviceViewModel> UsbHostControllers { get; private set; }

        private UsbDeviceViewModel selectedUsbHostController;
        public UsbDeviceViewModel SelectedUsbHostController
        {
            get
            {
                return this.selectedUsbHostController;
            }
            set
            {
                if (value != this.selectedUsbHostController)
                {
                    this.selectedUsbHostController = value;
                    this.OnPropertyChanged(() => this.SelectedUsbHostController);

                    this.SelectedDevice = this.selectedUsbHostController;
                }
            }
        }

        private UsbDeviceViewModel selectedDevice;
        public UsbDeviceViewModel SelectedDevice
        {
            get
            {
                return this.selectedDevice;
            }
            set
            {
                if (value != this.selectedDevice)
                {
                    this.selectedDevice = value;
                    this.OnPropertyChanged(() => this.SelectedDevice);
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
            this.UsbDevices = new ThreadSafeObservableCollection<UsbDeviceViewModel>();
            this.UsbHubs = new ThreadSafeObservableCollection<UsbDeviceViewModel>();
            this.UsbHostControllers = new ThreadSafeObservableCollection<UsbDeviceViewModel>();

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

        private Boolean selectConnectedDevice = true;
        public Boolean SelectConnectedDevice
        {
            get
            {
                return this.selectConnectedDevice;
            }
            set
            {
                if (value != this.selectConnectedDevice)
                {
                    this.selectConnectedDevice = value;
                    this.OnPropertyChanged(() => this.SelectConnectedDevice);
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
                this.Refresh(this.SelectConnectedDevice ? win32UsbControllerDevice.DeviceId : null);
            }
        }

        public ThreadSafeObservableCollection<WmiEvent> WmiEvents { get; set; }

        private WmiEvent selectedWmiEvent;
        public WmiEvent SelectedWmiEvent
        {
            get
            {
                return this.selectedWmiEvent;
            }
            set
            {
                if (value != this.selectedWmiEvent)
                {
                    this.selectedWmiEvent = value;
                    this.OnPropertyChanged(() => this.SelectedWmiEvent);
                }
            }
        }

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

        public String Summary { get; private set; }

        public void Refresh(String deviceId = null)
        {
            this.Refresh(UsbDeviceWinApi.GUID_DEVINTERFACE_USB_DEVICE, this.UsbDevices, this.SelectedUsbDevice, d => this.SelectedUsbDevice = d, deviceId);
            this.Refresh(UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HUB, this.UsbHubs, this.SelectedUsbHub, d => this.SelectedUsbHub = d);
            this.Refresh(UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HOST_CONTROLLER, this.UsbHostControllers, this.SelectedUsbHostController, d => this.SelectedUsbHostController = d);

            if (null == this.SelectedDevice)
            {
                this.SelectedDeviceType = this.SelectedDeviceType;
            }

            this.Summary = String.Format("{0:N0} USB devices, {1:N0} USB hubs and {2:N0} USB host controllers", this.UsbDevices.Count, this.UsbHubs.Count, this.UsbHostControllers.Count);
            this.OnPropertyChanged(() => this.Summary);
        }

        private void Refresh(String guid, ThreadSafeObservableCollection<UsbDeviceViewModel> deviceList, UsbDeviceViewModel selectedDevice, Action<UsbDeviceViewModel> setSelectedDevice, String deviceId = null)
        {
            if (String.IsNullOrEmpty(deviceId))
            {
                if (selectedDevice != null)
                {
                    deviceId = selectedDevice.DeviceId;
                }
                else if (deviceList.Count > 0)
                {
                    deviceId = deviceList[0].DeviceId;
                }
            }

            deviceList.Clear();

            UsbDevice[] usbDevices = UsbDevice.GetDevices(new Guid(guid));

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

            deviceList.AddRange(usbDeviceViewModels);

            if (!String.IsNullOrEmpty(deviceId))
            {
                foreach (UsbDeviceViewModel usbDeviceViewModel in deviceList)
                {
                    if (usbDeviceViewModel.DeviceId.Equals(deviceId, StringComparison.CurrentCultureIgnoreCase))
                    {
                        setSelectedDevice(usbDeviceViewModel);
                        return;
                    }
                }
            }

            if (deviceList.Count > 0)
            {
                setSelectedDevice(deviceList[0]);
            }
        }

        public ICommand CopyCommand { get; private set; }
        public void OnCopyCommand(String source)
        {
            try // too lazy to check for null
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
                    case "1101":
                        Clipboard.SetText(this.SelectedUsbHub.Vid);
                        break;
                    case "1102":
                        Clipboard.SetText(this.SelectedUsbHub.Pid);
                        break;
                    case "1103":
                        Clipboard.SetText(this.SelectedUsbHub.HubAndPort);
                        break;
                    case "1104":
                        Clipboard.SetText(this.SelectedUsbHub.Description);
                        break;
                    case "1105":
                        Clipboard.SetText(this.SelectedUsbHub.DeviceId);
                        break;
                    case "1106":
                        Clipboard.SetText(this.SelectedUsbHub.DevicePath);
                        break;
                    case "1204":
                        Clipboard.SetText(this.SelectedUsbHostController.Description);
                        break;
                    case "1205":
                        Clipboard.SetText(this.SelectedUsbHostController.DeviceId);
                        break;
                    case "1206":
                        Clipboard.SetText(this.SelectedUsbHostController.DevicePath);
                        break;
                    case "1301":
                        Clipboard.SetText(this.SelectedWmiEvent.Time.ToString("HH:mm:ss.ffff"));
                        break;
                    case "1302":
                        String[] eventTypes = { "Connected", "Disconnected", "Modified" };
                        Clipboard.SetText(eventTypes[this.SelectedWmiEvent.EventType]);
                        break;
                    case "1303":
                        Clipboard.SetText(this.SelectedWmiEvent.Device.Vid);
                        break;
                    case "1304":
                        Clipboard.SetText(this.SelectedWmiEvent.Device.Pid);
                        break;
                    case "1305":
                        Clipboard.SetText(this.SelectedWmiEvent.HubAndPort);
                        break;
                    case "1306":
                        Clipboard.SetText(this.SelectedWmiEvent.Device.DeviceId);
                        break;
                    case "1307":
                        Clipboard.SetText(this.SelectedWmiEvent.Device.ControllerId);
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
            catch { }
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

        private String propertiesHeight = "3*";
        public String PropertiesHeight
        {
            get { return this.propertiesHeight; }
            set
            {
                if (this.propertiesHeight != value)
                {
                    this.propertiesHeight = value;
                    this.OnPropertyChanged(() => this.PropertiesHeight);
                }
            }
        }
    }
}
