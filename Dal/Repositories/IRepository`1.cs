using System.Collections.Generic;

namespace Dal.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();

        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        void Attach(T entity);
    }
}