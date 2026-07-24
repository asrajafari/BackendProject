using System.Linq.Expressions;
using BackendProject.Domain.Entities;

namespace BackendProject.Application.Interfaces;

public interface IGenericRepository<TEntity, TKey>
    where TEntity : class, IBaseEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    IQueryable<TEntity> AsQueryable();

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    Task SaveChangesAsync();
}