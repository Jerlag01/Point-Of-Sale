using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Util.WpfMessageBox
{
    internal static class IconUtilities
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(this Icon icon)
        {
            IntPtr hbitmap = icon.ToBitmap().GetHbitmap();
            ImageSource sourceFromHbitmap = (ImageSource)System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            if (!IconUtilities.DeleteObject(hbitmap))
                throw new Win32Exception();
            return sourceFromHbitmap;
        }
    }
}