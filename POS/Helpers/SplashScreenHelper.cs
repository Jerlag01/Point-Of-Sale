using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Pos.ViewModels;
using Pos.Views;

namespace Pos.Helpers
{
    internal static class SplashScreenHelper
    {
        public static SplashScreenView SplashScreen { private get; set; }

        public static void Show()
        {
            SplashScreenHelper.SplashScreen?.Show();
        }

        public static void Close()
        {
            if (SplashScreenHelper.SplashScreen == null)
                return;
            if (!SplashScreenHelper.SplashScreen.Dispatcher.CheckAccess())
            {
                Thread thread = new Thread((ThreadStart)(() => SplashScreenHelper.SplashScreen.Dispatcher.Invoke(DispatcherPriority.Normal, (Delegate)(() =>
                {
                    Thread.Sleep(2000);
                    SplashScreenHelper.SplashScreen.Close();
                }))));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }
            else
                SplashScreenHelper.SplashScreen.Close();
        }

        public static void CloseInstant()
        {
            if (SplashScreenHelper.SplashScreen == null)
                return;
            if (!SplashScreenHelper.SplashScreen.Dispatcher.CheckAccess())
            {
                Thread thread = new Thread((ThreadStart)(() => SplashScreenHelper.SplashScreen.Dispatcher.Invoke(DispatcherPriority.Normal, (Delegate)(() => SplashScreenHelper.SplashScreen.Close()))));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }
            else
                SplashScreenHelper.SplashScreen.Close();
        }

        public static void ShowText(string text)
        {
            if (SplashScreenHelper.SplashScreen == null)
                return;
            if (!SplashScreenHelper.SplashScreen.Dispatcher.CheckAccess())
            {
                Thread thread = new Thread((ThreadStart)(() =>
                {
                    SplashScreenHelper.SplashScreen.Dispatcher.Invoke(DispatcherPriority.Normal, (Delegate)(() => ((SplashScreenViewModel)SplashScreenHelper.SplashScreen.DataContext).SplashScreenText = text));
                    SplashScreenHelper.SplashScreen.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, (Delegate)(() => { }));
                }));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else
                ((SplashScreenViewModel)SplashScreenHelper.SplashScreen.DataContext).SplashScreenText = text;
        }
    }
}
