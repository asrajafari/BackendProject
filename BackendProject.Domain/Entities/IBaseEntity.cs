namespace BackendProject.Entities;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
}