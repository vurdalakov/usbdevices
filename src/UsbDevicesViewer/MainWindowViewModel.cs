namespace UsbDevicesViewer
{
    using System;
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

        public MainWindowViewModel()
        {
            this.InterfaceTypes = new ThreadSafeObservableCollection<NameValueViewModel>();
            this.InterfaceTypes.Add(new NameValueViewModel("USB Host Controllers", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HOST_CONTROLLER));
            this.InterfaceTypes.Add(new NameValueViewModel("USB Hubs", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_HUB));
            this.InterfaceTypes.Add(new NameValueViewModel("USB Devices", UsbDeviceWinApi.GUID_DEVINTERFACE_USB_DEVICE));

            this.UsbDevices = new ThreadSafeObservableCollection<UsbDeviceViewModel>();

            this.RefreshCommand = new CommandBase(this.Refresh);
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
        }
    }
}
