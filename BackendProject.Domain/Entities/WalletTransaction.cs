namespace BackendProject.Domain.Entities;

public class WalletTransaction : IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid WalletId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Wallet Wallet { get; set; } = null!;
}