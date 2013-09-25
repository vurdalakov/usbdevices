namespace include2winapi
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(String[] args)
        {
            if (0 == args.Length)
            {
                Console.WriteLine(@"Usage: include2winapi <path to Windows SDK include directory>");
                Console.WriteLine(@"Examples:");
                Console.WriteLine(@"  include2winapi ""C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Include""");
                Console.WriteLine(@"  include2winapi ""C:\Program Files (x86)\Windows Kits\8.0\Include""");
                return;
            }

            String directoryName = args[0];

            String sharedDirectory = Path.Combine(directoryName, "shared");
            if (!Directory.Exists(sharedDirectory))
            {
                sharedDirectory = directoryName;
            }

            String umDirectory = Path.Combine(directoryName, "um");
            if (!Directory.Exists(umDirectory))
            {
                umDirectory = directoryName;
            }

            Program.ParseDevpkeyH(sharedDirectory);
            Program.ParseSetupapiH(umDirectory);
            Program.ParseDevpropdefH(sharedDirectory);
            Program.ParseWinntH(umDirectory);
        }

        private static void ParseDevpkeyH(String directoryName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
@"namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static partial class UsbDeviceWinApi
    {

        // devpkey.h

        public static class DevicePropertyKeys
        {
");

            using (StreamReader streamReader = new StreamReader(Path.Combine(directoryName, "devpkey.h")))
            {
                while (true)
                {
                    String line = streamReader.ReadLine();
                    if (null == line)
                    {
                        break;
                    }

                    if (!line.StartsWith("DEFINE_DEVPROPKEY(DEVPKEY_"))
                    {
                        continue;
                    }

/*
DEFINE_DEVPROPKEY(DEVPKEY_Device_DeviceDesc,             0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 2);     // DEVPROP_TYPE_STRING
*/
                    Match match = Regex.Match(line, @"DEFINE_DEVPROPKEY\(([a-z0-9_]+),\s+(.*), ((?:0x)?[0-9a-f]+)\);", RegexOptions.IgnoreCase);
                    if (match.Groups.Count != 4)
                    {
                        throw new Exception(String.Format("Line not handled:\n{0}", line));
                    }
/*
            public static DEVPROPKEY DEVPKEY_NAME = new DEVPROPKEY() { Fmtid = new Guid(0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac), Pid = 10 };
*/
                    stringBuilder.AppendFormat("            public static DEVPROPKEY {0} = new DEVPROPKEY() {{ Fmtid = new Guid({1}), Pid = {2} }};", match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                    stringBuilder.AppendLine();
                }
            }

            stringBuilder.Append(
@"        }
    }
}
");

            using (StreamWriter streamWriter = new StreamWriter("UsbDeviceWinApi.DevicePropertyKeys.cs"))
            {
                streamWriter.Write(stringBuilder);
            }
        }

        private static void ParseSetupapiH(String directoryName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
@"namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static partial class UsbDeviceWinApi
    {

        // setupapi.h

        public static class DeviceRegistryPropertyKeys
        {
");

            using (StreamReader streamReader = new StreamReader(Path.Combine(directoryName, "setupapi.h")))
            {
                while (true)
                {
                    String line = streamReader.ReadLine();
                    if (null == line)
                    {
                        break;
                    }

                    if (!line.StartsWith("#define SPDRP_"))
                    {
                        continue;
                    }

/*
#define SPDRP_CLASSGUID                   (0x00000008)  // ClassGUID (R/W)
*/
                    Match match = Regex.Match(line, @"#define ([a-z0-9_]+) +\((0x[0-9a-f]+)\)", RegexOptions.IgnoreCase);
                    if (match.Groups.Count != 3)
                    {
                        throw new Exception(String.Format("Line not handled:\n{0}", line));
                    }
/*
            public const Int32 SPDRP_CLASSGUID = 0x00000008;
*/
                    stringBuilder.AppendFormat("            public const UInt32 {0} = {1};", match.Groups[1].Value, match.Groups[2].Value);
                    stringBuilder.AppendLine();
                }
            }

            stringBuilder.Append(
@"        }
    }
}
");

            using (StreamWriter streamWriter = new StreamWriter("UsbDeviceWinApi.DeviceRegistryPropertyKeys.cs"))
            {
                streamWriter.Write(stringBuilder);
            }
        }

        private static void ParseDevpropdefH(String directoryName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
@"namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static partial class UsbDeviceWinApi
    {

        // devpropdef.h

        public static class DevicePropertyTypes
        {
");

            using (StreamReader streamReader = new StreamReader(Path.Combine(directoryName, "devpropdef.h")))
            {
                while (true)
                {
                    String line = streamReader.ReadLine();
                    if (null == line)
                    {
                        break;
                    }

                    if (!line.StartsWith("#define DEVPROP_TYPE") && !line.StartsWith("#define MAX_DEVPROP_") && !line.StartsWith("#define DEVPROP_MASK_"))
                    {
                        continue;
                    }

/*
#define DEVPROP_TYPE_UINT32                     0x00000007  // 32-bit unsigned int (ULONG)
*/
                    Match match = Regex.Match(line, @"#define ([a-z0-9_]+) +(0x[0-9a-f]+)", RegexOptions.IgnoreCase);
                    if (match.Groups.Count != 3)
                    {
                        match = Regex.Match(line, @"#define ([a-z0-9_]+) +\((.+)\)", RegexOptions.IgnoreCase);
                        if (match.Groups.Count != 3)
                        {
                            throw new Exception(String.Format("Line not handled:\n{0}", line));
                        }
                    }
/*
        public const UInt32 DEVPROP_TYPE_UINT32                     = 0x00000007;  // 32-bit unsigned int (ULONG)
*/
                    stringBuilder.AppendFormat("            public const UInt32 {0} = {1};", match.Groups[1].Value, match.Groups[2].Value);
                    stringBuilder.AppendLine();
                }
            }

            stringBuilder.Append(
@"        }
    }
}
");

            using (StreamWriter streamWriter = new StreamWriter("UsbDeviceWinApi.DevicePropertyTypes.cs"))
            {
                streamWriter.Write(stringBuilder);
            }
        }

        private static void ParseWinntH(String directoryName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
@"namespace Vurdalakov.UsbDevicesDotNet
{
    using System;

    public static partial class UsbDeviceWinApi
    {

        // winnt.h

        public static class DeviceRegistryPropertyTypes
        {
");
            Boolean started = false;
            using (StreamReader streamReader = new StreamReader(Path.Combine(directoryName, "winnt.h")))
            {
                while (true)
                {
                    String line = streamReader.ReadLine();
                    if (null == line)
                    {
                        break;
                    }

                    if (!started && !line.StartsWith("#define REG_NONE"))
                    {
                        continue;
                    }

                    started = true;

                    if (!line.StartsWith("#define REG_"))
                    {
                        continue;
                    }

/*
#define REG_DWORD                   ( 4 )   // 32-bit number
*/
                    Match match = Regex.Match(line, @"#define ([a-z0-9_]+) +\( +(\d+) +\)", RegexOptions.IgnoreCase);
                    if (match.Groups.Count != 3)
                    {
                        throw new Exception(String.Format("Line not handled:\n{0}", line));
                    }
/*
        public const Int32 REG_DWORD                      = 4;   // 32-bit number
*/
                    stringBuilder.AppendFormat("            public const Int32 {0} = {1};", match.Groups[1].Value, match.Groups[2].Value);
                    stringBuilder.AppendLine();
                }
            }

            stringBuilder.Append(
@"        }
    }
}
");

            using (StreamWriter streamWriter = new StreamWriter("UsbDeviceWinApi.DeviceRegistryPropertyTypes.cs"))
            {
                streamWriter.Write(stringBuilder);
            }
        }
    }
}
