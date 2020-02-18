using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dal.Model;

namespace Dal.Repositories
{
    public class TransactionRepository : IRepository<Transaction>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public TransactionRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(TransactionRepository.logger.Trace);
        }

        public void Add(Transaction transaction)
        {
            this.context.Transactions.Add(transaction);
            TransactionRepository.logger.Debug("Transaction object \"{0} - {1}\" added to database", transaction.Id);
        }

        public void Update(Transaction transaction)
        {
            this.context.Entry<Transaction>(transaction).State = EntityState.Modified;
            TransactionRepository.logger.Debug("Transaction object \"{0} - {1}\" marked as modified", transaction.Id);
        }

        public void Delete(Transaction transaction)
        {
            this.context.Transactions.Remove(transaction);
            TransactionRepository.logger.Debug("Transaction object \"{0} - {1}\" deleted from database", transaction.Id);
        }

        public void Attach(Transaction transaction)
        {
            this.context.Transactions.Attach(transaction);
        }

        public List<Transaction> GetAll()
        {
            return this.context.Transactions.ToList<Transaction>();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}