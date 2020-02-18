using NLog;
using System;
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
using Pos.Views.NewTicket;
using Util.MicroMvvm;
using Util.WpfMessageBox;

namespace Pos.ViewModels
{
    public class NewTicketViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UnitOfWork UnitOfWork;
        private CheckoutSheet openCheckoutSheet;
        private TicketViewModel currentTicket;
        private bool deleteItemButtonEnabled;
        private bool editItemButtonEnabled;
        private Category selectedCategory;

        public CheckoutSheet OpenCheckoutSheet
        {
            get
            {
                return this.openCheckoutSheet;
            }
            set
            {
                this.openCheckoutSheet = value;
                this.OnPropertyChanged<CheckoutSheet>((System.Linq.Expressions.Expression<Func<CheckoutSheet>>)(() => this.OpenCheckoutSheet));
            }
        }

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

        public bool DeleteItemButtonEnabled
        {
            get
            {
                return this.deleteItemButtonEnabled;
            }
            set
            {
                this.deleteItemButtonEnabled = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.DeleteItemButtonEnabled));
            }
        }

        public bool EditItemButtonEnabled
        {
            get
            {
                return this.editItemButtonEnabled;
            }
            set
            {
                this.editItemButtonEnabled = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.EditItemButtonEnabled));
            }
        }

        public Category SelectedCategory
        {
            get
            {
                return this.selectedCategory;
            }
            set
            {
                this.selectedCategory = value;
                this.OnPropertyChanged<Category>((System.Linq.Expressions.Expression<Func<Category>>)(() => this.SelectedCategory));
            }
        }

        public virtual ObservableCollection<TicketViewModel> OpenTickets { get; set; }

        public virtual ObservableCollection<Category> Categories { get; set; }

        public NewTicketViewModel()
        {
            NewTicketViewModel.logger.Debug("VIEWMODEL LOADING: NewTicketViewModel");
            AuthenticationService.Instance.SuspendService = false;
            this.OpenTickets = new ObservableCollection<TicketViewModel>();
            this.Categories = new ObservableCollection<Category>();
            this.UnitOfWork = new UnitOfWork();
            this.LoadCategories();
            SplashScreenHelper.ShowText(Resources.SplashScreenLoadingProducts);
            SplashScreenHelper.Close();
            if (AuthenticationService.Instance.AuthenticatedMember == null)
                LockScreenHelper.Show();
            this.CurrentTicket = new TicketViewModel()
            {
                Ticket = new Ticket(),
                Visibility = Visibility.Collapsed,
                NewTicket = true,
                ItemSelectedCommand = (ICommand)new RelayCommand(new Action(this.SelectedItemChanged))
            };
            this.OpenTickets.Add(this.CurrentTicket);
            this.OpenCheckoutSheet = this.UnitOfWork.CheckoutSheetRepository.GetLastOpenSheet();
            if (this.OpenCheckoutSheet == null)
            {
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewCheckoutSheetPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
            else
            {
                try
                {
                    foreach (Ticket ticket in this.UnitOfWork.TicketRepository.GetOpenTicketsNoTracking())
                        this.OpenTickets.Add(new TicketViewModel()
                        {
                            Ticket = ticket,
                            TicketControlClicked = (ICommand)new RelayCommand<TicketViewModel>(new Action<TicketViewModel>(this.TicketSelected))
                        });
                }
                catch (SqlException ex)
                {
                    SplashScreenHelper.CloseInstant();
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                    NewTicketViewModel.logger.Error("Database error: {0}", ex.Message);
                }
                catch (DataException ex)
                {
                    SplashScreenHelper.CloseInstant();
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                    NewTicketViewModel.logger.Error("Database error: {0}", ex.Message);
                }
                NewTicketViewModel.logger.Debug("VIEWMODEL LOADED: NewTicketViewModel");
            }
        }

        ~NewTicketViewModel()
        {
            NewTicketViewModel.logger.Debug("VIEWMODEL DESTROYED: NewTicketViewModel");
        }

        public ICommand ConfirmTicketCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.ConfirmTicketCommandExec));
            }
        }

        public ICommand RedoOrderCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.RedoOrderCommandExec));
            }
        }

        public ICommand DeleteItemCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.DeleteItemCommandExec));
            }
        }

        public ICommand EditItemCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.EditItemCommandExec));
            }
        }

        public ICommand ProductClickedCommand
        {
            get
            {
                return (ICommand)new RelayCommand<Product>(new Action<Product>(this.ProductClickedCommandExec));
            }
        }

        public ICommand CloseRegisterCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.CloseRegisterCommandExec));
            }
        }

        private void RedoOrderCommandExec()
        {
            if (NavigatorHelper.NavigationService == null)
                return;
            NavigatorHelper.NavigationService.Refresh();
        }

        private void DeleteItemCommandExec()
        {
            if (this.CurrentTicket.SelectedTicketLine == null)
                return;
            if (this.CurrentTicket.Ticket.TicketLines.Contains(this.CurrentTicket.SelectedTicketLine))
                this.CurrentTicket.Ticket.TicketLines.Remove(this.CurrentTicket.SelectedTicketLine);
            this.CurrentTicket.Ticket.RecalculatePrice();
        }

        private void EditItemCommandExec()
        {
            if (this.CurrentTicket.SelectedTicketLine == null || !this.CurrentTicket.Ticket.TicketLines.Contains(this.CurrentTicket.SelectedTicketLine))
                return;
            this.CurrentTicket.EditMode = !this.CurrentTicket.EditMode;
        }

        private void ProductClickedCommandExec(object clickedProduct)
        {
            Product product = (Product)clickedProduct;
            TicketLine ticketLine1 = this.CurrentTicket.Ticket.TicketLines.FirstOrDefault<TicketLine>((Func<TicketLine, bool>)(tl => tl.Product == product));
            if (ticketLine1 != null)
            {
                ++ticketLine1.Amount;
                TicketLine ticketLine2 = ticketLine1;
                ticketLine2.LinePriceExcl = (double)ticketLine2.Amount * ticketLine1.UnitPrice;
            }
            else
            {
                TicketLine ticketLine2 = new TicketLine()
                {
                    Product = product,
                    Amount = 1,
                    UnitPrice = product.Price,
                    UnitPriceCoins = product.PriceCoin
                };
                TicketLine ticketLine3 = ticketLine2;
                ticketLine3.LinePriceExcl = (double)ticketLine3.Amount * ticketLine2.UnitPrice;
                this.CurrentTicket.Ticket.TicketLines.Add(ticketLine2);
            }
            this.CurrentTicket.Ticket.RecalculatePrice();
            this.CurrentTicket.Ticket.TotalRemaining = this.CurrentTicket.Ticket.TotalPrice;
            this.CurrentTicket.Ticket.CreationTime = DateTime.Now;
            if (this.CurrentTicket.Ticket.CreatedBy == null && AuthenticationService.Instance.AuthenticatedMember != null)
                this.CurrentTicket.Ticket.CreatedBy = this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id);
            this.CurrentTicket.Visibility = Visibility.Visible;
        }

        private void ConfirmTicketCommandExec()
        {
            if (this.CurrentTicket == null || this.CurrentTicket.Ticket == null || this.CurrentTicket.Ticket.TicketLines.Count < 1)
            {
                int num1 = (int)WPFMessageBox.Show(Resources.MessageEmptyTicket, Resources.MessageEmptyTicketTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                this.CurrentTicket.Ticket.CreationTime = DateTime.Now;
                this.OpenCheckoutSheet = this.UnitOfWork.CheckoutSheetRepository.GetLastOpenSheet();
                if (this.OpenCheckoutSheet == null)
                {
                    if (NavigatorHelper.NavigationService == null)
                        return;
                    NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/NewCheckoutSheetPageView.xaml", UriKind.Relative));
                    NavigatorHelper.NavigationService.RemoveBackEntry();
                }
                else
                {
                    this.CurrentTicket.Ticket.CheckoutSheet = this.OpenCheckoutSheet;
                    try
                    {
                        this.UnitOfWork.TicketRepository.Add(this.CurrentTicket.Ticket);
                        this.UnitOfWork.Save();
                        NewTicketViewModel.logger.Info("New order created: " + (object)this.currentTicket.Ticket.Id);
                        if (NavigatorHelper.NavigationService == null)
                            return;
                        NavigatorHelper.NavigationService.Refresh();
                        NavigatorHelper.NavigationService.RemoveBackEntry();
                    }
                    catch (SqlException ex)
                    {
                        int num2 = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                        NewTicketViewModel.logger.Error("Database error: {0}", ex.Message);
                        if (NavigatorHelper.NavigationService == null)
                            return;
                        NavigatorHelper.NavigationService.Refresh();
                        NavigatorHelper.NavigationService.RemoveBackEntry();
                    }
                    catch (DataException ex)
                    {
                        SplashScreenHelper.CloseInstant();
                        int num2 = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                        NewTicketViewModel.logger.Error("Database error: {0}", ex.Message);
                        if (NavigatorHelper.NavigationService == null)
                            return;
                        NavigatorHelper.NavigationService.Refresh();
                        NavigatorHelper.NavigationService.RemoveBackEntry();
                    }
                }
            }
        }

        private void CloseRegisterCommandExec()
        {
            if (this.OpenTickets.Count > 1)
            {
                int num = (int)WPFMessageBox.Show(Resources.MessageOpenTickets, Resources.MessageOpenTicketsTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Navigate(new Uri("Views/NewTicket/CloseCheckoutSheetPageView.xaml", UriKind.Relative));
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
        }

        private void SelectedItemChanged()
        {
            if (this.CurrentTicket.SelectedTicketLine == null)
            {
                this.DeleteItemButtonEnabled = false;
                this.EditItemButtonEnabled = false;
                this.CurrentTicket.EditMode = false;
            }
            else
            {
                this.DeleteItemButtonEnabled = true;
                this.EditItemButtonEnabled = true;
            }
        }

        private void TicketSelected(TicketViewModel ticketViewModel)
        {
            try
            {
                PaymentViewModel paymentViewModel = new PaymentViewModel(ticketViewModel);
                PaymentPageView paymentPageView = new PaymentPageView();
                paymentPageView.DataContext = (object)paymentViewModel;
                NavigatorHelper.NavigationService.Navigate((object)paymentPageView);
                NavigatorHelper.NavigationService.RemoveBackEntry();
            }
            catch (FormattedDbEntityValidationException ex)
            {
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                NewTicketViewModel.logger.Warn("Database warning: {0}", ex.Message);
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
        }

        private void LoadCategories()
        {
            try
            {
                foreach (Category category in this.UnitOfWork.CategoryRepository.GetAll())
                    this.Categories.Add(category);
                if (this.Categories.Count <= 0)
                    return;
                int min = this.Categories.Min<Category>((Func<Category, int>)(o => o.Order));
                this.SelectedCategory = this.Categories.FirstOrDefault<Category>((Func<Category, bool>)(c => c.Order == min));
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewTicketViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                NewTicketViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }
    }
}
