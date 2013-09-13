namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static class Endianness
    {
        public static UInt32 Swap(UInt32 number)
        {
            return ((number & 0x000000ff) << 24) | ((number & 0x0000ff00) << 8) | ((number & 0x00ff0000) >> 8) | ((number & 0xff000000) >> 24);
        }
    }
}
