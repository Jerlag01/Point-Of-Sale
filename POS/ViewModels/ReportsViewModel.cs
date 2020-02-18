using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    public class ReportsViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UnitOfWork UnitOfWork;
        private PdfReportService PdfReportService;
        private string selectedMenuItem;
        private DateTime filterDate;
        private ReportsViewModel.Filter filterActiveMember;
        private ReportsViewModel.Filter filterExpiredMember;
        private string memberFilter;
        private Member selectedMember;
        private string pdfPreviewPath;
        private CheckoutSheet selectedSheetDailyOverview;
        private CheckoutSheet selectedSheetTicketOverview;

        public ObservableCollection<string> ReportsMenu { get; set; }

        public ObservableCollection<CheckoutSheet> SheetFilterList { get; set; }

        public ObservableCollection<Member> MemberFilterList { get; set; }

        public string SelectedMenuItem
        {
            get
            {
                return this.selectedMenuItem;
            }
            set
            {
                this.selectedMenuItem = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.SelectedMenuItem));
            }
        }

        public DateTime FilterDate
        {
            get
            {
                return this.filterDate;
            }
            set
            {
                this.filterDate = value;
                this.OnPropertyChanged<DateTime>((System.Linq.Expressions.Expression<Func<DateTime>>)(() => this.FilterDate));
                if (!(this.SelectedMenuItem == Resources.PageReportsMenuDailyOverview) && !(this.SelectedMenuItem == Resources.PageReportsMenuTicketOverview))
                    return;
                this.SheetFilterList.Clear();
                foreach (CheckoutSheet checkoutSheet in this.UnitOfWork.CheckoutSheetRepository.GetByCloseDate(this.FilterDate.Date))
                    this.SheetFilterList.Add(checkoutSheet);
            }
        }

        public ReportsViewModel.Filter FilterActiveMember
        {
            get
            {
                return this.filterActiveMember;
            }
            set
            {
                this.filterActiveMember = value;
                this.OnPropertyChanged<ReportsViewModel.Filter>((System.Linq.Expressions.Expression<Func<ReportsViewModel.Filter>>)(() => this.FilterActiveMember));
            }
        }

        public ReportsViewModel.Filter FilterExpiredMember
        {
            get
            {
                return this.filterExpiredMember;
            }
            set
            {
                this.filterExpiredMember = value;
                this.OnPropertyChanged<ReportsViewModel.Filter>((System.Linq.Expressions.Expression<Func<ReportsViewModel.Filter>>)(() => this.FilterExpiredMember));
            }
        }

        public string MemberFilter
        {
            get
            {
                return this.memberFilter;
            }
            set
            {
                this.memberFilter = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.MemberFilter));
                this.FilterMember();
            }
        }

        public Member SelectedMember
        {
            get
            {
                return this.selectedMember;
            }
            set
            {
                this.selectedMember = value;
                this.OnPropertyChanged<Member>((System.Linq.Expressions.Expression<Func<Member>>)(() => this.SelectedMember));
            }
        }

        public string PdfPreviewPath
        {
            get
            {
                return this.pdfPreviewPath;
            }
            set
            {
                this.pdfPreviewPath = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.PdfPreviewPath));
            }
        }

        public CheckoutSheet SelectedSheetDailyOverview
        {
            get
            {
                return this.selectedSheetDailyOverview;
            }
            set
            {
                this.selectedSheetDailyOverview = value;
                this.OnPropertyChanged<CheckoutSheet>((System.Linq.Expressions.Expression<Func<CheckoutSheet>>)(() => this.SelectedSheetDailyOverview));
            }
        }

        public CheckoutSheet SelectedSheetTicketOverview
        {
            get
            {
                return this.selectedSheetTicketOverview;
            }
            set
            {
                this.selectedSheetTicketOverview = value;
                this.OnPropertyChanged<CheckoutSheet>((System.Linq.Expressions.Expression<Func<CheckoutSheet>>)(() => this.SelectedSheetTicketOverview));
            }
        }

        public ReportsViewModel()
        {
            ReportsViewModel.logger.Debug("VIEWMODEL LOADING: ReportsViewModel");
            AuthenticationService.Instance.SuspendService = false;
            this.ReportsMenu = new ObservableCollection<string>();
            this.ReportsMenu.Add(Resources.PageReportsMenuDailyOverview);
            this.ReportsMenu.Add(Resources.PageReportsMenuTicketOverview);
            this.ReportsMenu.Add(Resources.PageReportsMenuMemberList);
            this.ReportsMenu.Add(Resources.PageReportsMenuMemberListCards);
            this.SelectedMenuItem = Resources.PageReportsMenuDailyOverview;
            this.UnitOfWork = new UnitOfWork();
            this.PdfReportService = new PdfReportService();
            this.SheetFilterList = new ObservableCollection<CheckoutSheet>();
            this.MemberFilterList = new ObservableCollection<Member>();
            this.LoadUsers();
            this.FilterDate = DateTime.Now;
        }

        ~ReportsViewModel()
        {
            ReportsViewModel.logger.Debug("VIEWMODEL DESTROYED: ReportsViewModel");
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
            this.PdfReportService = new PdfReportService();
            this.PdfPreviewPath = (string)null;
        }

        public ICommand SelectedSheetDailyOverviewChangedCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SelectedSheetDailyOverviewChangedCommandExec));
            }
        }

        private void SelectedSheetDailyOverviewChangedCommandExec()
        {
            if (this.SelectedSheetDailyOverview == null)
                return;
            this.PdfReportService.GenerateCheckoutSheet(this.SelectedSheetDailyOverview);
            if (string.IsNullOrEmpty(this.PdfReportService.FilePath))
                return;
            this.PdfPreviewPath = this.PdfReportService.FilePath;
        }

        public ICommand SelectedSheetTicketOverviewChangedCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SelectedSheetTicketOverviewChangedCommandExec));
            }
        }

        private void SelectedSheetTicketOverviewChangedCommandExec()
        {
            if (this.SelectedSheetTicketOverview == null)
                return;
            this.PdfReportService.GenerateCheckoutTicketOverview(this.SelectedSheetTicketOverview);
            if (string.IsNullOrEmpty(this.PdfReportService.FilePath))
                return;
            this.PdfPreviewPath = this.PdfReportService.FilePath;
        }

        public ICommand GenerateReportCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.GenerateReportCommandExec));
            }
        }

        private void GenerateReportCommandExec()
        {
            if (this.SelectedMenuItem == Resources.PageReportsMenuMemberList)
            {
                List<Member> source = this.UnitOfWork.MemberRepository.GetAll(this.FilterDate);
                if (this.FilterActiveMember == ReportsViewModel.Filter.Yes)
                    source = source.Where<Member>((Func<Member, bool>)(m => m.MemberCards.Count > 0)).Where<Member>((Func<Member, bool>)(m =>
                    {
                        MemberCard memberCard = m.MemberCards.OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(c => c.CreationTime)).FirstOrDefault<MemberCard>();
                        return memberCard != null && memberCard.ActiveMember;
                    })).ToList<Member>();
                else if (this.FilterActiveMember == ReportsViewModel.Filter.No)
                    source = source.Where<Member>((Func<Member, bool>)(m => m.MemberCards.Count > 0)).Where<Member>((Func<Member, bool>)(m =>
                    {
                        MemberCard memberCard = m.MemberCards.OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(c => c.CreationTime)).FirstOrDefault<MemberCard>();
                        return memberCard != null && !memberCard.ActiveMember;
                    })).ToList<Member>();
                if (this.FilterExpiredMember == ReportsViewModel.Filter.Yes)
                    source = source.Where<Member>((Func<Member, bool>)(m => m.MemberCards.Count<MemberCard>((Func<MemberCard, bool>)(mc => mc.ExpireDate >= this.FilterDate)) == 0)).ToList<Member>();
                else if (this.FilterExpiredMember == ReportsViewModel.Filter.No)
                    source = source.Where<Member>((Func<Member, bool>)(m => m.MemberCards.Count<MemberCard>((Func<MemberCard, bool>)(mc => mc.ExpireDate >= this.FilterDate)) > 0)).Where<Member>((Func<Member, bool>)(m => m.MemberCards.Count<MemberCard>((Func<MemberCard, bool>)(mc => mc.CreationTime <= this.FilterDate)) > 0)).ToList<Member>();
                List<Member> list = source.OrderBy<Member, string>((Func<Member, string>)(m => m.LastName)).ThenBy<Member, string>((Func<Member, string>)(m => m.FirstName)).ToList<Member>();
                if (list.Count > 0)
                {
                    this.PdfReportService.GenerateMemberList(list, this.FilterDate);
                    if (string.IsNullOrEmpty(this.PdfReportService.FilePath))
                        return;
                    this.PdfPreviewPath = this.PdfReportService.FilePath;
                }
                else
                {
                    int num = (int)WPFMessageBox.Show(Resources.PageReportsNoMemberResults, Resources.PageReportsNoMemberResultsTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            else
            {
                if (!(this.SelectedMenuItem == Resources.PageReportsMenuMemberListCards) || this.SelectedMember == null)
                    return;
                if (this.SelectedMember.FirstName == "*")
                    this.PdfReportService.GenerateMemberCardList(this.UnitOfWork.MemberRepository.GetAll().OrderBy<Member, string>((Func<Member, string>)(m => m.LastName)).ThenBy<Member, string>((Func<Member, string>)(m => m.FirstName)).ToList<Member>());
                else
                    this.PdfReportService.GenerateMemberCardList(new List<Member>()
          {
            this.SelectedMember
          });
                if (string.IsNullOrEmpty(this.PdfReportService.FilePath))
                    return;
                this.PdfPreviewPath = this.PdfReportService.FilePath;
            }
        }

        public ICommand SaveReportCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SaveReportCommandExec));
            }
        }

        private void SaveReportCommandExec()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF File|*.pdf";
            saveFileDialog1.Title = "Export report";
            if (this.SelectedMenuItem == Resources.PageReportsMenuDailyOverview)
            {
                SaveFileDialog saveFileDialog2 = saveFileDialog1;
                DateTime dateTime = this.FilterDate;
                dateTime = dateTime.Date;
                string str = "Kasblad " + dateTime.ToString("yyyy_MM_dd");
                saveFileDialog2.FileName = str;
            }
            else if (this.SelectedMenuItem == Resources.PageReportsMenuTicketOverview)
            {
                SaveFileDialog saveFileDialog2 = saveFileDialog1;
                DateTime dateTime = this.FilterDate;
                dateTime = dateTime.Date;
                string str = "Ticket overzicht " + dateTime.ToString("yyyy_MM_dd");
                saveFileDialog2.FileName = str;
            }
            else if (this.SelectedMenuItem == Resources.PageReportsMenuMemberList)
            {
                SaveFileDialog saveFileDialog2 = saveFileDialog1;
                DateTime dateTime = this.FilterDate;
                dateTime = dateTime.Date;
                string str = "Ledenlijst " + dateTime.ToString("yyyy_MM_dd");
                saveFileDialog2.FileName = str;
            }
            else if (this.SelectedMenuItem == Resources.PageReportsMenuMemberListCards)
            {
                SaveFileDialog saveFileDialog2 = saveFileDialog1;
                DateTime dateTime = this.FilterDate;
                dateTime = dateTime.Date;
                string str = "Lidkaarten " + dateTime.ToString("yyyy_MM_dd");
                saveFileDialog2.FileName = str;
            }
            bool? nullable = saveFileDialog1.ShowDialog();
            bool flag = true;
            if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return;
            this.PdfReportService.SaveFile(saveFileDialog1.FileName);
        }

        private void FilterMember()
        {
            if (this.MemberFilterList.Count == 0 || this.MemberFilter == string.Empty)
                return;
            this.SelectedMember = this.MemberFilterList.FirstOrDefault<Member>((Func<Member, bool>)(u => u.Fullname.ToLower().Contains(this.MemberFilter.ToLower())));
        }

        private void LoadUsers()
        {
            try
            {
                this.MemberFilterList.Add(new Member()
                {
                    FirstName = "*"
                });
                foreach (Member member in this.UnitOfWork.MemberRepository.GetAll())
                    this.MemberFilterList.Add(member);
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ReportsViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ReportsViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        public enum Filter
        {
            Any,
            Yes,
            No,
        }
    }
}
