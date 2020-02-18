using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Dal.Model;
using Util.MicroMvvm;

namespace Pos.ViewModels
{
    public class TransactionViewModel : ObservableObject
    {
        private Transaction transaction;
        private Visibility visibility;
        private bool newTransaction;
        private string transactionText;

        public Transaction Transaction
        {
            get
            {
                return this.transaction;
            }
            set
            {
                this.transaction = value;
                this.TransactionText = this.transaction.PaymentMethodUsed != Transaction.PaymentMethod.Coin ? "€ " + (object)this.transaction.Amount : this.transaction.Amount.ToString((IFormatProvider)CultureInfo.CurrentCulture);
                this.OnPropertyChanged<Transaction>((System.Linq.Expressions.Expression<Func<Transaction>>)(() => this.Transaction));
            }
        }

        public Visibility Visibility
        {
            get
            {
                return this.visibility;
            }
            set
            {
                this.visibility = value;
                this.OnPropertyChanged<Visibility>((System.Linq.Expressions.Expression<Func<Visibility>>)(() => this.Visibility));
            }
        }

        public bool NewTransaction
        {
            get
            {
                return this.newTransaction;
            }
            set
            {
                this.newTransaction = value;
                this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<bool>>)(() => this.NewTransaction));
            }
        }

        public string TransactionText
        {
            get
            {
                return this.transactionText;
            }
            set
            {
                this.transactionText = value;
                this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<string>>)(() => this.TransactionText));
            }
        }

        public ICommand TransactionControlClicked { get; set; }
    }
}
