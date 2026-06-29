using BackendProject.Domain.Entities;

namespace BackendProject.Application.Interfaces;

public interface IGenericRepository<TEntity, TKey>
    where TEntity : class, IBaseEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    Task SaveChangesAsync();
}