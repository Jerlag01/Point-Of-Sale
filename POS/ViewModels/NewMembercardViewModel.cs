using NLog;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Dal;
using Dal.Model;
using Pos.Helpers;
using Pos.Properties;
using Pos.Services;
using Util;
using Util.MicroMvvm;
using Util.WpfMessageBox;

namespace Pos.ViewModels
{
    public class NewMembercardViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UnitOfWork UnitOfWork;
        private readonly EidService EidService;
        private readonly PcscService PcscService;
        private MembercardService MembercardService;
        private MemberCard newMemberCard;
        private bool isReadOnly;
        private bool printCardEnabled;
        private ImageSource membercardImage;
        private string selectedMenuItem;
        private ObservableCollection<MemberCard> nfcCardQueue;
        private MemberCard selectedNfcCardQueueItem;
        private ImageSource membercardQueueImage;
        private string ScannedNfcId;
        private ObservableCollection<MemberCard> normalCardQueue;
        private MemberCard selectedNormalCardQueueItem;
        private ImageSource membercardNormalQueueImage;
        private Member selectedUser;
        private string userFilter;

        public MemberCard NewMemberCard
        {
            get
            {
                return this.newMemberCard;
            }
            set
            {
                this.newMemberCard = value;
                this.OnPropertyChanged<MemberCard>((System.Linq.Expressions.Expression<Func<MemberCard>>)(() => this.NewMemberCard));
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                this.isReadOnly = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.IsReadOnly));
            }
        }

        public bool PrintCardEnabled
        {
            get
            {
                return this.printCardEnabled;
            }
            set
            {
                this.printCardEnabled = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.PrintCardEnabled));
            }
        }

        public ImageSource MembercardImage
        {
            get
            {
                return this.membercardImage;
            }
            set
            {
                this.membercardImage = value;
                this.OnPropertyChanged<ImageSource>((System.Linq.Expressions.Expression<Func<ImageSource>>)(() => this.MembercardImage));
            }
        }

        public ObservableCollection<string> MembercardMenu { get; set; }

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

        public ObservableCollection<MemberCard> NfcCardQueue
        {
            get
            {
                return this.nfcCardQueue;
            }
            set
            {
                this.nfcCardQueue = value;
                this.OnPropertyChanged<ObservableCollection<MemberCard>>((System.Linq.Expressions.Expression<Func<ObservableCollection<MemberCard>>>)(() => this.NfcCardQueue));
            }
        }

        public MemberCard SelectedNfcCardQueueItem
        {
            get
            {
                return this.selectedNfcCardQueueItem;
            }
            set
            {
                this.selectedNfcCardQueueItem = value;
                this.OnPropertyChanged<MemberCard>((System.Linq.Expressions.Expression<Func<MemberCard>>)(() => this.SelectedNfcCardQueueItem));
            }
        }

        public ImageSource MembercardQueueImage
        {
            get
            {
                return this.membercardQueueImage;
            }
            set
            {
                this.membercardQueueImage = value;
                this.OnPropertyChanged<ImageSource>((System.Linq.Expressions.Expression<Func<ImageSource>>)(() => this.MembercardQueueImage));
            }
        }

        public ObservableCollection<MemberCard> NormalCardQueue
        {
            get
            {
                return this.normalCardQueue;
            }
            set
            {
                this.normalCardQueue = value;
                this.OnPropertyChanged<ObservableCollection<MemberCard>>((System.Linq.Expressions.Expression<Func<ObservableCollection<MemberCard>>>)(() => this.NormalCardQueue));
            }
        }

        public MemberCard SelectedNormalCardQueueItem
        {
            get
            {
                return this.selectedNormalCardQueueItem;
            }
            set
            {
                this.selectedNormalCardQueueItem = value;
                this.OnPropertyChanged<MemberCard>((System.Linq.Expressions.Expression<Func<MemberCard>>)(() => this.SelectedNormalCardQueueItem));
            }
        }

        public ImageSource MembercardNormalQueueImage
        {
            get
            {
                return this.membercardNormalQueueImage;
            }
            set
            {
                this.membercardNormalQueueImage = value;
                this.OnPropertyChanged<ImageSource>((System.Linq.Expressions.Expression<Func<ImageSource>>)(() => this.MembercardNormalQueueImage));
            }
        }

        public ObservableCollection<Member> UserList { get; set; }

        public Member SelectedUser
        {
            get
            {
                return this.selectedUser;
            }
            set
            {
                this.selectedUser = value;
                this.OnPropertyChanged<Member>((System.Linq.Expressions.Expression<Func<Member>>)(() => this.SelectedUser));
            }
        }

        public string UserFilter
        {
            get
            {
                return this.userFilter;
            }
            set
            {
                this.userFilter = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.UserFilter));
                this.FilterUser();
            }
        }

        public NewMembercardViewModel()
        {
            NewMembercardViewModel.logger.Debug("VIEWMODEL LOADING: NewMemberViewModel");
            AuthenticationService.Instance.SuspendService = false;
            this.NewMemberCard = new MemberCard();
            this.NewMemberCard.Member = new Member()
            {
                Country = "BE"
            };
            this.MembercardMenu = new ObservableCollection<string>();
            this.MembercardMenu.Add(Resources.PageNewMembercardMenuNew);
            this.MembercardMenu.Add(Resources.PageNewMembercardMenuExisting);
            this.MembercardMenu.Add(Resources.PageNewMembercardMenuPrintUnprintedNFCCards);
            this.MembercardMenu.Add(Resources.PageNewMembercardMenuPrintUnprintedNormalCards);
            this.MembercardMenu.Add(Resources.PageNewMembercardMenuChangeMemberDetails);
            this.MembercardMenu.Add(Resources.PageNewMembercardMenuLostMembercard);
            this.SelectedMenuItem = Resources.PageNewMembercardMenuNew;
            this.UnitOfWork = new UnitOfWork();
            this.EidService = new EidService()
            {
                SelectedReader = Pos.Properties.Settings.Default.EidReaderName
            };
            WeakEventManager<EidService, EventArgs>.AddHandler(this.EidService, "EidInserted", new EventHandler<EventArgs>(this.EidServiceOnEidInserted));
            this.MembercardService = new MembercardService();
            this.MembercardService.PrinterName = Pos.Properties.Settings.Default.CardPrinterName;
            this.PcscService = new PcscService(Pos.Properties.Settings.Default.NfcReaderName);
            WeakEventManager<PcscService, EventArgs>.AddHandler(this.PcscService, "CardInserted", new EventHandler<EventArgs>(this.PcscServiceOnCardInserted));
            this.PrintCardEnabled = false;
            this.UserList = new ObservableCollection<Member>();
            this.LoadUsers();
        }

        ~NewMembercardViewModel()
        {
            NewMembercardViewModel.logger.Debug("VIEWMODEL DESTROYED: NewMemberViewModel");
        }

        protected override void OnDispose()
        {
            this.EidService.EidInserted -= new EventHandler(this.EidServiceOnEidInserted);
            this.PcscService.CardInserted -= new EventHandler(this.PcscServiceOnCardInserted);
            this.PcscService.Dispose();
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
            if (this.SelectedMenuItem == Resources.PageNewMembercardMenuPrintUnprintedNFCCards)
            {
                this.NfcCardQueue = new ObservableCollection<MemberCard>();
                foreach (MemberCard unprintedActiveCard in this.UnitOfWork.MemberCardRepository.GetUnprintedActiveCards())
                    this.NfcCardQueue.Add(unprintedActiveCard);
                if (this.NfcCardQueue.Count > 0)
                {
                    this.SelectedNfcCardQueueItem = this.NfcCardQueue[0];
                    AuthenticationService.Instance.SuspendService = true;
                    this.PrintCardEnabled = true;
                }
                else
                    this.PrintCardEnabled = false;
            }
            else if (this.SelectedMenuItem == Resources.PageNewMembercardMenuPrintUnprintedNormalCards)
            {
                this.NormalCardQueue = new ObservableCollection<MemberCard>();
                foreach (MemberCard unprintedNormalCard in this.UnitOfWork.MemberCardRepository.GetUnprintedNormalCards())
                    this.NormalCardQueue.Add(unprintedNormalCard);
                if (this.NormalCardQueue.Count > 0)
                {
                    this.SelectedNormalCardQueueItem = this.NormalCardQueue[0];
                    AuthenticationService.Instance.SuspendService = true;
                    this.PrintCardEnabled = true;
                }
                else
                    this.PrintCardEnabled = false;
            }
            else if (this.SelectedMenuItem == Resources.PageNewMembercardMenuNew || this.SelectedMenuItem == Resources.PageNewMembercardMenuChangeMemberDetails)
            {
                this.MembercardService = new MembercardService();
                this.MembercardService.PrinterName = Pos.Properties.Settings.Default.CardPrinterName;
                this.NewMemberCard = new MemberCard();
                this.NewMemberCard.Member = new Member()
                {
                    Country = "BE"
                };
                this.PrintCardEnabled = false;
            }
            else
            {
                if (!(this.SelectedMenuItem == Resources.PageNewMembercardMenuExisting) && !(this.SelectedMenuItem == Resources.PageNewMembercardMenuLostMembercard))
                    return;
                this.MembercardService = new MembercardService();
                this.MembercardService.PrinterName = Pos.Properties.Settings.Default.CardPrinterName;
                this.NewMemberCard = new MemberCard();
                this.PrintCardEnabled = false;
            }
        }

        public ICommand CancelMemberCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.CancelMemberCommandExec));
            }
        }

        public ICommand GenerateMembercardCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.GenerateMembercardCommandExec));
            }
        }

        public ICommand GenerateExistingMembercardCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.GenerateExistingMembercardCommandExec));
            }
        }

        public ICommand GenerateLostMembercardCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.GenerateLostMembercardCommandExec));
            }
        }

        public ICommand ReadEidCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.ReadEidCommandExec));
            }
        }

        public ICommand PrintMemberCardCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.PrintMemberCardCommandExec));
            }
        }

        private void GenerateMembercardCommandExec()
        {
            if (string.IsNullOrWhiteSpace(this.NewMemberCard.Member.Address) || string.IsNullOrWhiteSpace(this.NewMemberCard.Member.City) || (string.IsNullOrWhiteSpace(this.NewMemberCard.Member.Country) || string.IsNullOrWhiteSpace(this.NewMemberCard.Member.Email)) || (string.IsNullOrWhiteSpace(this.NewMemberCard.Member.FirstName) || string.IsNullOrWhiteSpace(this.NewMemberCard.Member.LastName) || (string.IsNullOrWhiteSpace(this.NewMemberCard.Member.TelephoneNr) || string.IsNullOrWhiteSpace(this.NewMemberCard.Member.ZipCode))))
            {
                int num1 = (int)WPFMessageBox.Show(Resources.ValidationGeneral, Resources.ValidationGeneralTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                this.NewMemberCard.CreationTime = DateTime.Now;
                this.NewMemberCard.ExpireDate = Pos.Properties.Settings.Default.MemberCardStartDate.Add(Pos.Properties.Settings.Default.MemberCardValidityPeriod);
                string memberActive = string.Empty;
                string memberYear = string.Empty;
                if (this.NewMemberCard.ActiveMember)
                    memberActive = Resources.PdfMemberCardActiveMember;
                else
                    memberYear = Pos.Properties.Settings.Default.MemberCardStartDate.Year.ToString() + "-" + (object)Pos.Properties.Settings.Default.MemberCardStartDate.Add(Pos.Properties.Settings.Default.MemberCardValidityPeriod).Year;
                if (this.MembercardService.GenerateMembercard(this.NewMemberCard, memberYear, memberActive) == string.Empty)
                {
                    int num2 = (int)WPFMessageBox.Show(Resources.ExceptionMembercard, Resources.ExceptionMembercardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    this.MembercardImage = ImageHelper.ImageToSource(this.MembercardService.ConvertPdfToImage());
                    this.PrintCardEnabled = true;
                }
            }
        }

        private void GenerateExistingMembercardCommandExec()
        {
            Member selectedUser = this.SelectedUser;
            if ((selectedUser != null ? selectedUser.MemberCards.Where<MemberCard>((Func<MemberCard, bool>)(m => m.ActiveMember)).OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(m => m.ExpireDate)).FirstOrDefault<MemberCard>() : (MemberCard)null) != null)
            {
                int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardMembershipExtension, Resources.PageNewMembercardMembershipExtensionTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.PrintCardEnabled = true;
                this.MembercardImage = (ImageSource)null;
            }
            else
            {
                this.NewMemberCard.CreationTime = DateTime.Now;
                this.NewMemberCard.ExpireDate = Pos.Properties.Settings.Default.MemberCardStartDate.Add(Pos.Properties.Settings.Default.MemberCardValidityPeriod);
                string memberActive = string.Empty;
                string memberYear = string.Empty;
                if (this.NewMemberCard.ActiveMember)
                    memberActive = Resources.PdfMemberCardActiveMember;
                else
                    memberYear = Pos.Properties.Settings.Default.MemberCardStartDate.Year.ToString() + "-" + (object)Pos.Properties.Settings.Default.MemberCardStartDate.Add(Pos.Properties.Settings.Default.MemberCardValidityPeriod).Year;
                if (this.MembercardService.GenerateMembercard(this.NewMemberCard, memberYear, memberActive) == string.Empty)
                {
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionMembercard, Resources.ExceptionMembercardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    this.MembercardImage = ImageHelper.ImageToSource(this.MembercardService.ConvertPdfToImage());
                    this.PrintCardEnabled = true;
                }
            }
        }

        private void GenerateLostMembercardCommandExec()
        {
            Member selectedUser = this.SelectedUser;
            MemberCard memberCard = selectedUser != null ? selectedUser.MemberCards.Where<MemberCard>((Func<MemberCard, bool>)(m => m.ActiveMember)).OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(m => m.ExpireDate)).FirstOrDefault<MemberCard>() : (MemberCard)null;
            if (memberCard != null)
            {
                this.NewMemberCard.Member = memberCard.Member;
                this.NewMemberCard.CreationTime = DateTime.Now;
                this.NewMemberCard.ExpireDate = Pos.Properties.Settings.Default.MemberCardStartDate.Add(Pos.Properties.Settings.Default.MemberCardValidityPeriod);
                this.NewMemberCard.ActiveMember = true;
                if (this.MembercardService.GenerateMembercard(this.NewMemberCard, string.Empty, Resources.PdfMemberCardActiveMember) == string.Empty)
                {
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionMembercard, Resources.ExceptionMembercardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    this.MembercardImage = ImageHelper.ImageToSource(this.MembercardService.ConvertPdfToImage());
                    this.PrintCardEnabled = true;
                }
            }
            else
            {
                int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardCardLostNonActive, Resources.PageNewMembercardCardLostNonActiveTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.MembercardImage = (ImageSource)null;
            }
        }

        private void CancelMemberCommandExec()
        {
            NavigatorHelper.NavigationService.Refresh();
        }

        private void PrintMemberCardCommandExec()
        {
            if (this.SelectedMenuItem == Resources.PageNewMembercardMenuNew)
            {
                if (this.NewMemberCard.ActiveMember)
                {
                    int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardPrintActiveMember, Resources.PageNewMembercardPrintActiveMemberTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    NewMembercardViewModel.logger.Info("Card not printed because of active member: " + (object)this.NewMemberCard.Id);
                    this.NewMemberCard.Printed = false;
                    this.SaveMember();
                }
                else
                {
                    if (!this.MembercardService.PrintMembercard())
                    {
                        int num = (int)WPFMessageBox.Show(Resources.ExceptionPrintingCard, Resources.ExceptionPrintingCardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        NewMembercardViewModel.logger.Warn("There was a problem printing the membercard: " + (object)this.NewMemberCard.Id);
                        this.NewMemberCard.Printed = false;
                    }
                    else if (WPFMessageBox.Show(Resources.PageNewMembercardPrintedQuestion, Resources.PageNewMembercardPrintedQuestionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        NewMembercardViewModel.logger.Info("Membercard printed succesfully " + (object)this.NewMemberCard.Id);
                        this.NewMemberCard.Printed = true;
                    }
                    else
                    {
                        NewMembercardViewModel.logger.Warn("There was a problem printing the membercard (confirmed by user): " + (object)this.NewMemberCard.Id);
                        int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardNotPrinted, Resources.PageNewMembercardNotPrintedTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        this.NewMemberCard.Printed = false;
                    }
                    this.SaveMember();
                }
            }
            else if (this.SelectedMenuItem == Resources.PageNewMembercardMenuPrintUnprintedNormalCards)
            {
                if (!this.MembercardService.PrintMembercard())
                {
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionPrintingCard, Resources.ExceptionPrintingCardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    NewMembercardViewModel.logger.Warn("There was a problem printing the membercard: " + (object)this.NewMemberCard.Id);
                }
                else if (WPFMessageBox.Show(Resources.PageNewMembercardPrintedQuestion, Resources.PageNewMembercardPrintedQuestionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    NewMembercardViewModel.logger.Warn("There was a problem printing the membercard (confirmed by user): " + (object)this.NewMemberCard.Id);
                    int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardNotPrinted, Resources.PageNewMembercardNotPrintedTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    NewMembercardViewModel.logger.Info("Membercard printed succesfully " + (object)this.NewMemberCard.Id);
                    this.SelectedNormalCardQueueItem.Printed = true;
                    this.UpdateMember(this.selectedNormalCardQueueItem.Member);
                }
            }
            else if (this.SelectedMenuItem == Resources.PageNewMembercardMenuPrintUnprintedNFCCards)
            {
                if (!this.MembercardService.PrintMembercard())
                {
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionPrintingCard, Resources.ExceptionPrintingCardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    NewMembercardViewModel.logger.Warn("There was a problem printing the membercard: " + (object)this.NewMemberCard.Id);
                }
                else if (WPFMessageBox.Show(Resources.PageNewMembercardPrintedQuestion, Resources.PageNewMembercardPrintedQuestionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    NewMembercardViewModel.logger.Warn("There was a problem printing the membercard (confirmed by user): " + (object)this.NewMemberCard.Id);
                    int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardNotPrinted, Resources.PageNewMembercardNotPrintedTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    this.ScannedNfcId = string.Empty;
                    while (string.IsNullOrEmpty(this.ScannedNfcId))
                    {
                        if (WPFMessageBox.Show(Resources.PageNewMembercardScanNfcIdQuestion, Resources.PageNewMembercardScanNfcIdQuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Asterisk) == MessageBoxResult.Cancel)
                            return;
                        if (this.UnitOfWork.MemberCardRepository.GetBySmartcardIdNoTracking(this.ScannedNfcId) != null)
                        {
                            int num = (int)WPFMessageBox.Show(Resources.MessageDuplicateCard, Resources.MessageDuplicateCardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            NewMembercardViewModel.logger.Info("Smartcard already assigned to another user, smartcard id: " + this.ScannedNfcId);
                            this.ScannedNfcId = string.Empty;
                        }
                    }
                    NewMembercardViewModel.logger.Info("Membercard printed succesfully " + (object)this.NewMemberCard.Id);
                    this.SelectedNfcCardQueueItem.Printed = true;
                    this.SelectedNfcCardQueueItem.SmartCardId = this.ScannedNfcId;
                    this.UpdateMember(this.selectedNfcCardQueueItem.Member);
                }
            }
            else if (this.SelectedMenuItem == Resources.PageNewMembercardMenuChangeMemberDetails)
            {
                if (this.SelectedUser == null)
                    return;
                NewMembercardViewModel.logger.Info("Member details changed succesfully: " + (object)this.SelectedUser.Id);
                this.UpdateMember(this.SelectedUser);
            }
            else if (this.SelectedMenuItem == Resources.PageNewMembercardMenuExisting)
            {
                MemberCard mc = this.SelectedUser.MemberCards.Where<MemberCard>((Func<MemberCard, bool>)(m => m.ActiveMember)).OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(m => m.ExpireDate)).FirstOrDefault<MemberCard>();
                if (mc != null)
                {
                    mc.ExpireDate = Pos.Properties.Settings.Default.MemberCardStartDate.Add(Pos.Properties.Settings.Default.MemberCardValidityPeriod);
                    this.UpdateMemberCard(mc);
                }
                else if (this.NewMemberCard.ActiveMember)
                {
                    int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardPrintActiveMember, Resources.PageNewMembercardPrintActiveMemberTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    NewMembercardViewModel.logger.Info("Card not printed because of active member: " + (object)this.NewMemberCard.Id);
                    this.NewMemberCard.Printed = false;
                    this.AddMemberCard(this.NewMemberCard);
                }
                else
                {
                    if (!this.MembercardService.PrintMembercard())
                    {
                        int num = (int)WPFMessageBox.Show(Resources.ExceptionPrintingCard, Resources.ExceptionPrintingCardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        NewMembercardViewModel.logger.Warn("There was a problem printing the membercard: " + (object)this.NewMemberCard.Id);
                        this.NewMemberCard.Printed = false;
                    }
                    else if (WPFMessageBox.Show(Resources.PageNewMembercardPrintedQuestion, Resources.PageNewMembercardPrintedQuestionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        NewMembercardViewModel.logger.Info("Membercard printed succesfully " + (object)this.NewMemberCard.Id);
                        this.NewMemberCard.Printed = true;
                    }
                    else
                    {
                        NewMembercardViewModel.logger.Warn("There was a problem printing the membercard (confirmed by user): " + (object)this.NewMemberCard.Id);
                        int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardNotPrinted, Resources.PageNewMembercardNotPrintedTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        this.NewMemberCard.Printed = false;
                    }
                    this.AddMemberCard(this.NewMemberCard);
                }
            }
            else
            {
                if (!(this.SelectedMenuItem == Resources.PageNewMembercardMenuLostMembercard))
                    return;
                int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardPrintActiveMember, Resources.PageNewMembercardPrintActiveMemberTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                NewMembercardViewModel.logger.Info("Card not printed because of active member: " + (object)this.NewMemberCard.Id);
                this.NewMemberCard.Printed = false;
                this.AddMemberCard(this.NewMemberCard);
            }
        }

        private void SaveMember()
        {
            try
            {
                if (AuthenticationService.Instance.AuthenticatedMember != null)
                {
                    Member byId = this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id);
                    this.NewMemberCard.Member.CreatedBy = byId;
                    this.NewMemberCard.CreatedBy = byId;
                }
                this.UnitOfWork.MemberCardRepository.Add(this.NewMemberCard);
                this.UnitOfWork.Save();
                int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardSuccess, Resources.PageNewMembercardSuccessTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                NewMembercardViewModel.logger.Info("Member added successfully to the database: " + (object)this.NewMemberCard.Member.Id);
                NewMembercardViewModel.logger.Info("MemberCard added successfully to the database: " + (object)this.newMemberCard.Id);
                NavigatorHelper.NavigationService.Refresh();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewMembercardViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        private void UpdateMember(Member member)
        {
            try
            {
                this.UnitOfWork.MemberRepository.Update(member);
                this.UnitOfWork.Save();
                int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardUpdateSuccess, Resources.PageNewMembercardUpdateSuccessTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                NewMembercardViewModel.logger.Info("Member updated successfully: " + (object)member.Id);
                NavigatorHelper.NavigationService.Refresh();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewMembercardViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        private void UpdateMemberCard(MemberCard mc)
        {
            try
            {
                this.UnitOfWork.MemberCardRepository.Update(mc);
                this.UnitOfWork.Save();
                int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardUpdateSuccess, Resources.PageNewMembercardUpdateSuccessTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                NewMembercardViewModel.logger.Info("MemberCard updated successfully: " + (object)mc.Id);
                NavigatorHelper.NavigationService.Refresh();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewMembercardViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        private void AddMemberCard(MemberCard memberCard)
        {
            try
            {
                if (AuthenticationService.Instance.AuthenticatedMember == null)
                    return;
                Member byId = this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id);
                if (byId != null)
                    this.NewMemberCard.CreatedBy = byId;
                this.UnitOfWork.MemberCardRepository.Add(memberCard);
                this.UnitOfWork.Save();
                int num = (int)WPFMessageBox.Show(Resources.PageNewMembercardUpdateSuccess, Resources.PageNewMembercardUpdateSuccessTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                NewMembercardViewModel.logger.Info("MemberCard added successfully to existing member, card id: " + (object)memberCard.Id + ", member: " + (object)memberCard.Member.Id);
                NavigatorHelper.NavigationService.Refresh();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewMembercardViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        private void EidServiceOnEidInserted(object sender, EventArgs eventArgs)
        {
            this.ReadEidCommandExec();
        }

        private void PcscServiceOnCardInserted(object sender, EventArgs eventArgs)
        {
            this.ScannedNfcId = this.PcscService.CardId;
        }

        private void ReadEidCommandExec()
        {
            this.EidService.ReadEid();
            if (this.EidService.EidData == null || this.EidService.EidData.FirstName == null)
                return;
            this.isReadOnly = true;
            this.NewMemberCard.Member.FirstName = this.EidService.EidData.FirstName;
            this.NewMemberCard.Member.LastName = this.EidService.EidData.LastName;
            this.NewMemberCard.Member.Address = this.EidService.EidData.Address;
            this.NewMemberCard.Member.ZipCode = this.EidService.EidData.ZipCode;
            this.NewMemberCard.Member.City = this.EidService.EidData.City;
            this.NewMemberCard.Member.BirthDate = new DateTime?(this.EidService.EidData.BirthDate);
            this.NewMemberCard.Member.Country = this.EidService.EidData.CountryIsoCode;
            this.NewMemberCard.Member.Gender = this.EidService.EidData.Gender;
            this.NewMemberCard.Member.RrNumber = this.EidService.EidData.RRNumber;
            if (this.SelectedUser == null)
                return;
            this.SelectedUser = this.NewMemberCard.Member;
        }

        public ICommand SelectedNfcCardQueueItemChanged
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SelectedNfcCardQueueItemChangedExec));
            }
        }

        private void SelectedNfcCardQueueItemChangedExec()
        {
            string memberActive = string.Empty;
            if (this.SelectedNfcCardQueueItem == null)
                return;
            if (this.selectedNfcCardQueueItem.ActiveMember)
                memberActive = Resources.PdfMemberCardActiveMember;
            if (this.MembercardService.GenerateMembercard(this.selectedNfcCardQueueItem, string.Empty, memberActive) == string.Empty)
            {
                int num = (int)WPFMessageBox.Show(Resources.ExceptionMembercard, Resources.ExceptionMembercardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                this.MembercardQueueImage = ImageHelper.ImageToSource(this.MembercardService.ConvertPdfToImage());
                this.PrintCardEnabled = true;
            }
        }

        public ICommand SelectedNormalCardQueueItemChanged
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SelectedNormalCardQueueItemChangedExec));
            }
        }

        private void SelectedNormalCardQueueItemChangedExec()
        {
            DateTime dateTime = Pos.Properties.Settings.Default.MemberCardStartDate;
            // ISSUE: variable of a boxed type
            __Boxed<int> year1 = (System.ValueType)dateTime.Year;
            dateTime = Pos.Properties.Settings.Default.MemberCardStartDate;
            dateTime = dateTime.Add(Pos.Properties.Settings.Default.MemberCardValidityPeriod);
            // ISSUE: variable of a boxed type
            __Boxed<int> year2 = (System.ValueType)dateTime.Year;
            string memberYear = year1.ToString() + "-" + (object)year2;
            string memberActive = string.Empty;
            if (this.SelectedNormalCardQueueItem == null)
                return;
            if (this.selectedNormalCardQueueItem.ActiveMember)
                memberActive = Resources.PdfMemberCardActiveMember;
            if (this.MembercardService.GenerateMembercard(this.selectedNormalCardQueueItem, memberYear, memberActive) == string.Empty)
            {
                int num = (int)WPFMessageBox.Show(Resources.ExceptionMembercard, Resources.ExceptionMembercardTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                this.MembercardNormalQueueImage = ImageHelper.ImageToSource(this.MembercardService.ConvertPdfToImage());
                this.PrintCardEnabled = true;
            }
        }

        private void LoadUsers()
        {
            try
            {
                foreach (Member member in this.UnitOfWork.MemberRepository.GetAll())
                    this.UserList.Add(member);
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewMembercardViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewMembercardViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        private void FilterUser()
        {
            if (this.UserList.Count == 0 || this.UserFilter == string.Empty)
                return;
            this.SelectedUser = this.UserList.FirstOrDefault<Member>((Func<Member, bool>)(u => u.Fullname.ToLower().Contains(this.UserFilter.ToLower())));
        }

        public ICommand EditMemberUserSelectionChangedCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.EditMemberUserSelectionChangedCommandExec));
            }
        }

        private void EditMemberUserSelectionChangedCommandExec()
        {
            this.PrintCardEnabled = this.SelectedUser != null;
        }

        public ICommand ExistingMemberUserSelectionChangedCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.ExistingMemberUserSelectionChangedCommandExec));
            }
        }

        private void ExistingMemberUserSelectionChangedCommandExec()
        {
            if (this.SelectedUser != null)
                this.NewMemberCard.Member = this.SelectedUser;
            this.PrintCardEnabled = false;
        }
    }
}
