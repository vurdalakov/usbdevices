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

        public Boolean IsInterface { get; private set; }

        public ThreadSafeObservableCollection<NameValueTypeViewModel> Properties { get; private set; }

        public ThreadSafeObservableCollection<NameValueTypeViewModel> RegistryProperties { get; private set; }

        public UsbDeviceViewModel(UsbDevice usbDevice)
        {
            this.Properties = new ThreadSafeObservableCollection<NameValueTypeViewModel>();
            this.RegistryProperties = new ThreadSafeObservableCollection<NameValueTypeViewModel>();

            this.IsInterface = false;

            this.Refresh(usbDevice);
        }

        public UsbDeviceViewModel(UsbDevice usbDevice, String interfaceId) : this(usbDevice)
        {
            this.Vid = String.Empty;
            this.Pid = String.Empty;
            this.HubAndPort = String.Empty;
            this.DeviceId = interfaceId;
            this.DevicePath = String.Empty;
            this.Description = String.Empty;

            this.IsInterface = true;
        }

        public void Refresh(UsbDevice usbDevice)
        {
            this.Vid = usbDevice.Vid;
            this.OnPropertyChanged(() => this.Vid);

            this.Pid = usbDevice.Pid;
            this.OnPropertyChanged(() => this.Pid);

            this.HubAndPort = Helpers.MakeHubAndPort(usbDevice.Hub, usbDevice.Port);
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

                this.Properties.Add(new NameValueTypeViewModel(usbDeviceProperty.GetDescription(), values[0], usbDeviceProperty.GetType()));

                for (int i = 1; i < values.Length; i++)
                {
                    this.Properties.Add(new NameValueTypeViewModel(String.Empty, values[i], String.Empty));
                }
            }

            this.RegistryProperties.Clear();
            foreach (UsbDeviceRegistryProperty usbDeviceRegistryProperty in usbDevice.RegistryProperties)
            {
                String[] values = usbDeviceRegistryProperty.GetValue();

                this.RegistryProperties.Add(new NameValueTypeViewModel(usbDeviceRegistryProperty.GetDescription(), values[0], usbDeviceRegistryProperty.GetType()));

                for (int i = 1; i < values.Length; i++)
                {
                    this.RegistryProperties.Add(new NameValueTypeViewModel(String.Empty, values[i], String.Empty));
                }
            }
        }
    }
}
