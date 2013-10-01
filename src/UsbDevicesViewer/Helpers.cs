namespace UsbDevicesViewer
{
    using System;

    public static class Helpers
    {
        public static String MakeHubAndPort(String hub, String port)
        {
            return String.IsNullOrEmpty(hub) && String.IsNullOrEmpty(port) ? String.Empty : String.Format("{0}:{1}", hub, port);
        }
    }
}
