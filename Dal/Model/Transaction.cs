using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class Transaction : ObservableObject
    {
        private int id;
        private DateTime? payTime;
        private double amount;
        private Transaction.PaymentMethod paymentMethodUsed;
        private Member paymentHandledBy;
        private Ticket ticket;

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if (value == this.id)
                    return;
                this.id = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.Id));
            }
        }

        public virtual DateTime? PayTime
        {
            get
            {
                return this.payTime;
            }
            set
            {
                DateTime? nullable = value;
                DateTime? payTime = this.payTime;
                if ((nullable.HasValue == payTime.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != payTime.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
                    return;
                this.payTime = value;
                this.OnPropertyChanged<DateTime?>((Expression<Func<DateTime?>>)(() => this.PayTime));
            }
        }

        [Required]
        public virtual double Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                if (value == this.amount)
                    return;
                this.amount = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.Amount));
            }
        }

        public virtual Transaction.PaymentMethod PaymentMethodUsed
        {
            get
            {
                return this.paymentMethodUsed;
            }
            set
            {
                if (value == this.paymentMethodUsed)
                    return;
                this.paymentMethodUsed = value;
                this.OnPropertyChanged<Transaction.PaymentMethod>((Expression<Func<Transaction.PaymentMethod>>)(() => this.PaymentMethodUsed));
            }
        }

        public virtual Member PaymentHandledBy
        {
            get
            {
                return this.paymentHandledBy;
            }
            set
            {
                if (value == this.paymentHandledBy)
                    return;
                this.paymentHandledBy = value;
                this.OnPropertyChanged<Member>((Expression<Func<Member>>)(() => this.PaymentHandledBy));
            }
        }

        public virtual Ticket Ticket
        {
            get
            {
                return this.ticket;
            }
            set
            {
                if (value == this.ticket)
                    return;
                this.ticket = value;
                this.OnPropertyChanged<Ticket>((Expression<Func<Ticket>>)(() => this.Ticket));
            }
        }

        public enum PaymentMethod
        {
            Cash,
            Free,
            NFC,
            Coin,
        }
    }
}