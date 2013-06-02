namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    class Program
    {
        static void Main(String[] args)
        {
            UsbDevice[] usbDevices = UsbDevice.GetDevices(new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"));

            Console.WriteLine("{0} USB devices:\n", usbDevices.Length);

            foreach (UsbDevice usbDevice in usbDevices)
            {
                Console.WriteLine("VID_{0}&PID_{1}\nHub:Port = {2}:{3}\nDevice ID    = {4}",
                    usbDevice.Vid, usbDevice.Pid, usbDevice.Hub, usbDevice.Port, usbDevice.DeviceId);

                foreach (String interfaceId in usbDevice.InterfaceIds)
                {
                    Console.WriteLine("Interface ID = {0}", interfaceId);
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
