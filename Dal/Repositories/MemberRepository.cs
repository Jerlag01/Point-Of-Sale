using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dal.Model;

namespace Dal.Repositories
{
    public class MemberRepository : IRepository<Member>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public MemberRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(MemberRepository.logger.Trace);
        }

        public void Add(Member login)
        {
            this.context.Members.Add(login);
            MemberRepository.logger.Debug<Guid, string>("Member object \"{0} - {1}\" added to database", login.Id, login.Fullname);
        }

        public void Update(Member login)
        {
            this.context.Entry<Member>(login).State = EntityState.Modified;
            MemberRepository.logger.Debug<Guid, string>("Member object \"{0} - {1}\" marked as modified", login.Id, login.Fullname);
        }

        public void Delete(Member login)
        {
            this.context.Members.Remove(login);
            MemberRepository.logger.Debug<Guid, string>("Member object \"{0} - {1}\" removed from database", login.Id, login.Fullname);
        }

        public void Attach(Member member)
        {
            this.context.Members.Attach(member);
        }

        public List<Member> GetAll()
        {
            return this.context.Members.OrderBy<Member, string>((Expression<Func<Member, string>>)(m => m.FirstName)).ThenBy<Member, string>((Expression<Func<Member, string>>)(m => m.LastName)).ToList<Member>();
        }

        public List<Member> GetAll(DateTime snapshotDate)
        {
            DateTime snashotDatePlusOne = snapshotDate.AddDays(1.0);
            return this.context.Members.Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.Count<MemberCard>((Func<MemberCard, bool>)(mc => mc.CreationTime < snashotDatePlusOne)) > 0)).OrderBy<Member, string>((Expression<Func<Member, string>>)(m => m.LastName)).ThenBy<Member, string>((Expression<Func<Member, string>>)(m => m.FirstName)).ToList<Member>();
        }

        public List<Member> GetNotExpired(DateTime snapshotDate)
        {
            return this.context.Members.Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.Count<MemberCard>((Func<MemberCard, bool>)(mc => mc.ExpireDate >= snapshotDate)) > 0)).Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.Count<MemberCard>((Func<MemberCard, bool>)(mc => mc.CreationTime <= snapshotDate)) > 0)).OrderBy<Member, string>((Expression<Func<Member, string>>)(m => m.LastName)).ThenBy<Member, string>((Expression<Func<Member, string>>)(m => m.FirstName)).ToList<Member>();
        }

        public List<Member> GetExpired(DateTime snapshotDate)
        {
            return this.context.Members.Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.Count<MemberCard>((Func<MemberCard, bool>)(mc => mc.ExpireDate >= snapshotDate)) == 0)).OrderBy<Member, string>((Expression<Func<Member, string>>)(m => m.LastName)).ThenBy<Member, string>((Expression<Func<Member, string>>)(m => m.FirstName)).ToList<Member>();
        }

        public List<Member> GetActiveNotExpired()
        {
            return this.context.Members.Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.Count > 0)).Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(c => c.CreationTime)).FirstOrDefault<MemberCard>().ActiveMember)).Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(c => c.CreationTime)).FirstOrDefault<MemberCard>().ExpireDate > DateTime.Now)).OrderBy<Member, string>((Expression<Func<Member, string>>)(m => m.FirstName)).ThenBy<Member, string>((Expression<Func<Member, string>>)(m => m.LastName)).ToList<Member>();
        }

        public List<Member> GetActive()
        {
            return this.context.Members.Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.Count > 0)).Where<Member>((Expression<Func<Member, bool>>)(m => m.MemberCards.OrderByDescending<MemberCard, DateTime>((Func<MemberCard, DateTime>)(c => c.CreationTime)).FirstOrDefault<MemberCard>().ActiveMember)).OrderBy<Member, string>((Expression<Func<Member, string>>)(m => m.FirstName)).ThenBy<Member, string>((Expression<Func<Member, string>>)(m => m.LastName)).ToList<Member>();
        }

        public Member GetById(Guid id)
        {
            return this.context.Members.FirstOrDefault<Member>((Expression<Func<Member, bool>>)(l => l.Id == id));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}