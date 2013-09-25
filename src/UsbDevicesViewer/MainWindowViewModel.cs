namespace UsbDevicesViewer
{
    using System;
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

        public MainWindowViewModel()
        {
            this.InterfaceTypes = new ThreadSafeObservableCollection<NameValueViewModel>();
            this.InterfaceTypes.Add(new NameValueViewModel("USB Host Controllers", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HOST_CONTROLLER));
            this.InterfaceTypes.Add(new NameValueViewModel("USB Hubs", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HUB));
            this.InterfaceTypes.Add(new NameValueViewModel("USB Devices", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_DEVICE));

            this.UsbDevices = new ThreadSafeObservableCollection<UsbDeviceViewModel>();

            this.RefreshCommand = new CommandBase(this.Refresh);

            this.CopyCommand = new CommandBase<String>(this.OnCopyCommand);
        }

        public ICommand RefreshCommand { get; private set; }

        public void Refresh()
        {
            this.UsbDevices.Clear();

            if (null == this.InterfaceType)
            {
                this.InterfaceType = this.InterfaceTypes[2];
                return;
            }

            UsbDevice[] usbDevices = UsbDevice.GetDevices(new Guid(this.InterfaceType.Value as String));

            foreach (UsbDevice usbDevice in usbDevices)
            {
                this.UsbDevices.Add(new UsbDeviceViewModel(usbDevice));
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
    }
}
