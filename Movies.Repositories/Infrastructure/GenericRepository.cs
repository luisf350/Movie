using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Entities.Context;
using Movies.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Movies.Repositories.Infrastructure
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : EntityBase
    {
        protected readonly AppDbContext Context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger _logger;

        private bool _disposed;

        public GenericRepository(AppDbContext context, ILogger logger)
        {
            Context = context;
            _logger = logger;
            _dbSet = context.Set<T>();
        }

        public Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return Task.FromResult(query);
        }

        public async Task<T> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> FirstOfDefaultAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(filter);
            if (includes.Any())
            {
                foreach (Expression<Func<T, object>> include in includes)
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> Create(T entity)
        {
            try
            {
                entity.CreationDate = DateTimeOffset.UtcNow;
                entity.ModificationDate = DateTimeOffset.MinValue;

                await _dbSet.AddAsync(entity);
                return await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex);
                return 0;
            }
        }

        public async Task<int> CreateBulk(List<T> entityList)
        {
            try
            {
                entityList.ForEach(entity =>
                {
                    entity.CreationDate = DateTimeOffset.UtcNow;
                    entity.ModificationDate = DateTimeOffset.MinValue;
                });

                await _dbSet.AddRangeAsync(entityList);
                return await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex);
                return 0;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                entity.ModificationDate = DateTimeOffset.UtcNow;

                _dbSet.Update(entity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex);
                return false;
            }
        }

        public async Task<int> Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex);
                return 0;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}