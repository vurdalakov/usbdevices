namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    class Program
    {
        static void Main(String[] args)
        {
            UsbDevice[] usbDevices = UsbDevice.GetDevices();

            Console.WriteLine("{0} USB devices:\n", usbDevices.Length);

            foreach (UsbDevice usbDevice in usbDevices)
            {
                Console.WriteLine("VID_{0}&PID_{1}", usbDevice.Vid, usbDevice.Pid);
                Console.WriteLine("Hub:Port = {0}:{1}", usbDevice.Hub, usbDevice.Port);
                Console.WriteLine("Device ID     = {0}", usbDevice.DeviceId);

                foreach (String interfaceId in usbDevice.InterfaceIds)
                {
                    Console.WriteLine("Interface ID  = {0}", interfaceId);
                }

                Console.WriteLine("LocationInformation = {0}", usbDevice.GetRegistryPropertyValue(UsbDeviceWinApi.DeviceRegistryPropertyKeys.SPDRP_LOCATION_INFORMATION));

                foreach (String locationPath in usbDevice.GetRegistryPropertyValue(UsbDeviceWinApi.DeviceRegistryPropertyKeys.SPDRP_LOCATION_PATHS) as String[])
                {
                    Console.WriteLine("LocationPaths = {0}", locationPath);
                }

                if (!String.IsNullOrEmpty(usbDevice.BusReportedDeviceDescription))
                {
                    Console.WriteLine("DeviceDescription = {0}", usbDevice.BusReportedDeviceDescription);
                }

                Console.WriteLine();
            }
        }
    }
}
