namespace Vurdalakov
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public static class WindowExtensions
    {
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;

        private const int WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_SIZEBOX = 0x00040000;

        private const int WS_EX_DLGMODALFRAME = 0x00000001;

        //private const int SWP_NOSIZE = 0x00000001;
        //private const int SWP_NOMOVE = 0x00000002;
        //private const int SWP_NOZORDER = 0x00000004;
        //private const int SWP_FRAMECHANGED = 0x00000020;

        //private const uint WM_SETICON = 0x0080;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        //[DllImport("user32.dll")]
        //private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, UInt32 flags);

        //[DllImport("user32.dll")]
        //static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static void SetDialogStyle(this Window window)
        {
            window.SourceInitialized += (s, e) =>
            {
                var windowHandle = new WindowInteropHelper(window).Handle;

                var windowStyle = GetWindowLong(windowHandle, GWL_STYLE);
                SetWindowLong(windowHandle, GWL_STYLE, (windowStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX & ~WS_SIZEBOX));

                windowStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
                SetWindowLong(windowHandle, GWL_EXSTYLE, (windowStyle | WS_EX_DLGMODALFRAME));

                //SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER | SWP_FRAMECHANGED);

                //SendMessage(windowHandle, WM_SETICON, IntPtr.Zero, IntPtr.Zero);
            };
        }
    }
}
