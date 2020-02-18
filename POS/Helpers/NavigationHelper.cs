// Decompiled with JetBrains decompiler
using System;
using System.Windows;
using System.Windows.Navigation;
using Pos.Properties;
using Util.WpfMessageBox;

namespace Pos.Helpers
{
    internal static class NavigatorHelper
    {
        public static NavigationService NavigationService { get; set; }

        public static void Cancel()
        {
            if (WPFMessageBox.Show(Resources.ApplicationQuitDialog, Resources.ApplicationQuitDialogTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                Environment.Exit(0);
            }
            catch
            {
            }
        }
    }
}