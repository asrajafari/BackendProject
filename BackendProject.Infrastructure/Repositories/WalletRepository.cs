using BackendProject.Application.Interfaces;
using BackendProject.Domain.Entities;
using BackendProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Infrastructure.Repositories;

public class WalletRepository
    : GenericRepository<Wallet, Guid>, IWalletRepository
{
    public WalletRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Wallet?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId)
    {
        return await _context.WalletTransactions
            .Include(t => t.Wallet)
            .Where(t => t.Wallet.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}