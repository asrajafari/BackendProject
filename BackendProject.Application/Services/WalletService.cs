using BackendProject.Application.DTOs.Wallet.Requests;
using BackendProject.Application.DTOs.Wallet.Responses;
using BackendProject.Application.Interfaces;
using BackendProject.Domain.Entities;

namespace BackendProject.Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IGenericRepository<WalletTransaction, Guid> _transactionRepository;

    public WalletService(
        IWalletRepository walletRepository,
        IGenericRepository<WalletTransaction, Guid> transactionRepository)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<WalletResponseDto> GetWalletAsync(Guid userId)
    {
        try
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);

            if (wallet == null)
                throw new Exception("Wallet not found.");

            return new WalletResponseDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetWalletAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<WalletResponseDto> ChargeWalletAsync(Guid userId, ChargeWalletRequestDto request)
    {
        try
        {
            if (request.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            var wallet = await _walletRepository.GetByUserIdAsync(userId);

            if (wallet == null)
                throw new Exception("Wallet not found.");

            wallet.Balance += request.Amount;
            wallet.UpdatedAt = DateTime.Now;

            _walletRepository.Update(wallet);

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = request.Amount,
                Type = "Charge",
                Description = request.Description
            };

            _transactionRepository.Add(transaction);

            await _walletRepository.SaveChangesAsync();

            return new WalletResponseDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ChargeWalletAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<WalletTransactionResponseDto>> GetTransactionsAsync(Guid userId)
    {
        try
        {
            var transactions = await _walletRepository.GetTransactionsAsync(userId);

            return transactions.Select(t => new WalletTransactionResponseDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                CreatedAt = t.CreatedAt
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetTransactionsAsync: {ex.Message}");
            throw;
        }
    }
}