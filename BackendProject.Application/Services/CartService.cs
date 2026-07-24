using BackendProject.Application.DTOs.Carts.Requests;
using BackendProject.Application.DTOs.Carts.Responses;
using BackendProject.Application.Interfaces;
using BackendProject.Domain.Entities;

namespace BackendProject.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IGenericRepository<WalletTransaction, Guid> _transactionRepository;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IWalletRepository walletRepository,
        IGenericRepository<WalletTransaction, Guid> transactionRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task AddToCartAsync(Guid userId, AddToCartRequestDto request)
    {
        try
        {
            if (request.Quantity <= 0)
                throw new Exception("Quantity must be greater than zero.");

            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
                throw new Exception("Product not found.");

            if (product.Stock < request.Quantity)
                throw new Exception("Not enough stock.");

            var cartItem = await _cartRepository.GetCartItemAsync(userId, request.ProductId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };

                _cartRepository.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += request.Quantity;
                _cartRepository.Update(cartItem);
            }

            await _cartRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddToCartAsync: {ex.Message}");
            throw;
        }
    }

    public async Task RemoveFromCartAsync(Guid userId, Guid cartItemId)
    {
        try
        {
            var cartItem = await _cartRepository.GetByIdAsync(cartItemId);

            if (cartItem == null || cartItem.UserId != userId)
                throw new Exception("Cart item not found.");

            _cartRepository.Delete(cartItem);

            await _cartRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RemoveFromCartAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<CartItemResponseDto>> GetCartAsync(Guid userId)
    {
        try
        {
            var items = await _cartRepository.GetUserCartAsync(userId);

            return items.Select(c => new CartItemResponseDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                Quantity = c.Quantity,
                Price = c.Product.Price,
                AddedAt = c.AddedAt
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetCartAsync: {ex.Message}");
            throw;
        }
    }

    public async Task CheckoutAsync(Guid userId, CheckoutRequestDto request)
    {
        try
        {
            var cartItems = (await _cartRepository.GetUserCartAsync(userId)).ToList();

            if (!cartItems.Any())
                throw new Exception("Cart is empty.");

            decimal total = 0;

            foreach (var item in cartItems)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new Exception($"Not enough stock for {item.Product.Name}");

                total += item.Product.Price * item.Quantity;
            }

            var wallet = await _walletRepository.GetByUserIdAsync(userId);

            if (wallet == null)
                throw new Exception("Wallet not found.");

            if (wallet.Balance < total)
                throw new Exception("Insufficient wallet balance.");

            wallet.Balance -= total;
            wallet.UpdatedAt = DateTime.Now;

            _walletRepository.Update(wallet);

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = -total,
                Type = "Purchase",
                Description = request.Description
            };

            _transactionRepository.Add(transaction);

            foreach (var item in cartItems)
            {
                item.Product.Stock -= item.Quantity;

                _productRepository.Update(item.Product);

                _cartRepository.Delete(item);
            }

            await _cartRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CheckoutAsync: {ex.Message}");
            throw;
        }
    }
}