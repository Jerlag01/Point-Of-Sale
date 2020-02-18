using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Dal.Model
{
    [Table("FreeTransactions")]
    public class FreeTransaction : Transaction
    {
        private string reason;

        [Required]
        public virtual string Reason
        {
            get
            {
                return this.reason;
            }
            set
            {
                if (!(value != this.reason))
                    return;
                this.reason = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.Reason));
            }
        }
    }
}