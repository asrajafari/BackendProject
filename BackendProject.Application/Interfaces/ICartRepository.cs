using BackendProject.Domain.Entities;

namespace BackendProject.Application.Interfaces;

public interface ICartRepository : IGenericRepository<CartItem, Guid>
{
    Task<CartItem?> GetCartItemAsync(Guid userId, int productId);

    Task<IEnumerable<CartItem>> GetUserCartAsync(Guid userId);
}