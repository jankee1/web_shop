using System.Linq.Expressions;

namespace Web_Shop.Persistence.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IGenericRepository<T> WithTracking();
        IGenericRepository<T> WithoutTracking();

        IQueryable<T> Entities { get; }

        Task<T?> GetByIdAsync(params object?[]? id);
        Task<ICollection<T>> GetManyByIdAsync<TKey>(ICollection<TKey> ids,Expression<Func<T, TKey>> keySelector);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity, params object?[]? id);
        Task DeleteAsync(T entity);
        Task<bool> Exists(params object?[]? id);
    }
}
