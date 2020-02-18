using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dal.Model;

namespace Dal.Repositories
{
    public class RoleRepository : IRepository<Role>, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EfDbContext context;

        public RoleRepository(EfDbContext context)
        {
            this.context = context;
            this.context.Database.Log = new Action<string>(RoleRepository.logger.Trace);
        }

        public void Add(Role role)
        {
            this.context.Roles.Add(role);
            RoleRepository.logger.Debug<int, string>("Role object \"{0} - {1}\" added to database", role.Id, role.Name);
        }

        public void Update(Role role)
        {
            this.context.Entry<Role>(role).State = EntityState.Modified;
            RoleRepository.logger.Debug<int, string>("Role object \"{0} - {1}\" marked as modified", role.Id, role.Name);
        }

        public void Delete(Role role)
        {
            this.context.Roles.Remove(role);
            RoleRepository.logger.Debug<int, string>("Role object \"{0} - {1}\" removed from", role.Id, role.Name);
        }

        public void Attach(Role role)
        {
            this.context.Roles.Attach(role);
        }

        public List<Role> GetAll()
        {
            return this.context.Roles.ToList<Role>();
        }

        public Role GetById(int id)
        {
            return this.context.Roles.FirstOrDefault<Role>((Expression<Func<Role, bool>>)(l => l.Id == id));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}