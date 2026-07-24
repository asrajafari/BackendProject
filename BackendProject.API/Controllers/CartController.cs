using BackendProject.Application.DTOs.Carts.Requests;
using BackendProject.Application.DTOs.Carts.Responses;
using BackendProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendProject.API.Controllers;

[Route("api/cart")]
[ApiController]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDto request)
    {
        try
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized("User not authenticated");

            if (!Guid.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid UserId format");

            await _cartService.AddToCartAsync(userId, request);
            return Ok(new { message = "Product added to cart successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemResponseDto>>> GetCart()
    {
        try
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized("User not authenticated");

            if (!Guid.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid UserId format");

            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
    {
        try
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized("User not authenticated");

            if (!Guid.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid UserId format");

            await _cartService.RemoveFromCartAsync(userId, cartItemId);
            return Ok(new { message = "Product removed from cart" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
    {
        try
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized("User not authenticated");

            if (!Guid.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid UserId format");

            await _cartService.CheckoutAsync(userId, request);
            return Ok(new { message = "Checkout completed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}