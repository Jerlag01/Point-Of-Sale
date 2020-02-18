using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class Product : ObservableObject
    {
        private int id;
        private string name;
        private double price;
        private int priceCoin;
        private TaxCategory taxCategory;
        private Category category;
        private DateTime dateAdded;
        private Member addedBy;
        private bool isHdden;
        private string picturePath;
        private int xpos;
        private int ypos;

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
        public virtual double Price
        {
            get
            {
                return this.price;
            }
            set
            {
                if (value == this.price)
                    return;
                this.price = value;
                this.OnPropertyChanged<double>((Expression<Func<double>>)(() => this.Price));
            }
        }

        public virtual int PriceCoin
        {
            get
            {
                return this.priceCoin;
            }
            set
            {
                if (value == this.priceCoin)
                    return;
                this.priceCoin = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.PriceCoin));
            }
        }

        public virtual TaxCategory TaxCategory
        {
            get
            {
                return this.taxCategory;
            }
            set
            {
                if (value == this.taxCategory)
                    return;
                this.taxCategory = value;
                this.OnPropertyChanged<TaxCategory>((Expression<Func<TaxCategory>>)(() => this.TaxCategory));
            }
        }

        public virtual Category Category
        {
            get
            {
                return this.category;
            }
            set
            {
                if (value == this.category)
                    return;
                this.category = value;
                this.OnPropertyChanged<Category>((Expression<Func<Category>>)(() => this.Category));
            }
        }

        [Required]
        public virtual DateTime DateAdded
        {
            get
            {
                return this.dateAdded;
            }
            set
            {
                if (!(value != this.dateAdded))
                    return;
                this.dateAdded = value;
                this.OnPropertyChanged<DateTime>((Expression<Func<DateTime>>)(() => this.DateAdded));
            }
        }

        public virtual Member AddedBy
        {
            get
            {
                return this.addedBy;
            }
            set
            {
                if (value == this.addedBy)
                    return;
                this.addedBy = value;
                this.OnPropertyChanged<Member>((Expression<Func<Member>>)(() => this.AddedBy));
            }
        }

        [Required]
        public virtual bool IsHidden
        {
            get
            {
                return this.isHdden;
            }
            set
            {
                if (value == this.isHdden)
                    return;
                this.isHdden = value;
                this.OnPropertyChanged<bool>((Expression<Func<bool>>)(() => this.IsHidden));
            }
        }

        public virtual string PicturePath
        {
            get
            {
                return this.picturePath;
            }
            set
            {
                if (!(value != this.picturePath))
                    return;
                this.picturePath = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.PicturePath));
            }
        }

        public virtual int Xpos
        {
            get
            {
                return this.xpos;
            }
            set
            {
                if (value == this.xpos)
                    return;
                this.xpos = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.Xpos));
            }
        }

        public virtual int YPos
        {
            get
            {
                return this.ypos;
            }
            set
            {
                if (value == this.ypos)
                    return;
                this.ypos = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.YPos));
            }
        }
    }
}