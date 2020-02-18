using System;
using System.Windows;

namespace Util.WpfMessageBox
{
    public static class WPFMessageBox
    {
        public static MessageBoxResult Show(string messageBoxText)
        {
            return WPFMessageBox.ShowCore((Window)null, messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return WPFMessageBox.ShowCore((Window)null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return WPFMessageBox.ShowCore(owner, messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          string messageBoxText,
          string caption,
          MessageBoxButton button)
        {
            return WPFMessageBox.ShowCore((Window)null, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          Window owner,
          string messageBoxText,
          string caption)
        {
            return WPFMessageBox.ShowCore(owner, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon)
        {
            return WPFMessageBox.ShowCore((Window)null, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          Window owner,
          string messageBoxText,
          string caption,
          MessageBoxButton button)
        {
            return WPFMessageBox.ShowCore(owner, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon,
          MessageBoxResult defaultResult)
        {
            return WPFMessageBox.ShowCore((Window)null, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          Window owner,
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon)
        {
            return WPFMessageBox.ShowCore(owner, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon,
          MessageBoxResult defaultResult,
          MessageBoxOptions options)
        {
            return WPFMessageBox.ShowCore((Window)null, messageBoxText, caption, button, icon, defaultResult, options);
        }

        public static MessageBoxResult Show(
          Window owner,
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon,
          MessageBoxResult defaultResult)
        {
            return WPFMessageBox.ShowCore(owner, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(
          Window owner,
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon,
          MessageBoxResult defaultResult,
          MessageBoxOptions options)
        {
            return WPFMessageBox.ShowCore(owner, messageBoxText, caption, button, icon, defaultResult, options);
        }

        private static MessageBoxResult ShowCore(
          Window owner,
          string messageBoxText,
          string caption = "",
          MessageBoxButton button = MessageBoxButton.OK,
          MessageBoxImage icon = MessageBoxImage.None,
          MessageBoxResult defaultResult = MessageBoxResult.None,
          MessageBoxOptions options = MessageBoxOptions.None)
        {
            return WPFMessageBoxWindow.Show((Action<Window>)(messageBoxWindow => messageBoxWindow.Owner = owner), messageBoxText, caption, button, icon, defaultResult, options);
        }
    }
}
