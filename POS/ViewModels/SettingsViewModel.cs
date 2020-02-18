using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using Pos.Helpers;
using Pos.Properties;
using Pos.Services;
using Util.MicroMvvm;

namespace Pos.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private string selectedMenuItem;
        private int timeSpan;
        private string dbPassword;

        public ObservableCollection<string> SettingsMenu { get; set; }

        public ObservableCollection<string> SerialPorts { get; set; }

        public ObservableCollection<string> Printers { get; set; }

        public ObservableCollection<string> CardReaders { get; set; }

        public string SelectedMenuItem
        {
            get
            {
                return this.selectedMenuItem;
            }
            set
            {
                this.selectedMenuItem = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.SelectedMenuItem));
            }
        }

        public int TimeSpan
        {
            get
            {
                return this.timeSpan;
            }
            set
            {
                this.timeSpan = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.TimeSpan));
            }
        }

        public string DbPassword
        {
            get
            {
                return this.dbPassword;
            }
            set
            {
                this.dbPassword = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.DbPassword));
            }
        }

        public SettingsViewModel()
        {
            SettingsViewModel.logger.Debug("VIEWMODEL LOADING: SettingsViewModel");
            AuthenticationService.Instance.SuspendService = false;
            this.SettingsMenu = new ObservableCollection<string>();
            this.SettingsMenu.Add(Resources.PageSettingsMenuDbConfig);
            this.SettingsMenu.Add(Resources.PageSettingsMenuApplicationConfig);
            this.SettingsMenu.Add(Resources.PageSettingsMenuIOConfig);
            this.SettingsMenu.Add(Resources.PageSettingsMenuMailConfig);
            this.SettingsMenu.Add(Resources.PageSettingsMenuMembercardConfig);
            this.SelectedMenuItem = Resources.PageSettingsMenuDbConfig;
            this.SerialPorts = new ObservableCollection<string>(CashDrawerService.SerialPorts);
            this.CardReaders = new ObservableCollection<string>(EidService.ReaderList);
            this.Printers = new ObservableCollection<string>(Util.Util.SystemPrinters);
            this.TimeSpan = Settings.Default.MemberCardValidityPeriod.Days;
            SplashScreenHelper.Close();
        }

        ~SettingsViewModel()
        {
            SettingsViewModel.logger.Debug("VIEWMODEL DESTROYED: SettingsViewModel");
        }

        public ICommand SelectedMenuItemChanged
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SelectedMenuItemChangedExec));
            }
        }

        private void SelectedMenuItemChangedExec()
        {
        }

        public ICommand SaveSettingsCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SaveSettingsCommandExec));
            }
        }

        private void SaveSettingsCommandExec()
        {
            if (!string.IsNullOrEmpty(this.DbPassword))
                Settings.Default.DbPassword = this.DbPassword;
            Settings.Default.MemberCardValidityPeriod = new System.TimeSpan(this.TimeSpan, 0, 0, 0);
            Settings.Default.Save();
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
        }
    }
}
