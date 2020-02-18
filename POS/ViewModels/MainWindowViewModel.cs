// Decompiled with JetBrains decompiler
// Type: Tjok.Pos.ViewModels.MainWindowViewModel
// Assembly: Tjok.Pos, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43E441DF-D6D1-4015-9AD5-20E2185C44FE
// Assembly location: D:\lagae\Documents\POS Tjok\SDP Technologies\Kassasysteem\Tjok.Pos.exe

using NLog;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dal.Model;
using Pos.Helpers;
using Pos.Services;
using Util.MicroMvvm;

namespace Pos.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Member authenticatedMember;
        private Visibility newTicketButtonVisible;
        private Visibility newMembercardButtonVisible;
        private Visibility openDrawerButtonVisible;
        private Visibility configurationButtonVisible;
        private Visibility reportButtonVisible;
        private Visibility settingsButtonVisible;

        public Member AuthenticatedMember
        {
            get
            {
                return this.authenticatedMember;
            }
            set
            {
                this.authenticatedMember = value;
                this.OnPropertyChanged<Member>((System.Linq.Expressions.Expression<Func<Member>>)(() => this.AuthenticatedMember));
            }
        }

        public Visibility NewTicketButtonVisible
        {
            get
            {
                return this.newTicketButtonVisible;
            }
            set
            {
                this.newTicketButtonVisible = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.NewTicketButtonVisible));
            }
        }

        public Visibility NewMembercardButtonVisible
        {
            get
            {
                return this.newMembercardButtonVisible;
            }
            set
            {
                this.newMembercardButtonVisible = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.NewMembercardButtonVisible));
            }
        }

        public Visibility OpenDrawerButtonVisible
        {
            get
            {
                return this.openDrawerButtonVisible;
            }
            set
            {
                this.openDrawerButtonVisible = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.OpenDrawerButtonVisible));
            }
        }

        public Visibility ConfigurationButtonVisible
        {
            get
            {
                return this.configurationButtonVisible;
            }
            set
            {
                this.configurationButtonVisible = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.ConfigurationButtonVisible));
            }
        }

        public Visibility ReportButtonVisible
        {
            get
            {
                return this.reportButtonVisible;
            }
            set
            {
                this.reportButtonVisible = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.ReportButtonVisible));
            }
        }

        public Visibility SettingsButtonVisible
        {
            get
            {
                return this.settingsButtonVisible;
            }
            set
            {
                this.settingsButtonVisible = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.SettingsButtonVisible));
            }
        }

        public MainWindowViewModel()
        {
            MainWindowViewModel.logger.Debug("VIEWMODEL LOADING: MainWindowViewModel");
            AuthenticationService.Instance.MemberAuthenticated += (AuthenticationService.AuthenticationEventHandler)((sender, args) =>
            {
                this.AuthenticatedMember = args.AuthenticatedMember;
                this.NewTicketButtonVisible = Visibility.Hidden;
                this.ConfigurationButtonVisible = Visibility.Hidden;
                this.NewMembercardButtonVisible = Visibility.Hidden;
                this.OpenDrawerButtonVisible = Visibility.Hidden;
                this.ReportButtonVisible = Visibility.Hidden;
                this.SettingsButtonVisible = Visibility.Hidden;
                if (this.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "Admin")) > 0)
                {
                    this.NewTicketButtonVisible = Visibility.Visible;
                    this.ConfigurationButtonVisible = Visibility.Visible;
                    this.NewMembercardButtonVisible = Visibility.Visible;
                    this.OpenDrawerButtonVisible = Visibility.Visible;
                    this.ReportButtonVisible = Visibility.Visible;
                    this.SettingsButtonVisible = Visibility.Visible;
                }
                if (this.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "ProductAdmin")) > 0)
                {
                    this.NewTicketButtonVisible = Visibility.Visible;
                    this.ConfigurationButtonVisible = Visibility.Visible;
                }
                if (this.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "MemberAdmin")) > 0)
                {
                    this.NewTicketButtonVisible = Visibility.Visible;
                    this.NewMembercardButtonVisible = Visibility.Visible;
                    this.OpenDrawerButtonVisible = Visibility.Visible;
                }
                if (this.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "Bartender")) <= 0)
                    return;
                this.NewTicketButtonVisible = Visibility.Visible;
                this.OpenDrawerButtonVisible = Visibility.Visible;
            });
            AuthenticationService.Instance.MemberDeauthenticated += (AuthenticationService.AuthenticationEventHandler)delegate
            {
                this.AuthenticatedMember = (Member)null;
                this.NewTicketButtonVisible = Visibility.Hidden;
                this.ConfigurationButtonVisible = Visibility.Hidden;
                this.NewMembercardButtonVisible = Visibility.Hidden;
                this.OpenDrawerButtonVisible = Visibility.Hidden;
                this.ReportButtonVisible = Visibility.Hidden;
                this.SettingsButtonVisible = Visibility.Hidden;
            };
        }

        ~MainWindowViewModel()
        {
            MainWindowViewModel.logger.Debug("VIEWMODEL DESTROYED: MainWindowViewModel");
        }

        public ICommand ExitApplication
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.ExitApplicationExec));
            }
        }

        private void ExitApplicationExec()
        {
            NavigatorHelper.Cancel();
        }

        public ICommand NavigateNewTicketPage
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.NavigateNewTicketPageExec));
            }
        }

        private void NavigateNewTicketPageExec()
        {
            if (!(NavigatorHelper.NavigationService.CurrentSource != new Uri("Views/NewTicket/NewCheckoutSheetPageView.xaml", UriKind.Relative)))
                return;
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
        }

        public ICommand NavigateNewMemberPage
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.NavigateNewMemberPageExec));
            }
        }

        private void NavigateNewMemberPageExec()
        {
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewMembercard/NewMembercardPageView.xaml", UriKind.Relative));
        }

        public ICommand NavigateConfigurationPage
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.NavigateConfigurationPageExec));
            }
        }

        private void NavigateConfigurationPageExec()
        {
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/Configuration/ConfigurationPageView.xaml", UriKind.Relative));
        }

        public ICommand NavigateReportingPage
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.NavigateReportingPageExec));
            }
        }

        private void NavigateReportingPageExec()
        {
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/Reports/ReportsPageView.xaml", UriKind.Relative));
        }

        public ICommand NavigateSettingsPage
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.NavigateSettingsPageExec));
            }
        }

        private void NavigateSettingsPageExec()
        {
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/Settings/SettingsPageView.xaml", UriKind.Relative));
        }

        public ICommand OpenCashDrawer
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.OpenCashDrawerExec));
            }
        }

        private void OpenCashDrawerExec()
        {
            CashDrawerService.OpenDrawer();
        }
    }
}
