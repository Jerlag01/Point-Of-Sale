using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dal.Model;

namespace Dal.Repositories
{
    public class TicketLineRepository : IRepository<TicketLine>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public TicketLineRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(TicketLineRepository.logger.Trace);
        }

        public void Add(TicketLine ticketLine)
        {
            this.context.TicketLines.Add(ticketLine);
            TicketLineRepository.logger.Debug<int, string>("Ticket Line object \"{0} - {1}\" added to database", ticketLine.Id, ticketLine.Product.Name);
        }

        public void Update(TicketLine ticketLine)
        {
            this.context.Entry<TicketLine>(ticketLine).State = EntityState.Modified;
            TicketLineRepository.logger.Debug<int, string>("Ticket Line object \"{0} - {1}\" marked as modified", ticketLine.Id, ticketLine.Product.Name);
        }

        public void Delete(TicketLine ticketLine)
        {
            this.context.TicketLines.Remove(ticketLine);
            TicketLineRepository.logger.Debug<int, string>("Ticket Line object \"{0} - {1}\" removed from database", ticketLine.Id, ticketLine.Product.Name);
        }

        public void Attach(TicketLine ticketLine)
        {
            this.context.TicketLines.Attach(ticketLine);
        }

        public List<TicketLine> GetAll()
        {
            return this.context.TicketLines.ToList<TicketLine>();
        }

        public TicketLine GetById(int id)
        {
            return this.context.TicketLines.FirstOrDefault<TicketLine>((Expression<Func<TicketLine, bool>>)(l => l.Id == id));
        }

        public List<TicketLine> GetByProduct(Product product)
        {
            return this.context.TicketLines.Where<TicketLine>((Expression<Func<TicketLine, bool>>)(tl => tl.Product.Id == product.Id)).ToList<TicketLine>();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
