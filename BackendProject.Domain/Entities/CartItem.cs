namespace BackendProject.Domain.Entities;

public class CartItem : IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.Now;

       public Product Product { get; set; } = null!;
}