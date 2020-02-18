using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class Category : ObservableObject
    {
        private int id;
        private string name;
        private string imagePath;
        private int order;
        private ObservableCollection<Product> products;

        public Category()
        {
            this.products = new ObservableCollection<Product>();
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

        public virtual string ImagePath
        {
            get
            {
                return this.imagePath;
            }
            set
            {
                if (!(value != this.imagePath))
                    return;
                this.imagePath = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.ImagePath));
            }
        }

        public virtual int Order
        {
            get
            {
                return this.order;
            }
            set
            {
                if (value == this.order)
                    return;
                this.order = value;
                this.OnPropertyChanged<int>((Expression<Func<int>>)(() => this.Order));
            }
        }

        public virtual ObservableCollection<Product> Products
        {
            get
            {
                return this.products;
            }
            set
            {
                this.products = value;
            }
        }

        [NotMapped]
        public virtual ObservableCollection<Product> VisibleProducts
        {
            get
            {
                return new ObservableCollection<Product>(this.products.Where<Product>((Func<Product, bool>)(x => !x.IsHidden)));
            }
        }
    }
}