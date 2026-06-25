namespace BackendProject.Entities;

public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
{
    public TKey Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } 
    
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}