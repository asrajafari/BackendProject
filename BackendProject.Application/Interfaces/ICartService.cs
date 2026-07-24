using BackendProject.Application.DTOs.Carts.Requests;
using BackendProject.Application.DTOs.Carts.Responses;

namespace BackendProject.Application.Interfaces;

public interface ICartService
{
    Task AddToCartAsync(Guid userId, AddToCartRequestDto request);
    Task RemoveFromCartAsync(Guid userId, Guid cartItemId);
    Task<IEnumerable<CartItemResponseDto>> GetCartAsync(Guid userId);
    Task CheckoutAsync(Guid userId, CheckoutRequestDto request);
}