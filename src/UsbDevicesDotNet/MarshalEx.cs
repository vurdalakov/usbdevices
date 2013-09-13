namespace Vurdalakov.UsbDevicesDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Security.AccessControl;
    using System.Text;

    internal static class MarshalEx
    {
        public static Byte[] ReadByteArray(IntPtr source, Int32 startIndex, Int32 length)
        {
            Byte[] byteArray = new Byte[length];
            Marshal.Copy(source, byteArray, startIndex, length);
            return byteArray;
        }

        public static Byte[] ReadByteArray(IntPtr source, Int32 length)
        {
            return MarshalEx.ReadByteArray(source, 0, length);
        }

        public static Guid ReadGuid(IntPtr source, Int32 length)
        {
            Byte[] byteArray = MarshalEx.ReadByteArray(source, 0, length);
            return new Guid(byteArray);
        }

        public static String[] ReadMultiSzStringList(IntPtr source, Int32 length)
        {
            Byte[] byteArray = MarshalEx.ReadByteArray(source, 0, length);
            String multiSz = Encoding.Unicode.GetString(byteArray, 0, length);

            List<String> strings = new List<String>();
            
            Int32 start = 0;
            Int32 end = multiSz.IndexOf('\0', start);
            
            while (end > start)
            {
                strings.Add(multiSz.Substring(start, end - start));

                start = end + 1;
                end = multiSz.IndexOf('\0', start);
            }
            
            return strings.ToArray();
        }

        public static DateTime ReadFileTime(IntPtr source)
        {
            Int64 fileTime = Marshal.ReadInt64(source);
            return DateTime.FromFileTimeUtc(fileTime);
        }

        // TODO: not tested
        public static RawSecurityDescriptor ReadSecurityDescriptor(IntPtr source, Int32 length)
        {
            Byte[] byteArray = MarshalEx.ReadByteArray(source, 0, length);
            return new RawSecurityDescriptor(byteArray, 0);
        }
    }
}
