namespace BackendProject.Domain.Entities;

public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    where TKey : notnull
{
    public TKey Id { get; protected set; } = default!;

    public DateTime CreatedAt { get; protected set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public Guid? ModifiedBy { get; protected set; }

    public void MarkAsUpdated(Guid? modifiedBy = null)
    {
        UpdatedAt = DateTime.Now;
        ModifiedBy = modifiedBy;
    }
}