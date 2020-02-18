using System;
using System.Threading;
using System.Windows.Threading;
using Pos.Views;

namespace Pos.Helpers
{
    internal static class LockScreenHelper
    {
        public static LockScreenView LockScreen { private get; set; }

        public static void Show()
        {
            if (LockScreenHelper.LockScreen == null)
                return;
            if (!LockScreenHelper.LockScreen.Dispatcher.CheckAccess())
            {
                Thread thread = new Thread((ThreadStart)(() => LockScreenHelper.LockScreen.Dispatcher.Invoke(DispatcherPriority.Normal, (Delegate)(() => LockScreenHelper.LockScreen.Show()))));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else
                LockScreenHelper.LockScreen.Close();
        }

        public static void Hide()
        {
            if (LockScreenHelper.LockScreen == null)
                return;
            if (!LockScreenHelper.LockScreen.Dispatcher.CheckAccess())
            {
                Thread thread = new Thread((ThreadStart)(() => LockScreenHelper.LockScreen.Dispatcher.Invoke(DispatcherPriority.Normal, (Delegate)(() => LockScreenHelper.LockScreen.Hide()))));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else
                LockScreenHelper.LockScreen.Close();
        }
    }
}