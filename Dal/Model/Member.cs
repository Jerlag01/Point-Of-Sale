using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class Member : ObservableObject, IDataErrorInfo
    {
        private Guid id;
        private string firstName;
        private string lastName;
        private string zipCode;
        private string city;
        private string address;
        private Member.Genders gender;
        private DateTime? birthDate;
        private string email;
        private string telephoneNr;
        private string country;
        private Member createdBy;
        private string rrNumber;

        public Member()
        {
            this.Roles = new ObservableCollection<Role>();
            this.MemberCards = new ObservableCollection<MemberCard>();
            this.id = Guid.NewGuid();
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if (!(value != this.id))
                    return;
                this.id = value;
                this.OnPropertyChanged<Guid>((Expression<Func<Guid>>)(() => this.Id));
            }
        }

        public virtual string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                if (!(value != this.firstName))
                    return;
                this.firstName = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.FirstName));
            }
        }

        public virtual string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                if (!(value != this.lastName))
                    return;
                this.lastName = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.LastName));
            }
        }

        public virtual Member.Genders Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                if (value == this.gender)
                    return;
                this.gender = value;
                this.OnPropertyChanged<Member.Genders>((Expression<Func<Member.Genders>>)(() => this.Gender));
            }
        }

        public virtual string ZipCode
        {
            get
            {
                return this.zipCode;
            }
            set
            {
                if (!(value != this.zipCode))
                    return;
                this.zipCode = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.ZipCode));
            }
        }

        public virtual string City
        {
            get
            {
                return this.city;
            }
            set
            {
                if (!(value != this.city))
                    return;
                this.city = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.City));
            }
        }

        public virtual string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                if (!(value != this.address))
                    return;
                this.address = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.Address));
            }
        }

        public virtual DateTime? BirthDate
        {
            get
            {
                return this.birthDate;
            }
            set
            {
                DateTime? nullable = value;
                DateTime? birthDate = this.birthDate;
                if ((nullable.HasValue == birthDate.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != birthDate.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
                    return;
                this.birthDate = value;
                this.OnPropertyChanged<DateTime?>((Expression<Func<DateTime?>>)(() => this.BirthDate));
            }
        }

        public virtual string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (!(value != this.email))
                    return;
                this.email = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.Email));
            }
        }

        public virtual string TelephoneNr
        {
            get
            {
                return this.telephoneNr;
            }
            set
            {
                if (!(value != this.telephoneNr))
                    return;
                this.telephoneNr = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.TelephoneNr));
            }
        }

        public virtual string Country
        {
            get
            {
                return this.country;
            }
            set
            {
                if (!(value != this.country))
                    return;
                this.country = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.Country));
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

        public virtual string RrNumber
        {
            get
            {
                return this.rrNumber;
            }
            set
            {
                if (!(value != this.rrNumber))
                    return;
                this.rrNumber = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.RrNumber));
            }
        }

        public virtual ObservableCollection<Role> Roles { get; set; }

        public virtual ObservableCollection<MemberCard> MemberCards { get; set; }

        [NotMapped]
        public virtual string Fullname
        {
            get
            {
                return this.FirstName != string.Empty && this.LastName != string.Empty ? this.FirstName + " " + this.LastName : "";
            }
        }

        [NotMapped]
        public string Error
        {
            get
            {
                return (string)null;
            }
        }

        [NotMapped]
        public string this[string columnName]
        {
            get
            {
                string str = (string)null;
                if (columnName == "FirstName" && string.IsNullOrWhiteSpace(this.FirstName))
                    str = "Please enter a First Name";
                if (columnName == "LastName" && string.IsNullOrWhiteSpace(this.LastName))
                    str = "Please enter a Last Name";
                if (columnName == "ZipCode" && string.IsNullOrWhiteSpace(this.ZipCode))
                    str = "Please enter a ZIP code";
                if (columnName == "City" && string.IsNullOrWhiteSpace(this.City))
                    str = "Please enter a City";
                if (columnName == "Address" && string.IsNullOrWhiteSpace(this.Address))
                    str = "Please enter an Address";
                if (columnName == "Email" && string.IsNullOrWhiteSpace(this.Email))
                    str = "Please enter an Email address";
                if (columnName == "TelephoneNr" && string.IsNullOrWhiteSpace(this.TelephoneNr))
                    str = "Please enter a telephone number";
                if (columnName == "Country" && string.IsNullOrWhiteSpace(this.Country))
                    str = "Please enter a country ISO code (e.g. BE, NL, FR, ...";
                return str;
            }
        }

        public enum Genders
        {
            Male,
            Female,
        }
    }
}