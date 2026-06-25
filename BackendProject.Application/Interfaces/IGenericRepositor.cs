using BackendProject.Entities;

namespace BackendProject.Repositories;

public interface IGenericRepository<T, TKey> where T : class, IBaseEntity<TKey>
{
    Task<T?> GetByIdAsync(TKey id);
    Task<IEnumerable<T>> GetAllAsync();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}