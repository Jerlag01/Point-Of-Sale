using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Dal.Model
{
    [Table("CashTransactions")]
    public class CashTransaction : Transaction
    {
        private double moneyReceived;
        private double moneyReturned;

        [Required]
        public virtual double MoneyReceived
        {
            get
            {
                return this.moneyReceived;
            }
            set
            {
                if (value == this.moneyReceived)
                    return;
                this.moneyReceived = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.MoneyReceived));
            }
        }

        [Required]
        public virtual double MoneyReturned
        {
            get
            {
                return this.moneyReturned;
            }
            set
            {
                if (value == this.moneyReturned)
                    return;
                this.moneyReturned = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.MoneyReturned));
            }
        }
    }
}