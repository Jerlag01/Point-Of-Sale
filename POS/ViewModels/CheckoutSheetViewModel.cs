using NLog;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Dal;
using Dal.Model;
using Pos.Helpers;
using Pos.Properties;
using Pos.Services;
using Util.MicroMvvm;
using Util.WpfMessageBox;

namespace Pos.ViewModels
{
    public class CheckoutSheetViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UnitOfWork UnitOfWork;
        private Member AuthenticatedMember;
        private CheckoutSheet checkoutSheet;
        private double subTotalEur500;
        private double subTotalEur200;
        private double subTotalEur100;
        private double subTotalEur50;
        private double subTotalEur20;
        private double subTotalEur10;
        private double subTotalEur5;
        private double subTotalEur2;
        private double subTotalEur1;
        private double subTotalEur50c;
        private double subTotalEur20c;
        private double subTotalEur10c;
        private double subTotalEur5c;
        private double subTotalEur2c;
        private double subTotalEur1c;

        public CheckoutSheet CheckoutSheet
        {
            get
            {
                return this.checkoutSheet;
            }
            set
            {
                this.checkoutSheet = value;
                this.OnPropertyChanged<CheckoutSheet>((System.Linq.Expressions.Expression<Func<CheckoutSheet>>)(() => this.CheckoutSheet));
            }
        }

