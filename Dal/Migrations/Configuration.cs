using NLog;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Dal.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DbContext>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "POS.Dal.EfDbContext";
        }

        protected override void Seed(DbContext context)
        {
            Configuration.logger.Info("Database migration completed");
        }
    }
}