using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class TaxCategory : ObservableObject
    {
        private int id;
        private string name;
        private double taxPercentage;

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
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (!(value != this.name))
                    return;
                this.name = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.Name));
            }
        }

        [Required]
        public virtual double TaxPercentage
        {
            get
            {
                return this.taxPercentage;
            }
            set
            {
                if (value == this.taxPercentage)
                    return;
                this.taxPercentage = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.TaxPercentage));
            }
        }
    }
}