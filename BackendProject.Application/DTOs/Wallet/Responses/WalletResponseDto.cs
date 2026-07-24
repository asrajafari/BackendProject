namespace BackendProject.Application.DTOs.Wallet.Responses;

public class WalletResponseDto
{
    public  Guid Id { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
}