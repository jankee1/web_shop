using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Web_Shop.Persistence.Repositories.Interfaces;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly WwsishopContext _dbContext;

        internal DbSet<T> dbSet;

        private bool _tracking = true;


        public GenericRepository(WwsishopContext dbContext)
        {
            _dbContext = dbContext;

            dbSet = _dbContext.Set<T>();
        }

        public virtual IQueryable<T> Entities => GetEntities();

        public IGenericRepository<T> WithTracking()
        {
            _tracking = true;
            return this;
        }
        public IGenericRepository<T> WithoutTracking()
        {
            _tracking = false;
            return this;
        }

        public virtual async Task<T?> GetByIdAsync(params object?[]? id)
        {
            if (_tracking)
            {
                return await dbSet.FindAsync(id);
            }

            var entity = await dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public virtual async Task<ICollection<T>> GetManyByIdAsync<TKey>(ICollection<TKey> ids, Expression<Func<T, TKey>> keySelector)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<T>();
            }

            IQueryable<T> query = dbSet.Where(entity => ids.Contains(keySelector.Compile().Invoke(entity)));

            var parameter = keySelector.Parameters.Single();
            var body = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Contains),
                new[] { typeof(TKey) },
                Expression.Constant(ids),
                keySelector.Body
            );

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

            query = dbSet.Where(lambda);

            return await query.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity, params object?[]? id)
        {
            var task = await Task.Run(() => _dbContext.Update(entity));

            return entity;
        }

        public virtual Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);

            return Task.CompletedTask;
        }

        //public virtual Task DeleteAsync<TProperty>(
        //    T entity,
        //    List<Expression<Func<T, IEnumerable<TProperty>>>>? relations = null)
        //    where TProperty : class
        //{
        //    if(relations != null)
        //    {
        //        foreach (var includeExpression in relations)
        //        {
        //            var collectionEntry = _dbContext.Entry(entity).Collection(includeExpression);
        //            collectionEntry.Load();
        //            var relatedEntities = collectionEntry.CurrentValue;

        //            if (relatedEntities != null)
        //            {
        //                _dbContext.RemoveRange(relatedEntities.Cast<TProperty>());
        //            }
        //        }
        //    }

        //    _dbContext.Remove(entity);

        //    return Task.CompletedTask;
        //}

        public virtual async Task<bool> Exists(params object?[]? id)
        {
            var entity = await dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }

            return entity != null;
        }

        private IQueryable<T> GetEntities()
        { 
            return _tracking ? dbSet : dbSet.AsNoTracking();
        }
    }
}
