namespace BackendProject.Domain.Entities;

public class Wallet : IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public decimal Balance { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } 

  
    public ICollection<WalletTransaction> Transactions { get; set; }
        = new List<WalletTransaction>();}