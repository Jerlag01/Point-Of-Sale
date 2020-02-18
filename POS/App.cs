using NLog;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Dal;
using Dal.Model;
using Pos.Helpers;
using Pos.Properties;
using Pos.Services;
using Pos.ViewModels;
using Pos.Views;
using Util.WpfMessageBox;

namespace Pos
{
    public partial class App : MediaTypeNames.Application, ISingleInstanceApp, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string Unique = "POSInstance";
        private bool _contentLoaded;

        [STAThread]
        public static void Main()
        {
            Thread thread1 = new Thread((ThreadStart)(() =>
            {
                SplashScreenHelper.SplashScreen = new SplashScreenView();
                SplashScreenHelper.Show();
                Dispatcher.Run();
            }));
            thread1.SetApartmentState(ApartmentState.STA);
            thread1.IsBackground = true;
            thread1.Start();
            Thread thread2 = new Thread((ThreadStart)(() =>
            {
                LockScreenHelper.LockScreen = new LockScreenView();
                Dispatcher.Run();
            }));
            thread2.SetApartmentState(ApartmentState.STA);
            thread2.IsBackground = true;
            thread2.Start();
            if (!SingleInstance<App>.InitializeAsFirstInstance("POSInstance"))
                return;
            App app = new App();
            app.InitializeComponent();
            app.Run();
            SingleInstance<App>.Cleanup();
        }

        public App()
        {
            this.InitializeComponent();
            MediaTypeNames.Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            if (Pos.Properties.Settings.Default.UpdateSettings)
            {
                Pos.Properties.Settings.Default.Upgrade();
                Pos.Properties.Settings.Default.UpdateSettings = false;
                Pos.Properties.Settings.Default.Save();
            }
            Dal.Settings.ConnectionString = new SqlConnectionStringBuilder()
            {
                Password = Pos.Properties.Settings.Default.DbPassword,
                UserID = Pos.Properties.Settings.Default.DbUser,
                InitialCatalog = Pos.Properties.Settings.Default.DbCatalog,
                DataSource = Pos.Properties.Settings.Default.DbInstance
            }.ToString();
            MainWindowView mainWindowView = new MainWindowView();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            mainWindowView.DataContext = (object)mainWindowViewModel;
            MediaTypeNames.Application.Current.MainWindow = (Window)mainWindowView;
            NavigatorHelper.NavigationService = mainWindowView.MainFrame.NavigationService;
            mainWindowView.Show();
            mainWindowView.Visibility = Visibility.Hidden;
            if (!string.IsNullOrEmpty(Pos.Properties.Settings.Default.SerialPortCashDrawer))
            {
                CashDrawerService.ComPortName = Pos.Properties.Settings.Default.SerialPortCashDrawer;
                if (!CashDrawerService.Connect())
                {
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionSerial, Resources.ExceptionSerialTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            try
            {
                new UnitOfWork().CategoryRepository.GetAll();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                App.logger.Error("Database error: {0}", ex.Message);
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/Settings/SettingsPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
                return;
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                App.logger.Error("Database error: {0}", ex.Message);
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/Settings/SettingsPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
                return;
            }
            catch (InvalidOperationException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                App.logger.Error("Database error: {0}", ex.Message);
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/Settings/SettingsPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
                return;
            }
            if (string.IsNullOrEmpty(Pos.Properties.Settings.Default.NfcReaderName))
            {
                LockScreenHelper.Hide();
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/Settings/SettingsPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
            else
            {
                AuthenticationService.Instance.Inititialize(new PcscService(Pos.Properties.Settings.Default.NfcReaderName), Pos.Properties.Settings.Default.AuthenticationTimeout);
                AuthenticationService.Instance.MemberAuthenticated += (AuthenticationService.AuthenticationEventHandler)((o, e) =>
                {
                    if (!e.AuthenticatedMember.Roles.Any<Role>())
                        return;
                    LockScreenHelper.Hide();
                });
                AuthenticationService.Instance.MemberDeauthenticated += (AuthenticationService.AuthenticationEventHandler)delegate
                {
                    LockScreenHelper.Show();
                };
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add((TraceListener)new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Listeners.Add((TraceListener)new App.DebugTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
            base.OnStartup(e);
            CultureInfo cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.IetfLanguageTag);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), (PropertyMetadata)new FrameworkPropertyMetadata((object)XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            CashDrawerService.Disconnect();
        }

        private void App_DispatcherUnhandledException(
          object sender,
          DispatcherUnhandledExceptionEventArgs e)
        {
            App.logger.Fatal("UNEXPECTED EXCEPTION:" + e.Exception.Message + (object)e.Exception.InnerException);
            CashDrawerService.Disconnect();
            if (WPFMessageBox.Show(Resources.ExceptionGeneral + e.Exception.Message, Resources.ExceptionGeneralTitle, MessageBoxButton.OK, MessageBoxImage.Hand) == MessageBoxResult.OK)
            {
                e.Handled = true;
                MediaTypeNames.Application.Current.Shutdown();
            }
            e.Handled = true;
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            int num = (int)WPFMessageBox.Show(Resources.ApplicationAlreadyRunning, Resources.ApplicationAlreadyRunningTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
            return true;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            MediaTypeNames.Application.LoadComponent((object)this, new Uri("/Pos;component/app.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        public class DebugTraceListener : TraceListener
        {
            public override void Write(string message)
            {
            }

            public override void WriteLine(string message)
            {
            }
        }
    }
}
