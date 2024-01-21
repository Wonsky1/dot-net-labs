using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly NewsDbContext _context;

    public GenericRepository(NewsDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params string[] includes)
    {
        var query = _dbSet.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.ToListAsync();
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, params string[] includes)
    {
        var query = _dbSet.AsQueryable();

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        await Task.Run(() =>
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        });
    }

    public async Task DeleteAsync(T entity)
    {
        await Task.Run(() => _dbSet.Remove(entity));
    }
}
