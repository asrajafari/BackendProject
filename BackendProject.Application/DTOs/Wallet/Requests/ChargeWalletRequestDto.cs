namespace BackendProject.Application.DTOs.Wallet.Requests;

public class ChargeWalletRequestDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = "Wallet charged";
}