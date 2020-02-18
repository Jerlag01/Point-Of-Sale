using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dal.Model;

namespace Dal.Repositories
{
    public class ProductRepository : IRepository<Product>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public ProductRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(ProductRepository.logger.Trace);
        }

        public void Add(Product product)
        {
            this.context.Products.Add(product);
            ProductRepository.logger.Debug<int, string>("Product object \"{0} - {1}\" added to database", product.Id, product.Name);
        }

        public void Update(Product product)
        {
            this.context.Entry<Product>(product).State = EntityState.Modified;
            ProductRepository.logger.Debug<int, string>("Product object \"{0} - {1}\" marked as modified", product.Id, product.Name);
        }

        public void Delete(Product product)
        {
            this.context.Products.Remove(product);
            ProductRepository.logger.Debug<int, string>("Product object \"{0} - {1}\" removed from database", product.Id, product.Name);
        }

        public void Attach(Product product)
        {
            this.context.Products.Attach(product);
        }

        public List<Product> GetAll()
        {
            return this.context.Products.Include<Product, TaxCategory>((Expression<Func<Product, TaxCategory>>)(x => x.TaxCategory)).Include<Product, Member>((Expression<Func<Product, Member>>)(x => x.AddedBy)).ToList<Product>();
        }

        public List<Product> GetHidden()
        {
            return this.context.Products.Where<Product>((Expression<Func<Product, bool>>)(p => p.IsHidden)).ToList<Product>();
        }

        public Product GetById(int id)
        {
            return this.context.Products.FirstOrDefault<Product>((Expression<Func<Product, bool>>)(l => l.Id == id));
        }

        public Product GetByName(string name)
        {
            return this.context.Products.FirstOrDefault<Product>((Expression<Func<Product, bool>>)(l => l.Name == name));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}