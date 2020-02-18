using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Util.WpfMessageBox
{
    public static class WindowHelper
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 1;
        private const int WS_EX_RIGHT = 4096;
        private const int WS_EX_RTLREADING = 8192;
        private const int SWP_NOSIZE = 1;
        private const int SWP_NOMOVE = 2;
        private const int SWP_NOZORDER = 4;
        private const int SWP_FRAMECHANGED = 32;
        private const uint WM_SETICON = 128;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
          IntPtr hwnd,
          IntPtr hwndInsertAfter,
          int x,
          int y,
          int width,
          int height,
          uint flags);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(
          IntPtr hwnd,
          uint msg,
          IntPtr wParam,
          IntPtr lParam);

        public static void RemoveIcon(Window window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            WindowHelper.SetWindowLong(handle, -20, WindowHelper.GetWindowLong(handle, -20) | 1);
            WindowHelper.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39U);
        }

        public static void SetRightAligned(Window window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            WindowHelper.SetWindowLong(handle, -20, WindowHelper.GetWindowLong(handle, -20) | 4096);
            WindowHelper.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39U);
        }

        public static void SetRtlReading(Window window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            WindowHelper.SetWindowLong(handle, -20, WindowHelper.GetWindowLong(handle, -20) | 8192);
            WindowHelper.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39U);
        }
    }
}
