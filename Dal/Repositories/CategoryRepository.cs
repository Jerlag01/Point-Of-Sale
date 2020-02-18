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
    public class CategoryRepository : IRepository<Category>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public CategoryRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(CategoryRepository.logger.Trace);
        }

        public void Add(Category category)
        {
            this.context.Categories.Add(category);
            CategoryRepository.logger.Debug<int, string>("Category object \"{0} - {1}\" added to database", category.Id, category.Name);
        }

        public void Update(Category category)
        {
            this.context.Entry<Category>(category).State = EntityState.Modified;
            CategoryRepository.logger.Debug<int, string>("Category object \"{0} - {1}\" marked as modified", category.Id, category.Name);
        }

        public void Delete(Category category)
        {
            this.context.Categories.Remove(category);
            CategoryRepository.logger.Debug<int, string>("Category object \"{0} - {1}\" deleted from database", category.Id, category.Name);
        }

        public void Attach(Category category)
        {
            this.context.Categories.Attach(category);
        }

        public List<Category> GetAll()
        {
            return this.context.Categories.Include<Category, ObservableCollection<Product>>((Expression<Func<Category, ObservableCollection<Product>>>)(x => x.Products)).ToList<Category>();
        }

        public Category GetById(int id)
        {
            return this.context.Categories.FirstOrDefault<Category>((Expression<Func<Category, bool>>)(l => l.Id == id));
        }

        public Category GetByName(string name)
        {
            return this.context.Categories.FirstOrDefault<Category>((Expression<Func<Category, bool>>)(l => l.Name == name));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}