using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using Dal.Migrations;
using Dal.Model;

namespace Dal
{
    public class EfDbContext : DbContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public EfDbContext()
          : base(Settings.ConnectionString ?? "")
        {
            Database.SetInitializer<EfDbContext>((IDatabaseInitializer<EfDbContext>) new MigrateDatabaseToLatestVersion<EfDbContext, Configuration>());
            EfDbContext.logger.Debug("Database entity context initialized");
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<TaxCategory> TaxCategories { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketLine> TicketLines { get; set; }

        public DbSet<MemberCard> MemberCards { get; set; }

        public DbSet<CheckoutSheet> CheckoutSheets { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Ticket>().HasMany<TicketLine>((Expression<Func<Ticket, ICollection<TicketLine>>>)(p => p.TicketLines)).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<Ticket>().HasMany<Transaction>((Expression<Func<Ticket, ICollection<Transaction>>>)(p => p.Transactions)).WithOptional((Expression<Func<Transaction, Ticket>>)(m => m.Ticket)).WillCascadeOnDelete(true);
            modelBuilder.Entity<Member>().HasMany<MemberCard>((Expression<Func<Member, ICollection<MemberCard>>>)(p => p.MemberCards)).WithRequired((Expression<Func<MemberCard, Member>>)(m => m.Member));
            modelBuilder.Entity<CheckoutSheet>().HasMany<Ticket>((Expression<Func<CheckoutSheet, ICollection<Ticket>>>)(p => p.Tickets)).WithOptional((Expression<Func<Ticket, CheckoutSheet>>)(m => m.CheckoutSheet)).WillCascadeOnDelete(true);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new FormattedDbEntityValidationException(ex);
            }
        }

        ~EfDbContext()
        {
            EfDbContext.logger.Debug("Database entity context destroyed");
        }
    }
}