using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dal.Model;

namespace Dal.Repositories
{
    public class MemberCardRepository : IRepository<MemberCard>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public MemberCardRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(MemberCardRepository.logger.Trace);
        }

        public void Add(MemberCard memberCard)
        {
            this.context.MemberCards.Add(memberCard);
            MemberCardRepository.logger.Debug<Guid>("MemberCard object \"{0}\" added to database", memberCard.Id);
        }

        public void Update(MemberCard memberCard)
        {
            this.context.Entry<MemberCard>(memberCard).State = EntityState.Modified;
            MemberCardRepository.logger.Debug<Guid>("MemberCard object \"{0}\" marked as modified", memberCard.Id);
        }

        public void Delete(MemberCard memberCard)
        {
            this.context.MemberCards.Remove(memberCard);
            MemberCardRepository.logger.Debug<Guid>("Member object \"{0}\" removed from database", memberCard.Id);
        }

        public void Attach(MemberCard memberCard)
        {
            this.context.MemberCards.Attach(memberCard);
        }

        public List<MemberCard> GetAll()
        {
            return this.context.MemberCards.ToList<MemberCard>();
        }

        public MemberCard GetById(Guid id)
        {
            return this.context.MemberCards.FirstOrDefault<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.Id == id));
        }

        public MemberCard GetBySmartcardId(string id)
        {
            return this.context.MemberCards.FirstOrDefault<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.SmartCardId == id));
        }

        public MemberCard GetBySmartcardIdNoTracking(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return (MemberCard)null;
            return this.context.MemberCards.AsNoTracking().Where<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.SmartCardId == id)).Include<MemberCard, Member>((Expression<Func<MemberCard, Member>>)(m => m.Member)).Include<MemberCard, ObservableCollection<Role>>((Expression<Func<MemberCard, ObservableCollection<Role>>>)(r => r.Member.Roles)).FirstOrDefault<MemberCard>();
        }

        public List<MemberCard> GetUnprintedActiveCards()
        {
            return this.context.MemberCards.Where<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.Printed == false)).Where<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.ActiveMember)).Where<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.ExpireDate > DateTime.Now)).Include<MemberCard, Member>((Expression<Func<MemberCard, Member>>)(m => m.Member)).ToList<MemberCard>();
        }

        public List<MemberCard> GetUnprintedNormalCards()
        {
            return this.context.MemberCards.Where<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.Printed == false)).Where<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.ActiveMember == false)).Where<MemberCard>((Expression<Func<MemberCard, bool>>)(l => l.ExpireDate > DateTime.Now)).Include<MemberCard, Member>((Expression<Func<MemberCard, Member>>)(m => m.Member)).ToList<MemberCard>();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}