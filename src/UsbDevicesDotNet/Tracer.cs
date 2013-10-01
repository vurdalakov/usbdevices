namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static class Tracer
    {
        public static void Trace(String format, params Object[] args)
        {
            System.Diagnostics.Trace.WriteLine(String.Format(format, args));
        }

        public static void Trace(Exception ex, String format, params Object[] args)
        {
            System.Diagnostics.Trace.WriteLine(String.Format(format, args));
            System.Diagnostics.Trace.WriteLine(ex.ToString());
        }
    }
}
