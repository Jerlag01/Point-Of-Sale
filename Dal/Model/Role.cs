using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class Role : ObservableObject
    {
        private int id;
        private string name;

        public Role()
        {
            this.Logins = new ObservableCollection<Member>();
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

        public virtual ObservableCollection<Member> Logins { get; set; }
    }
}