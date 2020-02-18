using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Dal.Model
{
    [Table("CoinTransactions")]
    public class CoinTransaction : Transaction
    {
        private int coinsReceived;

        [Required]
        public virtual int CoinsReceived
        {
            get
            {
                return this.coinsReceived;
            }
            set
            {
                if (value == this.coinsReceived)
                    return;
                this.coinsReceived = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.CoinsReceived));
            }
        }
    }
}