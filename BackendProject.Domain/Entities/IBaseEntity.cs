namespace BackendProject.Domain.Entities;

public interface IBaseEntity<TKey>
{
    TKey Id { get; }
}