        public double SubTotalEur500
        {
            get
            {
                return this.subTotalEur500;
            }
            set
            {
                this.subTotalEur500 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur500));
            }
        }

        public double SubTotalEur200
        {
            get
            {
                return this.subTotalEur200;
            }
            set
            {
                this.subTotalEur200 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur200));
            }
        }

        public double SubTotalEur100
        {
            get
            {
                return this.subTotalEur100;
            }
            set
            {
                this.subTotalEur100 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur100));
            }
        }

        public double SubTotalEur50
        {
            get
            {
                return this.subTotalEur50;
            }
            set
            {
                this.subTotalEur50 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur50));
            }
        }

        public double SubTotalEur20
        {
            get
            {
                return this.subTotalEur20;
            }
            set
            {
                this.subTotalEur20 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur20));
            }
        }

        public double SubTotalEur10
        {
            get
            {
                return this.subTotalEur10;
            }
            set
            {
                this.subTotalEur10 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur10));
            }
        }

        public double SubTotalEur5
        {
            get
            {
                return this.subTotalEur5;
            }
            set
            {
                this.subTotalEur5 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur5));
            }
        }

        public double SubTotalEur2
        {
            get
            {
                return this.subTotalEur2;
            }
            set
            {
                this.subTotalEur2 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur2));
            }
        }

        public double SubTotalEur1
        {
            get
            {
                return this.subTotalEur1;
            }
            set
            {
                this.subTotalEur1 = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur1));
            }
        }

        public double SubTotalEur50c
        {
            get
            {
                return this.subTotalEur50c;
            }
            set
            {
                this.subTotalEur50c = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur50c));
            }
        }

        public double SubTotalEur20c
        {
            get
            {
                return this.subTotalEur20c;
            }
            set
            {
                this.subTotalEur20c = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur20c));
            }
        }

        public double SubTotalEur10c
        {
            get
            {
                return this.subTotalEur10c;
            }
            set
            {
                this.subTotalEur10c = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur10c));
            }
        }

        public double SubTotalEur5c
        {
            get
            {
                return this.subTotalEur5c;
            }
            set
            {
                this.subTotalEur5c = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur5c));
            }
        }

        public double SubTotalEur2c
        {
            get
            {
                return this.subTotalEur2c;
            }
            set
            {
                this.subTotalEur2c = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur2c));
            }
        }

        public double SubTotalEur1c
        {
            get
            {
                return this.subTotalEur1c;
            }
            set
            {
                this.subTotalEur1c = value;
                this.OnPropertyChanged<double>((System.Linq.Expressions.Expression<Func<double>>)(() => this.SubTotalEur1c));
            }
        }

        public CheckoutSheetViewModel()
        {
            CheckoutSheetViewModel.logger.Debug("VIEWMODEL LOADING: CheckoutSheetViewModel");
            this.UnitOfWork = new UnitOfWork();
            if (AuthenticationService.Instance.AuthenticatedMember != null)
                this.AuthenticatedMember = this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id);
            CheckoutSheet lastOpenSheet = this.UnitOfWork.CheckoutSheetRepository.GetLastOpenSheet();
            if (lastOpenSheet == null)
            {
                this.CheckoutSheet = new CheckoutSheet()
                {
                    OpenTime = new DateTime?(DateTime.Now),
                    OpenedBy = this.AuthenticatedMember
                };
            }
            else
            {
                this.CheckoutSheet = lastOpenSheet;
                this.CheckoutSheet.CloseTime = new DateTime?(DateTime.Now);
                this.CheckoutSheet.ClosedBy = this.AuthenticatedMember;
            }
            WeakEventManager<AuthenticationService, AuthenticationService.AuthenticationEventArgs>.AddHandler(AuthenticationService.Instance, "MemberAuthenticated", new EventHandler<AuthenticationService.AuthenticationEventArgs>(this.AuthenticationEventHandler));
            PropertyChangedEventManager.AddHandler((INotifyPropertyChanged)this.CheckoutSheet, new EventHandler<PropertyChangedEventArgs>(this.CheckoutSheetPropertyChangedEventHandler), "CloseAmount");
        }

        private void CheckoutSheetPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (!(e.PropertyName == "CloseAmount"))
                return;
            this.SubTotalEur1c = (double)this.CheckoutSheet.CloseEur1c * 0.01;
            this.SubTotalEur2c = (double)this.CheckoutSheet.CloseEur2c * 0.02;
            this.SubTotalEur5c = (double)this.CheckoutSheet.CloseEur5c * 0.05;
            this.SubTotalEur10c = (double)this.CheckoutSheet.CloseEur10c * 0.1;
            this.SubTotalEur20c = (double)this.CheckoutSheet.CloseEur20c * 0.2;
            this.SubTotalEur50c = (double)this.CheckoutSheet.CloseEur50c * 0.5;
            this.SubTotalEur1 = (double)(this.CheckoutSheet.CloseEur1 * 1);
            this.SubTotalEur2 = (double)(this.CheckoutSheet.CloseEur2 * 2);
            this.SubTotalEur5 = (double)(this.CheckoutSheet.CloseEur5 * 5);
            this.SubTotalEur10 = (double)(this.CheckoutSheet.CloseEur10 * 10);
            this.SubTotalEur20 = (double)(this.CheckoutSheet.CloseEur20 * 20);
            this.SubTotalEur50 = (double)(this.CheckoutSheet.CloseEur50 * 50);
            this.SubTotalEur100 = (double)(this.CheckoutSheet.CloseEur100 * 100);
            this.SubTotalEur200 = (double)(this.CheckoutSheet.CloseEur200 * 200);
            this.SubTotalEur500 = (double)(this.CheckoutSheet.CloseEur500 * 500);
        }

        private void AuthenticationEventHandler(
          object sender,
          AuthenticationService.AuthenticationEventArgs e)
        {
            this.AuthenticatedMember = this.UnitOfWork.MemberRepository.GetById(e.AuthenticatedMember.Id);
            this.CheckoutSheet.OpenedBy = this.AuthenticatedMember;
            this.CheckoutSheet.ClosedBy = this.AuthenticatedMember;
        }

        ~CheckoutSheetViewModel()
        {
            CheckoutSheetViewModel.logger.Debug("VIEWMODEL DESTROYED: CheckoutSheetViewModel");
        }

        public ICommand OpenCheckoutSheetCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.OpenCheckoutSheetCommandExec));
            }
        }

        private void OpenCheckoutSheetCommandExec()
        {
            this.CheckoutSheet.OpenTime = new DateTime?(DateTime.Now);
            if (AuthenticationService.Instance.AuthenticatedMember != null)
            {
                this.AuthenticatedMember = this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id);
                this.CheckoutSheet.OpenedBy = this.AuthenticatedMember;
            }
            try
            {
                this.UnitOfWork.CheckoutSheetRepository.Add(this.CheckoutSheet);
                this.UnitOfWork.Save();
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                CheckoutSheetViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                CheckoutSheetViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
        }

        public ICommand CloseCheckoutSheetCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.CloseCheckoutSheetCommandExec));
            }
        }

        private void CloseCheckoutSheetCommandExec()
        {
            this.CheckoutSheet.CloseTime = new DateTime?(DateTime.Now);
            if (AuthenticationService.Instance.AuthenticatedMember != null)
            {
                this.AuthenticatedMember = this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id);
                this.CheckoutSheet.ClosedBy = this.AuthenticatedMember;
                this.CheckoutSheet.CloseTime = new DateTime?(DateTime.Now);
            }
            try
            {
                this.UnitOfWork.CheckoutSheetRepository.Update(this.CheckoutSheet);
                this.UnitOfWork.Save();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                CheckoutSheetViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService != null)
                {
                    NavigatorHelper.NavigationService.Refresh();
                    NavigatorHelper.NavigationService.RemoveBackEntry();
                }
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                CheckoutSheetViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService != null)
                {
                    NavigatorHelper.NavigationService.Refresh();
                    NavigatorHelper.NavigationService.RemoveBackEntry();
                }
            }
            catch (FormattedDbEntityValidationException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                CheckoutSheetViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService != null)
                {
                    NavigatorHelper.NavigationService.Refresh();
                    NavigatorHelper.NavigationService.RemoveBackEntry();
                }
            }
            new Thread(new ThreadStart(this.GenerateReport)).Start();
            if (NavigatorHelper.NavigationService == null)
                return;
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
            NavigatorHelper.NavigationService.RemoveBackEntry();
        }

        public ICommand CancelCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.CancelCommandExec));
            }
        }

        private void CancelCommandExec()
        {
            if (NavigatorHelper.NavigationService == null)
                return;
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
            NavigatorHelper.NavigationService.RemoveBackEntry();
        }

        private void GenerateReport()
        {
            PdfReportService pdfReportService = new PdfReportService();
            pdfReportService.GenerateCheckoutSheet(this.CheckoutSheet);
            pdfReportService.MailTo = Pos.Properties.Settings.Default.EmailToAddress;
            pdfReportService.MailFromAddress = Pos.Properties.Settings.Default.EmailFromAddress;
            pdfReportService.MailFromName = Pos.Properties.Settings.Default.EmailFromName;
            pdfReportService.MailServer = Pos.Properties.Settings.Default.EmailSmtpServer;
            pdfReportService.SendReportAsMail(Resources.MailCheckoutSheetSubject, Resources.MailCheckoutSheetContent);
        }
    }
}
