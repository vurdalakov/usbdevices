namespace UsbDevicesViewer
{
    using System;

    using Vurdalakov;
    using Vurdalakov.UsbDevicesDotNet;

    public class MainWindowViewModel : ViewModelBase
    {
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
            this.UsbDevices = new ThreadSafeObservableCollection<UsbDeviceViewModel>();
        }

        public void Refresh()
        {
            UsbDevice[] usbDevices = UsbDevice.GetDevices();

            this.UsbDevices.Clear();

            foreach (UsbDevice usbDevice in usbDevices)
            {
                this.UsbDevices.Add(new UsbDeviceViewModel(usbDevice));
            }
        }
    }
}
