using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Util.WpfMessageBox
{
    public class SystemMenuHelper
    {
        private const uint MF_BYCOMMAND = 0;
        private const uint MF_GRAYED = 1;
        private const uint MF_ENABLED = 0;
        private const uint SC_SIZE = 61440;
        private const uint SC_RESTORE = 61728;
        private const uint SC_MINIMIZE = 61472;
        private const uint SC_MAXIMIZE = 61488;
        private const uint SC_CLOSE = 61536;
        private const int WM_SHOWWINDOW = 24;
        private const int WM_CLOSE = 16;
        private HwndSource _hwndSource;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        public SystemMenuHelper(Window window)
        {
            this.AddHook(window);
        }

        public bool DisableCloseButton { get; set; }

        public bool RemoveResizeMenu { get; set; }

        private void AddHook(Window window)
        {
            if (this._hwndSource != null)
                return;
            this._hwndSource = PresentationSource.FromVisual((Visual)window) as HwndSource;
            this._hwndSource?.AddHook(new HwndSourceHook(this.hwndSourceHook));
        }

        private IntPtr hwndSourceHook(
          IntPtr hwnd,
          int msg,
          IntPtr wParam,
          IntPtr lParam,
          ref bool handled)
        {
            if (msg == 24)
            {
                IntPtr systemMenu = SystemMenuHelper.GetSystemMenu(hwnd, false);
                if (systemMenu != IntPtr.Zero)
                {
                    if (this.DisableCloseButton)
                        SystemMenuHelper.EnableMenuItem(systemMenu, 61536U, 1U);
                    if (this.RemoveResizeMenu)
                    {
                        SystemMenuHelper.RemoveMenu(systemMenu, 61728U, 0U);
                        SystemMenuHelper.RemoveMenu(systemMenu, 61440U, 0U);
                        SystemMenuHelper.RemoveMenu(systemMenu, 61472U, 0U);
                        SystemMenuHelper.RemoveMenu(systemMenu, 61488U, 0U);
                    }
                }
            }
            return IntPtr.Zero;
        }
    }
}
