using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dal.Model;

namespace Dal.Repositories
{
    public class CheckoutSheetRepository : IRepository<CheckoutSheet>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public CheckoutSheetRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(CheckoutSheetRepository.logger.Trace);
        }

        public void Add(CheckoutSheet sheet)
        {
            this.context.CheckoutSheets.Add(sheet);
            CheckoutSheetRepository.logger.Debug<int, DateTime?>("CheckoutSheet object \"{0} - {1}\" added to database", sheet.Id, sheet.OpenTime);
        }

        public void Update(CheckoutSheet sheet)
        {
            this.context.Entry<CheckoutSheet>(sheet).State = EntityState.Modified;
            CheckoutSheetRepository.logger.Debug<int, DateTime?>("CheckoutSheet object \"{0} - {1} - {2}\" marked as modified", sheet.Id, sheet.OpenTime);
        }

        public void Delete(CheckoutSheet sheet)
        {
            this.context.CheckoutSheets.Remove(sheet);
            CheckoutSheetRepository.logger.Debug<int, DateTime?>("CheckoutSheet object \"{0} - {1}\" deleted from database", sheet.Id, sheet.OpenTime);
        }

        public void Attach(CheckoutSheet sheet)
        {
            this.context.CheckoutSheets.Attach(sheet);
        }

        public List<CheckoutSheet> GetAll()
        {
            return this.context.CheckoutSheets.ToList<CheckoutSheet>();
        }

        public CheckoutSheet GetById(int id)
        {
            return this.context.CheckoutSheets.FirstOrDefault<CheckoutSheet>((Expression<Func<CheckoutSheet, bool>>)(c => c.Id == id));
        }

        public List<CheckoutSheet> GetByOpenDate(DateTime date)
        {
            DateTime datePlusOne = date.AddDays(1.0);
            return this.context.CheckoutSheets.Where<CheckoutSheet>((Expression<Func<CheckoutSheet, bool>>)(c => c.OpenTime >= (DateTime?)date.Date)).Where<CheckoutSheet>((Expression<Func<CheckoutSheet, bool>>)(c => c.OpenTime <= (DateTime?)datePlusOne.Date)).ToList<CheckoutSheet>();
        }

        public List<CheckoutSheet> GetByCloseDate(DateTime date)
        {
            DateTime datePlusOne = date.AddDays(1.0);
            return this.context.CheckoutSheets.Where<CheckoutSheet>((Expression<Func<CheckoutSheet, bool>>)(c => c.CloseTime >= (DateTime?)date.Date)).Where<CheckoutSheet>((Expression<Func<CheckoutSheet, bool>>)(c => c.CloseTime <= (DateTime?)datePlusOne.Date)).ToList<CheckoutSheet>();
        }

        public CheckoutSheet GetLastOpenSheet()
        {
            return this.context.CheckoutSheets.Include<CheckoutSheet, Member>((Expression<Func<CheckoutSheet, Member>>)(m => m.OpenedBy)).FirstOrDefault<CheckoutSheet>((Expression<Func<CheckoutSheet, bool>>)(c => c.CloseTime == new DateTime?()));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}