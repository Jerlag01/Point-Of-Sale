using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class CheckoutSheet : ObservableObject
    {
        private int id;
        private DateTime? openTime;
        private Member openedBy;
        private DateTime? closeTime;
        private Member closedBy;
        private ObservableCollection<Ticket> tickets;
        private string comments;
        private double openAmount;
        private double closeAmount;
        private int closeEur1c;
        private int closeEur2c;
        private int closeEur5c;
        private int closeEur10c;
        private int closeEur20c;
        private int closeEur50c;
        private int closeEur1;
        private int closeEur2;
        private int closeEur5;
        private int closeEur10;
        private int closeEur20;
        private int closeEur50;
        private int closeEur100;
        private int closeEur200;
        private int closeEur500;

        public CheckoutSheet()
        {
            this.tickets = new ObservableCollection<Ticket>();
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
        public virtual DateTime? OpenTime
        {
            get
            {
                return this.openTime;
            }
            set
            {
                DateTime? nullable = value;
                DateTime? openTime = this.openTime;
                if ((nullable.HasValue == openTime.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != openTime.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
                    return;
                this.openTime = value;
                this.OnPropertyChanged<DateTime?>((Expression<Func<DateTime?>>)(() => this.OpenTime));
            }
        }

        [Required]
        public virtual Member OpenedBy
        {
            get
            {
                return this.openedBy;
            }
            set
            {
                if (value == this.openedBy)
                    return;
                this.openedBy = value;
                this.OnPropertyChanged<Member>((Expression<Func<Member>>)(() => this.OpenedBy));
            }
        }

        public virtual DateTime? CloseTime
        {
            get
            {
                return this.closeTime;
            }
            set
            {
                DateTime? nullable = value;
                DateTime? closeTime = this.closeTime;
                if ((nullable.HasValue == closeTime.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != closeTime.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
                    return;
                this.closeTime = value;
                this.OnPropertyChanged<DateTime?>((Expression<Func<DateTime?>>)(() => this.CloseTime));
            }
        }

        public virtual Member ClosedBy
        {
            get
            {
                return this.closedBy;
            }
            set
            {
                if (value == this.closedBy)
                    return;
                this.closedBy = value;
                this.OnPropertyChanged<Member>((Expression<Func<Member>>)(() => this.ClosedBy));
            }
        }

        public virtual ObservableCollection<Ticket> Tickets
        {
            get
            {
                return this.tickets;
            }
            set
            {
                this.tickets = value;
            }
        }

        public virtual string Comments
        {
            get
            {
                return this.comments;
            }
            set
            {
                if (!(value != this.comments))
                    return;
                this.comments = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.Comments));
            }
        }

        public virtual double OpenAmount
        {
            get
            {
                return this.openAmount;
            }
            set
            {
                if (value == this.openAmount)
                    return;
                this.openAmount = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.OpenAmount));
            }
        }

        public virtual double CloseAmount
        {
            get
            {
                return this.closeAmount;
            }
            set
            {
                if (value == this.closeAmount)
                    return;
                this.closeAmount = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.CloseAmount));
            }
        }

        public virtual int CloseEur1c
        {
            get
            {
                return this.closeEur1c;
            }
            set
            {
                if (value == this.closeEur1c)
                    return;
                this.closeEur1c = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur1c));
            }
        }

        public virtual int CloseEur2c
        {
            get
            {
                return this.closeEur2c;
            }
            set
            {
                if (value == this.closeEur2c)
                    return;
                this.closeEur2c = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur2c));
            }
        }

        public virtual int CloseEur5c
        {
            get
            {
                return this.closeEur5c;
            }
            set
            {
                if (value == this.closeEur5c)
                    return;
                this.closeEur5c = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur5c));
            }
        }

        public virtual int CloseEur10c
        {
            get
            {
                return this.closeEur10c;
            }
            set
            {
                if (value == this.closeEur10c)
                    return;
                this.closeEur10c = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur10c));
            }
        }

        public virtual int CloseEur20c
        {
            get
            {
                return this.closeEur20c;
            }
            set
            {
                if (value == this.closeEur20c)
                    return;
                this.closeEur20c = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur20c));
            }
        }

        public virtual int CloseEur50c
        {
            get
            {
                return this.closeEur50c;
            }
            set
            {
                if (value == this.closeEur50c)
                    return;
                this.closeEur50c = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur50c));
            }
        }

        public virtual int CloseEur1
        {
            get
            {
                return this.closeEur1;
            }
            set
            {
                if (value == this.closeEur1)
                    return;
                this.closeEur1 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur1));
            }
        }

        public virtual int CloseEur2
        {
            get
            {
                return this.closeEur2;
            }
            set
            {
                if (value == this.closeEur2)
                    return;
                this.closeEur2 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur2));
            }
        }

        public virtual int CloseEur5
        {
            get
            {
                return this.closeEur5;
            }
            set
            {
                if (value == this.closeEur5)
                    return;
                this.closeEur5 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur5));
            }
        }

        public virtual int CloseEur10
        {
            get
            {
                return this.closeEur10;
            }
            set
            {
                if (value == this.closeEur10)
                    return;
                this.closeEur10 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur10));
            }
        }

        public virtual int CloseEur20
        {
            get
            {
                return this.closeEur20;
            }
            set
            {
                if (value == this.closeEur20)
                    return;
                this.closeEur20 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur20));
            }
        }

        public virtual int CloseEur50
        {
            get
            {
                return this.closeEur50;
            }
            set
            {
                if (value == this.closeEur50)
                    return;
                this.closeEur50 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur50));
            }
        }

        public virtual int CloseEur100
        {
            get
            {
                return this.closeEur100;
            }
            set
            {
                if (value == this.closeEur100)
                    return;
                this.closeEur100 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur100));
            }
        }

        public virtual int CloseEur200
        {
            get
            {
                return this.closeEur200;
            }
            set
            {
                if (value == this.closeEur200)
                    return;
                this.closeEur200 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur200));
            }
        }

        public virtual int CloseEur500
        {
            get
            {
                return this.closeEur500;
            }
            set
            {
                if (value == this.closeEur500)
                    return;
                this.closeEur500 = value;
                this.RecalculateTotal();
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CloseEur500));
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.CloseAmount));
            }
        }

        public void RecalculateTotal()
        {
            this.CloseAmount = (double)this.CloseEur1c * 0.01 + (double)this.CloseEur2c * 0.02 + (double)this.CloseEur5c * 0.05 + (double)this.CloseEur10c * 0.1 + (double)this.CloseEur20c * 0.2 + (double)this.CloseEur50c * 0.5 + (double)(this.CloseEur1 * 1) + (double)(this.CloseEur2 * 2) + (double)(this.CloseEur5 * 5) + (double)(this.CloseEur10 * 10) + (double)(this.CloseEur20 * 20) + (double)(this.CloseEur50 * 50) + (double)(this.CloseEur100 * 100) + (double)(this.CloseEur200 * 200) + (double)(this.CloseEur500 * 500);
        }
    }
}