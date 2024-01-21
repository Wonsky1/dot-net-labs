using System.Linq.Expressions;

namespace DAL.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params string[] includes);
    Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, params string[] includes);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
