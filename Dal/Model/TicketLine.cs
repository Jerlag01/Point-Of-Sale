using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class TicketLine : ObservableObject
    {
        private int id;
        private Product product;
        private double unitPrice;
        private int unitPriceCoins;
        private int linePriceCoins;
        private short amount;
        private double linePriceExcl;
        private double linePriceIncl;
        private double vatPercentage;
        private double vatValue;

        [Key]
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
        public virtual Product Product
        {
            get
            {
                return this.product;
            }
            set
            {
                if (value == this.product)
                    return;
                this.product = value;
                this.OnPropertyChanged<Product>((Expression<Func<Product>>)(() => this.Product));
            }
        }

        [Required]
        public virtual double UnitPrice
        {
            get
            {
                return this.unitPrice;
            }
            set
            {
                if (value == this.unitPrice)
                    return;
                this.unitPrice = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.UnitPrice));
            }
        }

        public virtual int UnitPriceCoins
        {
            get
            {
                return this.unitPriceCoins;
            }
            set
            {
                if (value == this.unitPriceCoins)
                    return;
                this.unitPriceCoins = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.UnitPriceCoins));
            }
        }

        public virtual int LinePriceCoins
        {
            get
            {
                return this.linePriceCoins;
            }
            set
            {
                if (value == this.linePriceCoins)
                    return;
                this.linePriceCoins = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.LinePriceCoins));
            }
        }

        [Required]
        public virtual short Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                if ((int)value == (int)this.amount)
                    return;
                this.amount = value;
                this.OnPropertyChanged<short>((Expression<Func<short>>)(() => this.Amount));
            }
        }

        [Required]
        public virtual double LinePriceExcl
        {
            get
            {
                return this.linePriceExcl;
            }
            set
            {
                if (value == this.linePriceExcl)
                    return;
                this.linePriceExcl = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.LinePriceExcl));
            }
        }

        [Required]
        public virtual double VatPercentage
        {
            get
            {
                return this.vatPercentage;
            }
            set
            {
                if (value == this.vatPercentage)
                    return;
                this.vatPercentage = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.VatPercentage));
            }
        }

        [Required]
        public virtual double VatValue
        {
            get
            {
                return this.vatValue;
            }
            set
            {
                if (value == this.vatValue)
                    return;
                this.vatValue = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.VatValue));
            }
        }

        [Required]
        public virtual double LinePriceIncl
        {
            get
            {
                return this.linePriceIncl;
            }
            set
            {
                if (value == this.linePriceIncl)
                    return;
                this.linePriceIncl = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.LinePriceIncl));
            }
        }

        public void RecalculatePrice()
        {
            this.LinePriceCoins = this.UnitPriceCoins * (int)this.Amount;
            this.LinePriceExcl = this.UnitPrice * (double)this.Amount;
            this.LinePriceIncl = this.LinePriceExcl + this.LinePriceExcl * this.Product.TaxCategory.TaxPercentage / 100.0;
        }
    }
}