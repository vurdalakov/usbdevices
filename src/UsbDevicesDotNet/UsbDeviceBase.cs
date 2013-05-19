namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    internal class UsbDeviceBase
    {
        protected void Trace(String format, params Object[] args)
        {
            System.Diagnostics.Trace.WriteLine(String.Format(format, args));
        }

        protected void TraceError(String functionName, Int32 errorCode)
        {
            Trace("{0}() function failed with error {1}", functionName, errorCode);
        }

        protected void TraceError(String functionName)
        {
            TraceError(functionName, Marshal.GetLastWin32Error());
        }
    }
}
