using NLog;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dal;
using Dal.Model;
using Pos.Helpers;
using Pos.Properties;
using Pos.Services;
using Pos.Views.NewTicket;
using Util.MicroMvvm;
using Util.WpfMessageBox;

namespace Pos.ViewModels
{
    public class PaymentViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UnitOfWork UnitOfWork;
        private TicketViewModel currentTicket;
        private bool canDeleteOrder;
        private string keypadEntry;
        private TransactionViewModel newTransaction;
        private Transaction.PaymentMethod paymentMethodUsed;
        private CashTransaction newCashTransaction;
        private FreeTransaction newFreeTransaction;
        private NfcTransaction newNfcTransaction;
        private CoinTransaction newCoinTransaction;

        public TicketViewModel CurrentTicket
        {
            get
            {
                return this.currentTicket;
            }
            set
            {
                this.currentTicket = value;
                this.OnPropertyChanged<TicketViewModel>((System.Linq.Expressions.Expression<Func<TicketViewModel>>)(() => this.CurrentTicket));
            }
        }

        public bool CanDeleteOrder
        {
            get
            {
                return this.canDeleteOrder;
            }
            set
            {
                this.canDeleteOrder = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.CanDeleteOrder));
            }
        }

        public string KeypadEntry
        {
            get
            {
                return this.keypadEntry;
            }
            set
            {
                this.keypadEntry = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.KeypadEntry));
            }
        }

        public TransactionViewModel NewTransaction
        {
            get
            {
                return this.newTransaction;
            }
            set
            {
                this.newTransaction = value;
                this.OnPropertyChanged<TransactionViewModel>((System.Linq.Expressions.Expression<Func<TransactionViewModel>>)(() => this.NewTransaction));
            }
        }

        public virtual ObservableCollection<TransactionViewModel> Transactions { get; set; }

        public Transaction.PaymentMethod PaymentMethodUsed
        {
            get
            {
                return this.paymentMethodUsed;
            }
            set
            {
                this.paymentMethodUsed = value;
                this.OnPropertyChanged<Transaction.PaymentMethod>((System.Linq.Expressions.Expression<Func<Transaction.PaymentMethod>>)(() => this.PaymentMethodUsed));
                this.OnPaymentMethodChanged(value);
            }
        }

        public virtual ObservableCollection<Product> SelectedItemsCoinPayment { get; set; }

        public virtual ObservableCollection<Product> TicketProducts { get; set; }

        public CashTransaction NewCashTransaction
        {
            get
            {
                return this.newCashTransaction;
            }
            set
            {
                this.newCashTransaction = value;
                this.OnPropertyChanged<CashTransaction>((System.Linq.Expressions.Expression<Func<CashTransaction>>)(() => this.NewCashTransaction));
            }
        }

        public FreeTransaction NewFreeTransaction
        {
            get
            {
                return this.newFreeTransaction;
            }
            set
            {
                this.newFreeTransaction = value;
                this.OnPropertyChanged<FreeTransaction>((System.Linq.Expressions.Expression<Func<FreeTransaction>>)(() => this.NewFreeTransaction));
            }
        }

        public NfcTransaction NewNfcTransaction
        {
            get
            {
                return this.newNfcTransaction;
            }
            set
            {
                this.newNfcTransaction = value;
                this.OnPropertyChanged<NfcTransaction>((System.Linq.Expressions.Expression<Func<NfcTransaction>>)(() => this.NewNfcTransaction));
            }
        }

        public CoinTransaction NewCoinTransaction
        {
            get
            {
                return this.newCoinTransaction;
            }
            set
            {
                this.newCoinTransaction = value;
                this.OnPropertyChanged<CoinTransaction>((System.Linq.Expressions.Expression<Func<CoinTransaction>>)(() => this.NewCoinTransaction));
            }
        }

        public PaymentViewModel()
        {
            PaymentViewModel.logger.Debug("VIEWMODEL LOADING: PaymentViewModel");
            AuthenticationService.Instance.SuspendService = false;
            this.CurrentTicket = new TicketViewModel();
            this.UnitOfWork = new UnitOfWork();
            this.SelectedItemsCoinPayment = new ObservableCollection<Product>();
            this.TicketProducts = new ObservableCollection<Product>();
            this.Transactions = new ObservableCollection<TransactionViewModel>();
            this.NewTransaction = new TransactionViewModel();
            this.NewTransaction.NewTransaction = true;
            CashTransaction cashTransaction = new CashTransaction();
            cashTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewCashTransaction = cashTransaction;
            CoinTransaction coinTransaction = new CoinTransaction();
            coinTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewCoinTransaction = coinTransaction;
            FreeTransaction freeTransaction = new FreeTransaction();
            freeTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewFreeTransaction = freeTransaction;
            NfcTransaction nfcTransaction = new NfcTransaction();
            nfcTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewNfcTransaction = nfcTransaction;
            if (this.paymentMethodUsed == Transaction.PaymentMethod.Cash)
                this.NewTransaction.Transaction = (Transaction)this.NewCashTransaction;
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.Free)
                this.NewTransaction.Transaction = (Transaction)this.NewFreeTransaction;
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.NFC)
                this.NewTransaction.Transaction = (Transaction)this.NewNfcTransaction;
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.Coin)
                this.NewTransaction.Transaction = (Transaction)this.NewCoinTransaction;
            this.Transactions.Add(this.NewTransaction);
            PaymentViewModel.logger.Debug("VIEWMODEL LOADED: PaymentViewModel");
        }

        public PaymentViewModel(TicketViewModel ticket)
        {
            PaymentViewModel.logger.Debug("VIEWMODEL LOADING: PaymentViewModel");
            AuthenticationService.Instance.SuspendService = false;
            this.CurrentTicket = ticket;
            this.UnitOfWork = new UnitOfWork();
            this.SelectedItemsCoinPayment = new ObservableCollection<Product>();
            this.TicketProducts = new ObservableCollection<Product>();
            if (!this.currentTicket.Ticket.Transactions.Any<Transaction>())
                this.CanDeleteOrder = true;
            this.Transactions = new ObservableCollection<TransactionViewModel>();
            foreach (Transaction transaction in (Collection<Transaction>)this.CurrentTicket.Ticket.Transactions)
                this.Transactions.Add(new TransactionViewModel()
                {
                    Transaction = transaction,
                    NewTransaction = false
                });
            this.NewTransaction = new TransactionViewModel();
            this.NewTransaction.NewTransaction = true;
            CashTransaction cashTransaction = new CashTransaction();
            cashTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewCashTransaction = cashTransaction;
            CoinTransaction coinTransaction = new CoinTransaction();
            coinTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewCoinTransaction = coinTransaction;
            FreeTransaction freeTransaction = new FreeTransaction();
            freeTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewFreeTransaction = freeTransaction;
            NfcTransaction nfcTransaction = new NfcTransaction();
            nfcTransaction.PayTime = new DateTime?(DateTime.Now);
            this.NewNfcTransaction = nfcTransaction;
            if (this.paymentMethodUsed == Transaction.PaymentMethod.Cash)
                this.NewTransaction.Transaction = (Transaction)this.NewCashTransaction;
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.Free)
                this.NewTransaction.Transaction = (Transaction)this.NewFreeTransaction;
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.NFC)
                this.NewTransaction.Transaction = (Transaction)this.NewNfcTransaction;
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.Coin)
                this.NewTransaction.Transaction = (Transaction)this.NewCoinTransaction;
            this.Transactions.Add(this.NewTransaction);
            foreach (TicketLine ticketLine in (Collection<TicketLine>)this.CurrentTicket.Ticket.TicketLines)
            {
                for (int index = 0; index < (int)ticketLine.Amount; ++index)
                    this.TicketProducts.Add(ticketLine.Product);
            }
            foreach (Product ticketProduct in (Collection<Product>)this.TicketProducts)
                this.SelectedItemsCoinPayment.Add(ticketProduct);
            if (this.NewTransaction.Transaction.PaymentMethodUsed == Transaction.PaymentMethod.Cash || this.NewTransaction.Transaction.PaymentMethodUsed == Transaction.PaymentMethod.Free)
                this.NewTransaction.Transaction.Amount = this.CurrentTicket.Ticket.TotalRemaining;
            PaymentViewModel.logger.Debug("VIEWMODEL LOADED: PaymentViewModel");
        }

        ~PaymentViewModel()
        {
            PaymentViewModel.logger.Debug("VIEWMODEL DESTROYED: PaymentViewModel");
        }

        private void OnPaymentMethodChanged(Transaction.PaymentMethod paymentMethod)
        {
            this.Transactions.Remove(this.NewTransaction);
            this.NewTransaction = new TransactionViewModel();
            this.NewTransaction.NewTransaction = true;
            CashTransaction cashTransaction = new CashTransaction();
            cashTransaction.PayTime = new DateTime?(DateTime.Now);
            cashTransaction.PaymentMethodUsed = paymentMethod;
            this.NewCashTransaction = cashTransaction;
            CoinTransaction coinTransaction = new CoinTransaction();
            coinTransaction.PayTime = new DateTime?(DateTime.Now);
            coinTransaction.PaymentMethodUsed = paymentMethod;
            this.NewCoinTransaction = coinTransaction;
            FreeTransaction freeTransaction = new FreeTransaction();
            freeTransaction.PayTime = new DateTime?(DateTime.Now);
            freeTransaction.PaymentMethodUsed = paymentMethod;
            this.NewFreeTransaction = freeTransaction;
            NfcTransaction nfcTransaction = new NfcTransaction();
            nfcTransaction.PayTime = new DateTime?(DateTime.Now);
            nfcTransaction.PaymentMethodUsed = paymentMethod;
            this.NewNfcTransaction = nfcTransaction;
            if (this.paymentMethodUsed == Transaction.PaymentMethod.Cash)
            {
                this.NewTransaction.Transaction = (Transaction)this.NewCashTransaction;
                this.NewTransaction.Transaction.Amount = this.CurrentTicket.Ticket.TotalRemaining;
            }
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.Free)
            {
                this.NewTransaction.Transaction = (Transaction)this.NewFreeTransaction;
                this.NewTransaction.Transaction.Amount = this.CurrentTicket.Ticket.TotalRemaining;
            }
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.NFC)
                this.NewTransaction.Transaction = (Transaction)this.NewNfcTransaction;
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.Coin)
                this.NewTransaction.Transaction = (Transaction)this.NewCoinTransaction;
            this.Transactions.Add(this.NewTransaction);
        }

        public ICommand RedoTransactionCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.RedoTransactionCommandExec));
            }
        }

        public ICommand ConfirmPaymentCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.ConfirmPaymentCommandExec));
            }
        }

        public ICommand CancelPaymentCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.CancelPaymentExec));
            }
        }

        public ICommand DeleteOrderCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.DeleteOrderExec));
            }
        }

        private void RedoTransactionCommandExec()
        {
            if (NavigatorHelper.NavigationService == null)
                return;
            try
            {
                this.UnitOfWork.Dispose();
                PaymentViewModel paymentViewModel = new PaymentViewModel(this.currentTicket);
                PaymentPageView paymentPageView = new PaymentPageView();
                paymentPageView.DataContext = (object)paymentViewModel;
                NavigatorHelper.NavigationService.Navigate((object)paymentPageView);
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
            catch (FormattedDbEntityValidationException ex)
            {
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                PaymentViewModel.logger.Warn("Database warning: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        private void ConfirmPaymentCommandExec()
        {
            if (this.paymentMethodUsed == Transaction.PaymentMethod.Cash)
            {
                if (((CashTransaction)this.newTransaction.Transaction).MoneyReceived == 0.0)
                {
                    int num = (int)WPFMessageBox.Show(Resources.MessageZeroPayed, Resources.MessageZeroPayedTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else if (this.paymentMethodUsed == Transaction.PaymentMethod.Free)
            {
                if (string.IsNullOrWhiteSpace(this.newFreeTransaction.Reason))
                {
                    int num = (int)WPFMessageBox.Show(Resources.MessageZeroPayed, Resources.MessageZeroPayedTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else if (this.paymentMethodUsed != Transaction.PaymentMethod.NFC && this.paymentMethodUsed == Transaction.PaymentMethod.Coin)
            {
                this.NewTransaction.Transaction.Amount = (double)((CoinTransaction)this.NewTransaction.Transaction).CoinsReceived;
                if (((CoinTransaction)this.newTransaction.Transaction).CoinsReceived == 0)
                {
                    int num = (int)WPFMessageBox.Show(Resources.MessageZeroPayed, Resources.MessageZeroPayedTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            try
            {
                this.UnitOfWork.TicketRepository.Attach(this.CurrentTicket.Ticket);
                this.NewTransaction.Transaction.PayTime = new DateTime?(DateTime.Now);
                if (AuthenticationService.Instance.AuthenticatedMember != null && this.CurrentTicket.Ticket.CreatedBy != null)
                    this.NewTransaction.Transaction.PaymentHandledBy = !(this.CurrentTicket.Ticket.CreatedBy.Id == AuthenticationService.Instance.AuthenticatedMember.Id) ? this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id) : this.CurrentTicket.Ticket.CreatedBy;
                if (this.NewTransaction.Transaction.PaymentMethodUsed == Transaction.PaymentMethod.Coin)
                    this.CurrentTicket.Ticket.TotalRemaining -= this.SelectedItemsCoinPayment.Sum<Product>((Func<Product, double>)(x => x.Price));
                else
                    this.CurrentTicket.Ticket.TotalRemaining -= this.NewTransaction.Transaction.Amount;
                this.CurrentTicket.Ticket.Transactions.Add(this.NewTransaction.Transaction);
                this.UnitOfWork.TicketRepository.Update(this.CurrentTicket.Ticket);
                this.UnitOfWork.Save();
                PaymentViewModel.logger.Info("Payment added for ticket ID: " + (object)this.currentTicket.Ticket.Id);
                if (NavigatorHelper.NavigationService == null)
                    return;
                if (this.CurrentTicket.Ticket.TotalRemaining == 0.0)
                {
                    NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
                    NavigatorHelper.NavigationService.RemoveBackEntry();
                }
                else
                    this.RedoTransactionCommandExec();
            }
            catch (SqlException ex)
            {
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                PaymentViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        private void CancelPaymentExec()
        {
            NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
        }

        private void DeleteOrderExec()
        {
            try
            {
                if (MessageBoxResult.Yes != WPFMessageBox.Show(Resources.QuestionConfirmDeleteOrder, Resources.QuestionConfirmDeleteOrderTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No))
                    return;
                this.UnitOfWork.TicketRepository.Attach(this.CurrentTicket.Ticket);
                this.UnitOfWork.TicketRepository.Delete(this.CurrentTicket.Ticket);
                this.UnitOfWork.Save();
                PaymentViewModel.logger.Info("Order deleted: " + (object)this.currentTicket.Ticket.Id);
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewTicketPageView.xaml", UriKind.Relative));
            }
            catch (SqlException ex)
            {
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                PaymentViewModel.logger.Error("Database error: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        public ICommand KeyPadEnterCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.KeypadEnterCommandExec));
            }
        }

        private void KeypadEnterCommandExec()
        {
            if (string.IsNullOrEmpty(this.KeypadEntry))
                return;
            double num1 = double.Parse(this.KeypadEntry);
            if (num1 >= this.CurrentTicket.Ticket.TotalRemaining)
            {
                CashDrawerService.OpenDrawer();
                ((CashTransaction)this.NewTransaction.Transaction).MoneyReceived = num1;
                ((CashTransaction)this.NewTransaction.Transaction).MoneyReturned = ((CashTransaction)this.NewTransaction.Transaction).MoneyReceived - this.CurrentTicket.Ticket.TotalRemaining;
                this.KeypadEntry = string.Empty;
            }
            else
            {
                int num2 = (int)WPFMessageBox.Show("The entered price has to be bigger then the total remaining price.", "Validation warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        public ICommand CoinKeyPadEnterCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.CoinKeypadEnterCommandExec));
            }
        }

        private void CoinKeypadEnterCommandExec()
        {
            if (string.IsNullOrEmpty(this.KeypadEntry))
                return;
            int num1 = int.Parse(this.KeypadEntry);
            if ((double)num1 == this.NewTransaction.Transaction.Amount)
            {
                CashDrawerService.OpenDrawer();
                ((CoinTransaction)this.NewTransaction.Transaction).CoinsReceived = num1;
                this.KeypadEntry = string.Empty;
            }
            else
            {
                int num2 = (int)WPFMessageBox.Show("The received amount of coins has to be the exact amount due.", "Validation warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        public ICommand SelectedItemsCoinPaymentChanged
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SelectedItemsCoinPaymentChangedExec));
            }
        }

        private void SelectedItemsCoinPaymentChangedExec()
        {
            this.NewTransaction.Transaction.Amount = (double)this.SelectedItemsCoinPayment.Sum<Product>((Func<Product, int>)(x => x.PriceCoin));
        }
    }
}
