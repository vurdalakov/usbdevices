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
                Console.WriteLine("{0}:{1} = VID_{2}&PID_{3} = {4}",
                    usbDevice.Hub, usbDevice.Port, usbDevice.Vid, usbDevice.Pid, usbDevice.DeviceId);

                if (!String.IsNullOrEmpty(usbDevice.BusReportedDeviceDescription))
                {
                    Console.WriteLine("{0}", usbDevice.BusReportedDeviceDescription);
                }

                Console.WriteLine();
            }
        }
    }
}
