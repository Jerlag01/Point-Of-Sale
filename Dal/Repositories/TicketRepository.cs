// Decompiled with JetBrains decompiler
// Type: Tjok.Dal.Repositories.TicketRepository
// Assembly: Tjok.Dal, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA6A112C-6289-4EDA-9367-76706BD7FA59
// Assembly location: D:\lagae\Documents\POS Tjok\SDP Technologies\Kassasysteem\Tjok.Dal.dll

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
    public class TicketRepository : IRepository<Ticket>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public TicketRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(TicketRepository.logger.Trace);
        }

        public void Add(Ticket ticket)
        {
            this.context.Tickets.Add(ticket);
            TicketRepository.logger.Debug("Ticket object \"{0}\" added to database", ticket.Id);
        }

        public void Update(Ticket ticket)
        {
            this.context.Entry<Ticket>(ticket).State = EntityState.Modified;
            TicketRepository.logger.Debug("Ticket object \"{0}\" marked as modified", ticket.Id);
        }

        public void Delete(Ticket ticket)
        {
            this.context.Tickets.Remove(ticket);
            TicketRepository.logger.Debug("Ticket object \"{0}\" removed from database", ticket.Id);
        }

        public void Attach(Ticket ticket)
        {
            this.context.Tickets.Attach(ticket);
        }

        public List<Ticket> GetAll()
        {
            return this.context.Tickets.ToList<Ticket>();
        }

        public Ticket GetById(int id)
        {
            return this.context.Tickets.FirstOrDefault<Ticket>((Expression<Func<Ticket, bool>>)(t => t.Id == id));
        }

        public List<Ticket> GetLastTicketsOfDayNoTracking(int amount)
        {
            return this.context.Tickets.AsNoTracking().Where<Ticket>((Expression<Func<Ticket, bool>>)(t => DbFunctions.TruncateTime((DateTime?)t.CreationTime) == (DateTime?)DateTime.Today)).OrderByDescending<Ticket, int>((Expression<Func<Ticket, int>>)(t => t.Id)).Include<Ticket, IEnumerable<Product>>((Expression<Func<Ticket, IEnumerable<Product>>>)(t => t.TicketLines.Select<TicketLine, Product>((Func<TicketLine, Product>)(l => l.Product)))).Include<Ticket, ObservableCollection<Transaction>>((Expression<Func<Ticket, ObservableCollection<Transaction>>>)(t => t.Transactions)).Take<Ticket>(amount).ToList<Ticket>();
        }

        public List<Ticket> GetOpenTicketsNoTracking()
        {
            return this.context.Tickets.AsNoTracking().Where<Ticket>((Expression<Func<Ticket, bool>>)(t => t.Transactions.Count == 0 || t.TotalRemaining != 0.0)).OrderByDescending<Ticket, int>((Expression<Func<Ticket, int>>)(t => t.Id)).Include<Ticket, IEnumerable<Product>>((Expression<Func<Ticket, IEnumerable<Product>>>)(t => t.TicketLines.Select<TicketLine, Product>((Func<TicketLine, Product>)(l => l.Product)))).ToList<Ticket>();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
