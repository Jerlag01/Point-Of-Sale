using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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
    public class ConfigurationViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UnitOfWork UnitOfWork;
        private string selectedMenuItem;
        private int ProductX;
        private int ProductY;
        private int CategoryX;
        private Category selectedCategory;
        private Product selectedProduct;
        private bool canDeleteSelectedProduct;
        private bool canHideSelectedProduct;
        private bool canDeleteSelectedCategory;
        private bool canEditSelectedCategory;
        private bool canEditSelectedProduct;
        private Category newCategory;
        private Product newProduct;
        private Product selectedHiddenProduct;
        private string newProductImagePath;
        private string newCategoryImagePath;
        private Member selectedUser;
        private string userFilter;

        public ObservableCollection<string> SettingsMenu { get; set; }

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

        public ObservableCollection<Category> Categories { get; set; }

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

        public Product SelectedProduct
        {
            get
            {
                return this.selectedProduct;
            }
            set
            {
                this.selectedProduct = value;
                this.OnPropertyChanged<Product>((System.Linq.Expressions.Expression<Func<Product>>)(() => this.SelectedProduct));
            }
        }

        public bool CanDeleteSelectedProduct
        {
            get
            {
                return this.canDeleteSelectedProduct;
            }
            set
            {
                this.canDeleteSelectedProduct = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.CanDeleteSelectedProduct));
            }
        }

        public bool CanHideSelectedProduct
        {
            get
            {
                return this.canHideSelectedProduct;
            }
            set
            {
                this.canHideSelectedProduct = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.CanHideSelectedProduct));
            }
        }

        public bool CanDeleteSelectedCategory
        {
            get
            {
                return this.canDeleteSelectedCategory;
            }
            set
            {
                this.canDeleteSelectedCategory = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.CanDeleteSelectedCategory));
            }
        }

        public bool CanEditSelectedCategory
        {
            get
            {
                return this.canEditSelectedCategory;
            }
            set
            {
                this.canEditSelectedCategory = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.CanEditSelectedCategory));
            }
        }

        public bool CanEditSelectedProduct
        {
            get
            {
                return this.canEditSelectedProduct;
            }
            set
            {
                this.canEditSelectedProduct = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.CanEditSelectedProduct));
            }
        }

        public Category NewCategory
        {
            get
            {
                return this.newCategory;
            }
            set
            {
                this.newCategory = value;
                this.OnPropertyChanged<Category>((System.Linq.Expressions.Expression<Func<Category>>)(() => this.NewCategory));
            }
        }

        public Product NewProduct
        {
            get
            {
                return this.newProduct;
            }
            set
            {
                this.newProduct = value;
                this.OnPropertyChanged<Product>((System.Linq.Expressions.Expression<Func<Product>>)(() => this.NewProduct));
            }
        }

        public ObservableCollection<Product> HiddenProducts { get; set; }

        public Product SelectedHiddenProduct
        {
            get
            {
                return this.selectedHiddenProduct;
            }
            set
            {
                this.selectedHiddenProduct = value;
                this.OnPropertyChanged<Product>((System.Linq.Expressions.Expression<Func<Product>>)(() => this.SelectedHiddenProduct));
            }
        }

        public string NewProductImagePath
        {
            get
            {
                return this.newProductImagePath;
            }
            set
            {
                this.newProductImagePath = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.NewProductImagePath));
            }
        }

        public string NewCategoryImagePath
        {
            get
            {
                return this.newCategoryImagePath;
            }
            set
            {
                this.newCategoryImagePath = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.NewCategoryImagePath));
            }
        }

        public ObservableCollection<Member> ActiveUserList { get; set; }

        public ObservableCollection<Role> UserRolesList { get; set; }

        public ObservableCollection<Role> SelectedRoles { get; set; }

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

        public ConfigurationViewModel()
        {
            ConfigurationViewModel.logger.Debug("VIEWMODEL LOADING: ConfigurationViewModel");
            AuthenticationService.Instance.SuspendService = false;
            this.SettingsMenu = new ObservableCollection<string>();
            this.UnitOfWork = new UnitOfWork();
            if (AuthenticationService.Instance.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "Admin")) > 0)
            {
                this.SettingsMenu.Add(Resources.PageConfigurationMenuProducts);
                this.SettingsMenu.Add(Resources.PageConfigurationMenuUserRights);
                this.SelectedMenuItem = Resources.PageConfigurationMenuProducts;
            }
            else if (AuthenticationService.Instance.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "ProductAdmin")) > 0)
            {
                this.SettingsMenu.Add(Resources.PageConfigurationMenuProducts);
                this.SelectedMenuItem = Resources.PageConfigurationMenuProducts;
            }
            WeakEventManager<AuthenticationService, AuthenticationService.AuthenticationEventArgs>.AddHandler(AuthenticationService.Instance, "MemberAuthenticated", new EventHandler<AuthenticationService.AuthenticationEventArgs>(this.ConfigurationViewModelOnMemberAuthenticated));
            WeakEventManager<AuthenticationService, AuthenticationService.AuthenticationEventArgs>.AddHandler(AuthenticationService.Instance, "MemberDeauthenticated", new EventHandler<AuthenticationService.AuthenticationEventArgs>(this.ConfigurationViewModelOnMemberDeauthenticated));
            this.Categories = new ObservableCollection<Category>();
            this.LoadCategories();
            this.HiddenProducts = new ObservableCollection<Product>();
            this.LoadHiddenProducts();
            this.NewCategory = new Category();
            this.NewProduct = new Product();
            this.ActiveUserList = new ObservableCollection<Member>();
            this.LoadActiveUsers();
            this.UserRolesList = new ObservableCollection<Role>();
            this.SelectedRoles = new ObservableCollection<Role>();
            this.LoadRoles();
        }

        private void ConfigurationViewModelOnMemberAuthenticated(
          object sender,
          AuthenticationService.AuthenticationEventArgs args)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                this.SettingsMenu.Clear();
                if (args.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "Admin")) > 0)
                {
                    this.SettingsMenu.Add(Resources.PageConfigurationMenuProducts);
                    this.SettingsMenu.Add(Resources.PageConfigurationMenuUserRights);
                    this.SelectedMenuItem = Resources.PageConfigurationMenuProducts;
                }
                else
                {
                    if (args.AuthenticatedMember.Roles.Count<Role>((Func<Role, bool>)(r => r.Name == "ProductAdmin")) <= 0)
                        return;
                    this.SettingsMenu.Add(Resources.PageConfigurationMenuProducts);
                    this.SelectedMenuItem = Resources.PageConfigurationMenuProducts;
                }
            }));
        }

        private void ConfigurationViewModelOnMemberDeauthenticated(
          object sender,
          AuthenticationService.AuthenticationEventArgs args)
        {
            Application.Current.Dispatcher.Invoke((Action)(() => this.SettingsMenu.Clear()));
        }

        ~ConfigurationViewModel()
        {
            ConfigurationViewModel.logger.Debug("VIEWMODEL DESTROYED: ConfigurationViewModel");
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
            NavigatorHelper.NavigationService.Refresh();
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
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        private void LoadHiddenProducts()
        {
            try
            {
                foreach (Product product in this.UnitOfWork.ProductRepository.GetHidden())
                    this.HiddenProducts.Add(product);
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        public ICommand ProductSelectedCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.ProductSelectedCommandExec));
            }
        }

        private void ProductSelectedCommandExec()
        {
            if (this.SelectedProduct == null)
            {
                this.CanDeleteSelectedProduct = false;
                this.CanHideSelectedProduct = false;
                this.CanEditSelectedProduct = false;
            }
            else
            {
                List<TicketLine> byProduct = this.UnitOfWork.TicketLineRepository.GetByProduct(this.SelectedProduct);
                if (byProduct == null || byProduct.Count == 0)
                {
                    this.CanDeleteSelectedProduct = true;
                    this.CanHideSelectedProduct = true;
                    this.CanEditSelectedProduct = true;
                }
                else
                {
                    if (byProduct.Count <= 0)
                        return;
                    this.CanDeleteSelectedProduct = false;
                    this.CanHideSelectedProduct = true;
                    this.CanEditSelectedProduct = true;
                }
            }
        }

        public ICommand CategorySelectedCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.CategorySelectedCommandExec));
            }
        }

        private void CategorySelectedCommandExec()
        {
            if (this.SelectedCategory == null)
            {
                this.CanDeleteSelectedCategory = false;
                this.CanEditSelectedCategory = false;
            }
            else
            {
                this.CanDeleteSelectedCategory = true;
                this.CanEditSelectedCategory = true;
            }
        }

        public ICommand AddProductCommand
        {
            get
            {
                return (ICommand)new RelayCommand<string>(new Action<string>(this.AddProductCommandExec));
            }
        }

        private void AddProductCommandExec(string xylocation)
        {
            if (string.IsNullOrWhiteSpace(xylocation))
                return;
            string[] strArray = xylocation.Split(',');
            this.ProductX = int.Parse(strArray[0]);
            this.ProductY = int.Parse(strArray[1]);
            this.SelectedMenuItem = "AddProductControlView";
        }

        public ICommand AddCategoryCommand
        {
            get
            {
                return (ICommand)new RelayCommand<string>(new Action<string>(this.AddCategoryCommandExec));
            }
        }

        private void AddCategoryCommandExec(string xlocation)
        {
            if (string.IsNullOrWhiteSpace(xlocation))
                return;
            this.CategoryX = int.Parse(xlocation);
            this.SelectedMenuItem = "AddCategoryControlView";
        }

        public ICommand DeleteCategoryCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.DeleteCategoryCommandExec));
            }
        }

        private void DeleteCategoryCommandExec()
        {
            if (WPFMessageBox.Show(Resources.PageConfigurationProductsCategoryConfirmDeletion, Resources.PageConfigurationProductsCategoryConfirmDeletionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.No)
                return;
            if (this.SelectedCategory != null && this.SelectedCategory.Products != null && this.SelectedCategory.Products.Count > 0)
            {
                if (WPFMessageBox.Show(Resources.PageConfigurationProductsCategoryContainsProducts, Resources.PageConfigurationProductsCategoryContainsProductsTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.No)
                    return;
                foreach (Product product in (Collection<Product>)this.SelectedCategory.Products)
                {
                    product.Category = (Category)null;
                    product.IsHidden = true;
                }
                try
                {
                    this.UnitOfWork.CategoryRepository.Update(this.SelectedCategory);
                    this.UnitOfWork.Save();
                }
                catch (SqlException ex)
                {
                    SplashScreenHelper.CloseInstant();
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                    ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
                }
                catch (DataException ex)
                {
                    SplashScreenHelper.CloseInstant();
                    int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                    ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
                }
            }
            try
            {
                if (this.SelectedCategory != null)
                {
                    if (!string.IsNullOrWhiteSpace(this.SelectedCategory.ImagePath))
                        File.Delete(Directory.GetCurrentDirectory() + "\\Images\\" + this.selectedCategory.ImagePath);
                }
            }
            catch (Exception ex)
            {
                ConfigurationViewModel.logger.Warn("Could not delete category image file:", ex.Message + ex.InnerException?.ToString());
            }
            try
            {
                this.UnitOfWork.CategoryRepository.Delete(this.SelectedCategory);
                this.UnitOfWork.Save();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            if (NavigatorHelper.NavigationService == null)
                return;
            NavigatorHelper.NavigationService.Refresh();
        }

        public ICommand HideProductCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.HideProductCommandExec));
            }
        }

        private void HideProductCommandExec()
        {
            if (WPFMessageBox.Show(Resources.PageConfigurationProductsProductConfirmHide, Resources.PageConfigurationProductsProductConfirmHideTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.No)
                return;
            try
            {
                this.SelectedProduct.IsHidden = true;
                this.UnitOfWork.ProductRepository.Update(this.SelectedProduct);
                this.UnitOfWork.Save();
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        public ICommand DeleteProductCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.DeleteProductCommandExec));
            }
        }

        private void DeleteProductCommandExec()
        {
            if (WPFMessageBox.Show(Resources.PageConfigurationProductsProductConfirmDeletion, Resources.PageConfigurationProductsProductConfirmDeletionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.No)
                return;
            try
            {
                if (this.SelectedProduct != null)
                {
                    if (!string.IsNullOrWhiteSpace(this.SelectedProduct.PicturePath))
                        File.Delete(Directory.GetCurrentDirectory() + "\\Images\\" + this.selectedProduct.PicturePath);
                }
            }
            catch (Exception ex)
            {
                ConfigurationViewModel.logger.Warn("Could not delete product image file:", ex.Message + ex.InnerException?.ToString());
            }
            try
            {
                this.UnitOfWork.ProductRepository.Delete(this.SelectedProduct);
                this.UnitOfWork.Save();
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        public ICommand EditProductCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.EditProductCommandExec));
            }
        }

        private void EditProductCommandExec()
        {
            this.SelectedMenuItem = "EditProductControlView";
        }

        public ICommand EditCategoryCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.EditCategoryCommandExec));
            }
        }

        private void EditCategoryCommandExec()
        {
            this.SelectedMenuItem = "EditCategoryControlView";
        }

        public ICommand BrowseCategoryImageCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.BrowseCategoryImageCommandExec));
            }
        }

        private void BrowseCategoryImageCommandExec()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = ".png";
            openFileDialog1.Filter = "PNG Files (*.png)|*.png";
            OpenFileDialog openFileDialog2 = openFileDialog1;
            bool? nullable = openFileDialog2.ShowDialog();
            bool flag = true;
            if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return;
            this.NewCategory.ImagePath = openFileDialog2.FileName;
        }

        public ICommand BrowseProductImageCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.BrowseProductImageCommandExec));
            }
        }

        private void BrowseProductImageCommandExec()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = ".png";
            openFileDialog1.Filter = "PNG Files (*.png)|*.png";
            OpenFileDialog openFileDialog2 = openFileDialog1;
            bool? nullable = openFileDialog2.ShowDialog();
            bool flag = true;
            if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return;
            this.NewProduct.PicturePath = openFileDialog2.FileName;
        }

        public ICommand BrowseEditProductImageCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.BrowseEditProductImageCommandExec));
            }
        }

        private void BrowseEditProductImageCommandExec()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = ".png";
            openFileDialog1.Filter = "PNG Files (*.png)|*.png";
            OpenFileDialog openFileDialog2 = openFileDialog1;
            bool? nullable = openFileDialog2.ShowDialog();
            bool flag = true;
            if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return;
            this.NewProductImagePath = openFileDialog2.FileName;
        }

        public ICommand BrowseEditCategoryImageCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.BrowseEditCategoryImageCommandExec));
            }
        }

        private void BrowseEditCategoryImageCommandExec()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = ".png";
            openFileDialog1.Filter = "PNG Files (*.png)|*.png";
            OpenFileDialog openFileDialog2 = openFileDialog1;
            bool? nullable = openFileDialog2.ShowDialog();
            bool flag = true;
            if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return;
            this.NewCategoryImagePath = openFileDialog2.FileName;
        }

        private void LoadActiveUsers()
        {
            try
            {
                foreach (Member member in this.UnitOfWork.MemberRepository.GetActive())
                    this.ActiveUserList.Add(member);
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        private void LoadRoles()
        {
            try
            {
                foreach (Role role in this.UnitOfWork.RoleRepository.GetAll())
                    this.UserRolesList.Add(role);
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }

        private void FilterUser()
        {
            if (this.ActiveUserList.Count == 0 || this.UserFilter == string.Empty)
                return;
            this.SelectedUser = this.ActiveUserList.FirstOrDefault<Member>((Func<Member, bool>)(u => u.Fullname.ToLower().Contains(this.UserFilter.ToLower())));
        }

        public ICommand SelectedUserChangedCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SelectedUserChangedCommandExec));
            }
        }

        private void SelectedUserChangedCommandExec()
        {
            this.SelectedRoles.Clear();
            if (this.SelectedUser == null || this.SelectedUser.Roles == null)
                return;
            foreach (Role role1 in (Collection<Role>)this.SelectedUser.Roles)
            {
                Role role = role1;
                this.SelectedRoles.Add(this.UserRolesList.First<Role>((Func<Role, bool>)(r => r.Id == role.Id)));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return (ICommand)new RelayCommand(new Action(this.SaveCommandExec));
            }
        }

        private void SaveCommandExec()
        {
            if (this.SelectedMenuItem == "AddCategoryControlView")
            {
                this.NewCategory.Order = this.CategoryX;
                if (string.IsNullOrWhiteSpace(this.NewCategory.Name))
                {
                    int num = (int)WPFMessageBox.Show(Resources.ValidationGeneral, Resources.ValidationGeneralTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (this.UnitOfWork.CategoryRepository.GetByName(this.NewCategory.Name) != null)
                {
                    int num = (int)WPFMessageBox.Show(Resources.ValidationCategorytNameUnique, Resources.ValidationCategoryNameUniqueTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (!string.IsNullOrWhiteSpace(this.NewCategory.ImagePath))
                {
                    try
                    {
                        string str = Guid.NewGuid().ToString();
                        using (Bitmap bitmap = new Bitmap(this.NewCategory.ImagePath))
                        {
                            double num = Math.Min(100.0 / (double)bitmap.Width, 100.0 / (double)bitmap.Height);
                            int width = (int)((double)bitmap.Width * num);
                            int height = (int)((double)bitmap.Height * num);
                            using (Image image = (Image)new Bitmap(width, height))
                            {
                                using (Graphics graphics = Graphics.FromImage(image))
                                {
                                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage((Image)bitmap, 0, 0, width, height);
                                }
                                string filename = Directory.GetCurrentDirectory() + "\\Images\\" + str + ".png";
                                image.Save(filename, ImageFormat.Png);
                            }
                        }
                        this.NewCategory.ImagePath = str + ".png";
                    }
                    catch (Exception ex)
                    {
                        int num = (int)WPFMessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Hand);
                        ConfigurationViewModel.logger.Error("Error while resizing image: {0}", ex.Message);
                        return;
                    }
                }
                this.UnitOfWork.CategoryRepository.Add(this.NewCategory);
            }
            else if (this.SelectedMenuItem == "AddProductControlView")
            {
                if (this.SelectedHiddenProduct != null)
                {
                    this.SelectedHiddenProduct.Xpos = this.ProductX;
                    this.SelectedHiddenProduct.YPos = this.ProductY;
                    this.SelectedHiddenProduct.Category = this.SelectedCategory;
                    this.SelectedHiddenProduct.IsHidden = false;
                    this.UnitOfWork.ProductRepository.Update(this.SelectedHiddenProduct);
                }
                else
                {
                    this.NewProduct.IsHidden = false;
                    this.NewProduct.Xpos = this.ProductX;
                    this.NewProduct.YPos = this.ProductY;
                    this.NewProduct.DateAdded = DateTime.Now;
                    this.NewProduct.Category = this.SelectedCategory;
                    this.NewProduct.TaxCategory = this.UnitOfWork.TaxCategoryRepository.GetById(1);
                    if (AuthenticationService.Instance.AuthenticatedMember != null)
                        this.NewProduct.AddedBy = this.UnitOfWork.MemberRepository.GetById(AuthenticationService.Instance.AuthenticatedMember.Id);
                    if (string.IsNullOrWhiteSpace(this.NewProduct.Name) || this.NewProduct.Price <= 0.0 || this.NewProduct.PriceCoin <= 0)
                    {
                        int num = (int)WPFMessageBox.Show(Resources.ValidationGeneral, Resources.ValidationGeneralTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    if (this.UnitOfWork.ProductRepository.GetByName(this.NewProduct.Name) != null)
                    {
                        int num = (int)WPFMessageBox.Show(Resources.ValidationProductNameUnique, Resources.ValidationProductNameUniqueTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(this.NewProduct.PicturePath))
                    {
                        try
                        {
                            string str = Guid.NewGuid().ToString();
                            using (Bitmap bitmap = new Bitmap(this.NewProduct.PicturePath))
                            {
                                double num = Math.Min(100.0 / (double)bitmap.Width, 100.0 / (double)bitmap.Height);
                                int width = (int)((double)bitmap.Width * num);
                                int height = (int)((double)bitmap.Height * num);
                                using (Image image = (Image)new Bitmap(width, height))
                                {
                                    using (Graphics graphics = Graphics.FromImage(image))
                                    {
                                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        graphics.DrawImage((Image)bitmap, 0, 0, width, height);
                                    }
                                    string filename = Directory.GetCurrentDirectory() + "\\Images\\" + str + ".png";
                                    image.Save(filename, ImageFormat.Png);
                                }
                            }
                            this.NewProduct.PicturePath = str + ".png";
                        }
                        catch (Exception ex)
                        {
                            int num = (int)WPFMessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Hand);
                            ConfigurationViewModel.logger.Error("Error while resizing image: {0}", ex.Message);
                            return;
                        }
                    }
                    this.UnitOfWork.ProductRepository.Add(this.NewProduct);
                }
            }
            else if (this.SelectedMenuItem == "EditProductControlView")
            {
                if (this.SelectedProduct == null)
                    return;
                if (string.IsNullOrWhiteSpace(this.SelectedProduct.Name) || this.SelectedProduct.Price <= 0.0 || this.SelectedProduct.PriceCoin <= 0)
                {
                    int num = (int)WPFMessageBox.Show(Resources.ValidationGeneral, Resources.ValidationGeneralTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (!string.IsNullOrWhiteSpace(this.NewProductImagePath))
                {
                    try
                    {
                        string str = Guid.NewGuid().ToString();
                        using (Bitmap bitmap = new Bitmap(this.NewProductImagePath))
                        {
                            double num = Math.Min(100.0 / (double)bitmap.Width, 100.0 / (double)bitmap.Height);
                            int width = (int)((double)bitmap.Width * num);
                            int height = (int)((double)bitmap.Height * num);
                            using (Image image = (Image)new Bitmap(width, height))
                            {
                                using (Graphics graphics = Graphics.FromImage(image))
                                {
                                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage((Image)bitmap, 0, 0, width, height);
                                }
                                string filename = Directory.GetCurrentDirectory() + "\\Images\\" + str + ".png";
                                image.Save(filename, ImageFormat.Png);
                            }
                        }
                        this.SelectedProduct.PicturePath = str + ".png";
                    }
                    catch (Exception ex)
                    {
                        int num = (int)WPFMessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Hand);
                        ConfigurationViewModel.logger.Error("Error while resizing image: {0}", ex.Message);
                        return;
                    }
                }
                this.UnitOfWork.ProductRepository.Update(this.SelectedProduct);
            }
            else if (this.SelectedMenuItem == "EditCategoryControlView")
            {
                if (string.IsNullOrWhiteSpace(this.SelectedCategory.Name))
                {
                    int num = (int)WPFMessageBox.Show(Resources.ValidationGeneral, Resources.ValidationGeneralTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (this.UnitOfWork.CategoryRepository.GetByName(this.SelectedCategory.Name) != null)
                {
                    int num = (int)WPFMessageBox.Show(Resources.ValidationCategorytNameUnique, Resources.ValidationCategoryNameUniqueTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (!string.IsNullOrWhiteSpace(this.NewCategoryImagePath))
                {
                    try
                    {
                        string str = Guid.NewGuid().ToString();
                        using (Bitmap bitmap = new Bitmap(this.NewCategoryImagePath))
                        {
                            double num = Math.Min(100.0 / (double)bitmap.Width, 100.0 / (double)bitmap.Height);
                            int width = (int)((double)bitmap.Width * num);
                            int height = (int)((double)bitmap.Height * num);
                            using (Image image = (Image)new Bitmap(width, height))
                            {
                                using (Graphics graphics = Graphics.FromImage(image))
                                {
                                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage((Image)bitmap, 0, 0, width, height);
                                }
                                string filename = Directory.GetCurrentDirectory() + "\\Images\\" + str + ".png";
                                image.Save(filename, ImageFormat.Png);
                            }
                        }
                        this.SelectedCategory.ImagePath = str + ".png";
                    }
                    catch (Exception ex)
                    {
                        int num = (int)WPFMessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Hand);
                        ConfigurationViewModel.logger.Error("Error while resizing image: {0}", ex.Message);
                        return;
                    }
                }
                this.UnitOfWork.CategoryRepository.Update(this.SelectedCategory);
            }
            else if (this.SelectedMenuItem == Resources.PageConfigurationMenuUserRights)
            {
                this.SelectedUser.Roles = this.SelectedRoles;
                this.UnitOfWork.MemberRepository.Update(this.SelectedUser);
            }
            try
            {
                this.UnitOfWork.Save();
                if (NavigatorHelper.NavigationService == null)
                    return;
                NavigatorHelper.NavigationService.Refresh();
            }
            catch (SqlException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
            catch (DataException ex)
            {
                SplashScreenHelper.CloseInstant();
                int num = (int)WPFMessageBox.Show(Resources.ExceptionDbWarning + ex.Message, Resources.ExceptionDbWarningTitle, MessageBoxButton.OK, MessageBoxImage.Hand);
                ConfigurationViewModel.logger.Error("Database error: {0}", ex.Message);
            }
        }
    }
}
