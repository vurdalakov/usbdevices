namespace UsbDevicesViewer
{
    using System;

    using Vurdalakov;
    using Vurdalakov.UsbDevicesDotNet;

    public class UsbDeviceViewModel : ViewModelBase
    {
        public String Vid { get; private set; }
        public String Pid { get; private set; }
        public String HubAndPort { get; private set; }
        public String DeviceId { get; private set; }
        public String DevicePath { get; private set; }
        public String Description { get; private set; }

        public ThreadSafeObservableCollection<NameValueViewModel> Properties { get; private set; }

        public ThreadSafeObservableCollection<NameValueViewModel> RegistryProperties { get; private set; }

        public UsbDeviceViewModel(UsbDevice usbDevice)
        {
            this.Properties = new ThreadSafeObservableCollection<NameValueViewModel>();
            this.RegistryProperties = new ThreadSafeObservableCollection<NameValueViewModel>();

            this.Refresh(usbDevice);
        }

        public void Refresh(UsbDevice usbDevice)
        {
            this.Vid = usbDevice.Vid;
            this.OnPropertyChanged(() => this.Vid);

            this.Pid = usbDevice.Pid;
            this.OnPropertyChanged(() => this.Pid);

            this.HubAndPort = String.Format("{0}:{1}", usbDevice.Hub, usbDevice.Port);
            this.OnPropertyChanged(() => this.HubAndPort);

            this.DeviceId = usbDevice.DeviceId;
            this.OnPropertyChanged(() => this.DeviceId);

            this.DevicePath = usbDevice.DevicePath;
            this.OnPropertyChanged(() => this.DevicePath);

            this.Description = usbDevice.BusReportedDeviceDescription;
            this.OnPropertyChanged(() => this.Description);

            this.Properties.Clear();
            foreach (UsbDeviceProperty usbDeviceProperty in usbDevice.Properties)
            {
                String[] values = usbDeviceProperty.GetValues();

                this.Properties.Add(new NameValueViewModel(usbDeviceProperty.GetDescription(), values[0]));

                for (int i = 1; i < values.Length; i++)
                {
                    this.Properties.Add(new NameValueViewModel(String.Empty, values[i]));
                }
            }

            this.RegistryProperties.Clear();
            foreach (UsbDeviceRegistryProperty usbDeviceRegistryProperty in usbDevice.RegistryProperties)
            {
                String[] values = usbDeviceRegistryProperty.GetValue();
                
                this.RegistryProperties.Add(new NameValueViewModel(usbDeviceRegistryProperty.GetDescription(), values[0]));

                for (int i = 1; i < values.Length; i++)
                {
                    this.RegistryProperties.Add(new NameValueViewModel(String.Empty, values[i]));
                }
            }
        }
    }
}
