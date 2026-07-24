using BackendProject.Domain.Entities;

namespace BackendProject.Application.Interfaces;

public interface IWalletRepository : IGenericRepository<Wallet, Guid>
{
    Task<Wallet?> GetByUserIdAsync(Guid userId);

    Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId);
}