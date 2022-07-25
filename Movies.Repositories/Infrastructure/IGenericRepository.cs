using Movies.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Movies.Repositories.Infrastructure
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task<T> GetById(Guid id);

        Task<T> FirstOfDefaultAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        Task<int> Create(T entity);

        Task<int> CreateBulk(List<T> entityList);

        Task<bool> Update(T entity);

        Task<int> Delete(T entity);
    }
}
