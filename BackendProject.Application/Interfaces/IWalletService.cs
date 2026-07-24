using BackendProject.Application.DTOs.Wallet.Requests;
using BackendProject.Application.DTOs.Wallet.Responses;

namespace BackendProject.Application.Interfaces;

public interface IWalletService
{
    Task<WalletResponseDto> GetWalletAsync(Guid UserId);
    Task<WalletResponseDto> ChargeWalletAsync(Guid UserId, ChargeWalletRequestDto request);
    Task<IEnumerable<WalletTransactionResponseDto>> GetTransactionsAsync(Guid UserId);
}