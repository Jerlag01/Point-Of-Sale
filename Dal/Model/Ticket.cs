using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class Ticket : ObservableObject
    {
        private int id;
        private DateTime creationTime;
        private Member createdBy;
        private double totalPrice;
        private double totalVat;
        private double totalRemaining;
        private int totalCoins;
        private ObservableCollection<TicketLine> ticketLines;
        private CheckoutSheet checkoutSheet;

        public Ticket()
        {
            this.ticketLines = new ObservableCollection<TicketLine>();
        }

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

        [Required]
        public virtual DateTime CreationTime
        {
            get
            {
                return this.creationTime;
            }
            set
            {
                if (!(value != this.creationTime))
                    return;
                this.creationTime = value;
                this.OnPropertyChanged<DateTime>((Expression<Func<DateTime>>)(() => this.CreationTime));
            }
        }

        public virtual Member CreatedBy
        {
            get
            {
                return this.createdBy;
            }
            set
            {
                if (value == this.createdBy)
                    return;
                this.createdBy = value;
                this.OnPropertyChanged<Member>((Expression<Func<Member>>)(() => this.CreatedBy));
            }
        }

        [Required]
        public virtual double TotalPrice
        {
            get
            {
                return this.totalPrice;
            }
            private set
            {
                if (value == this.totalPrice)
                    return;
                this.totalPrice = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.TotalPrice));
            }
        }

        [Required]
        public virtual double TotalVat
        {
            get
            {
                return this.totalVat;
            }
            private set
            {
                if (value == this.totalVat)
                    return;
                this.totalVat = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.TotalVat));
            }
        }

        public virtual double TotalRemaining
        {
            get
            {
                return this.totalRemaining;
            }
            set
            {
                if (value == this.totalRemaining)
                    return;
                this.totalRemaining = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.TotalRemaining));
            }
        }

        public virtual int TotalCoins
        {
            get
            {
                return this.totalCoins;
            }
            private set
            {
                if (value == this.totalCoins)
                    return;
                this.totalCoins = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.TotalCoins));
            }
        }

        [Required]
        public virtual ObservableCollection<TicketLine> TicketLines
        {
            get
            {
                return this.ticketLines;
            }
            set
            {
                this.ticketLines = value;
            }
        }

        public virtual ObservableCollection<Transaction> Transactions { get; set; }

        public virtual CheckoutSheet CheckoutSheet
        {
            get
            {
                return this.checkoutSheet;
            }
            set
            {
                if (value == this.checkoutSheet)
                    return;
                this.checkoutSheet = value;
                this.OnPropertyChanged<CheckoutSheet>((Expression<Func<CheckoutSheet>>)(() => this.CheckoutSheet));
            }
        }

        public void RecalculatePrice()
        {
            foreach (TicketLine ticketLine in (Collection<TicketLine>)this.TicketLines)
                ticketLine.RecalculatePrice();
            this.TotalCoins = this.TicketLines.Sum<TicketLine>((Func<TicketLine, int>)(ticketline => ticketline.LinePriceCoins));
            this.TotalPrice = this.TicketLines.Sum<TicketLine>((Func<TicketLine, double>)(ticketLine => ticketLine.LinePriceIncl));
            this.TotalVat = this.TicketLines.Sum<TicketLine>((Func<TicketLine, double>)(ticketLine => ticketLine.VatValue));
            this.OnPropertyChanged<ObservableCollection<TicketLine>>((Expression<Func<ObservableCollection<TicketLine>>>)(() => this.TicketLines));
            this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.TotalPrice));
            this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.TotalVat));
            this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.TotalCoins));
        }
    }
}