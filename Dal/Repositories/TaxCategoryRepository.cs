using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dal.Model;

namespace Dal.Repositories
{
    public class TaxCategoryRepository : IRepository<TaxCategory>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public TaxCategoryRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(TaxCategoryRepository.logger.Trace);
        }

        public void Add(TaxCategory taxCategory)
        {
            this.context.TaxCategories.Add(taxCategory);
            TaxCategoryRepository.logger.Debug<int, string>("Tax category object \"{0} - {1}\" added to database", taxCategory.Id, taxCategory.Name);
        }

        public void Update(TaxCategory taxCategory)
        {
            this.context.Entry<TaxCategory>(taxCategory).State = EntityState.Modified;
            TaxCategoryRepository.logger.Debug<int, string>("Tax category  object \"{0} - {1}\" marked as modified", taxCategory.Id, taxCategory.Name);
        }

        public void Delete(TaxCategory taxCategory)
        {
            this.context.TaxCategories.Remove(taxCategory);
            TaxCategoryRepository.logger.Debug<int, string>("Tax category object \"{0} - {1}\" removed from database", taxCategory.Id, taxCategory.Name);
        }

        public void Attach(TaxCategory taxCategory)
        {
            this.context.TaxCategories.Attach(taxCategory);
        }

        public List<TaxCategory> GetAll()
        {
            return this.context.TaxCategories.ToList<TaxCategory>();
        }

        public TaxCategory GetById(int id)
        {
            return this.context.TaxCategories.FirstOrDefault<TaxCategory>((Expression<Func<TaxCategory, bool>>)(l => l.Id == id));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
