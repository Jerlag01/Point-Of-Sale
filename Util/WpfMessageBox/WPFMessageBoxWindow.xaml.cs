using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Util.WpfMessageBox
{
    public partial class WPFMessageBoxWindow : Window, IComponentConnector
    {
        private MessageBoxViewModel _viewModel;
        [ThreadStatic]
        private static WPFMessageBoxWindow _messageBoxWindow;
        private bool _contentLoaded;

        public WPFMessageBoxWindow()
        {
            this.InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        public static MessageBoxResult Show(
          Action<Window> setOwner,
          string messageBoxText,
          string caption,
          MessageBoxButton button,
          MessageBoxImage icon,
          MessageBoxResult defaultResult,
          MessageBoxOptions options)
        {
            if ((options & MessageBoxOptions.DefaultDesktopOnly) == MessageBoxOptions.DefaultDesktopOnly)
                throw new NotImplementedException();
            if ((options & MessageBoxOptions.ServiceNotification) == MessageBoxOptions.ServiceNotification)
                throw new NotImplementedException();
            WPFMessageBoxWindow._messageBoxWindow = new WPFMessageBoxWindow();
            setOwner((Window)WPFMessageBoxWindow._messageBoxWindow);
            WPFMessageBoxWindow.PlayMessageBeep(icon);
            WPFMessageBoxWindow._messageBoxWindow._viewModel = new MessageBoxViewModel(WPFMessageBoxWindow._messageBoxWindow, caption, messageBoxText, button, icon, defaultResult, options);
            WPFMessageBoxWindow._messageBoxWindow.DataContext = (object)WPFMessageBoxWindow._messageBoxWindow._viewModel;
            WPFMessageBoxWindow._messageBoxWindow.ShowDialog();
            return WPFMessageBoxWindow._messageBoxWindow._viewModel.Result;
        }

        private static void PlayMessageBeep(MessageBoxImage icon)
        {
            switch (icon)
            {
                case MessageBoxImage.Hand:
                    SystemSounds.Hand.Play();
                    break;
                case MessageBoxImage.Question:
                    SystemSounds.Question.Play();
                    break;
                case MessageBoxImage.Exclamation:
                    SystemSounds.Exclamation.Play();
                    break;
                case MessageBoxImage.Asterisk:
                    SystemSounds.Asterisk.Play();
                    break;
                default:
                    SystemSounds.Beep.Play();
                    break;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            WindowHelper.RemoveIcon((Window)this);
            switch (this._viewModel.Options)
            {
                case MessageBoxOptions.RightAlign:
                    WindowHelper.SetRightAligned((Window)this);
                    break;
            }
            SystemMenuHelper systemMenuHelper = new SystemMenuHelper((Window)this);
            if (this._viewModel.ButtonOption == MessageBoxButton.YesNo)
                systemMenuHelper.DisableCloseButton = true;
            systemMenuHelper.RemoveResizeMenu = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
                return;
            this._viewModel.EscapeCommand.Execute((object)null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this._viewModel.CloseCommand.Execute((object)null);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Tjok.Util.WpfMessageBox;component/wpfmessageboxwindow.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
                ((UIElement)target).KeyDown += new KeyEventHandler(this.Window_KeyDown);
            else
                this._contentLoaded = true;
        }
    }
}
