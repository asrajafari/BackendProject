using BackendProject.Application.Interfaces;
using BackendProject.Domain.Entities;
using BackendProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Infrastructure.Repositories;

public class CartRepository
    : GenericRepository<CartItem, Guid>, ICartRepository
{
    public CartRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<CartItem?> GetCartItemAsync(Guid userId, int productId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.ProductId == productId);
    }

    public async Task<IEnumerable<CartItem>> GetUserCartAsync(Guid userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }
}