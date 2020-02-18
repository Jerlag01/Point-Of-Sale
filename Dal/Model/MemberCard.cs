using System;
using System.Linq.Expressions;
using Util.MicroMvvm;

namespace Dal.Model
{
    public class MemberCard : ObservableObject
    {
        private Guid id;
        private Member member;
        private DateTime creationTime;
        private DateTime expireDate;
        private bool activeMember;
        private string smartCardId;
        private Member createdBy;
        private bool printed;
        private bool blocked;

        public MemberCard()
        {
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

        public virtual Member Member
        {
            get
            {
                return this.member;
            }
            set
            {
                if (value == this.member)
                    return;
                this.member = value;
                this.OnPropertyChanged<Member>((Expression<Func<Member>>)(() => this.Member));
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

        public virtual DateTime CreationTime
        {
            get
            {
                return this.creationTime;
            }
            set
            {
                if (!(value != this.creationTime))
                    return;
                this.creationTime = value;
                this.OnPropertyChanged<DateTime>((Expression<Func<DateTime>>)(() => this.CreationTime));
            }
        }

        public virtual DateTime ExpireDate
        {
            get
            {
                return this.expireDate;
            }
            set
            {
                if (!(value != this.expireDate))
                    return;
                this.expireDate = value;
                this.OnPropertyChanged<DateTime>((Expression<Func<DateTime>>)(() => this.ExpireDate));
            }
        }

        public virtual bool ActiveMember
        {
            get
            {
                return this.activeMember;
            }
            set
            {
                if (value == this.activeMember)
                    return;
                this.activeMember = value;
                this.OnPropertyChanged<bool>((Expression<Func<bool>>)(() => this.ActiveMember));
            }
        }

        public virtual string SmartCardId
        {
            get
            {
                return this.smartCardId;
            }
            set
            {
                if (!(value != this.smartCardId))
                    return;
                this.smartCardId = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.SmartCardId));
            }
        }

        public virtual bool Printed
        {
            get
            {
                return this.printed;
            }
            set
            {
                if (value == this.printed)
                    return;
                this.printed = value;
                this.OnPropertyChanged<bool>((Expression<Func<bool>>)(() => this.Printed));
            }
        }

        public virtual bool Blocked
        {
            get
            {
                return this.blocked;
            }
            set
            {
                if (value == this.blocked)
                    return;
                this.blocked = value;
                this.OnPropertyChanged<bool>((Expression<Func<bool>>)(() => this.Blocked));
            }
        }
    }
}