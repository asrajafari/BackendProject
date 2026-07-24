using BackendProject.Application.DTOs.Wallet.Requests;
using BackendProject.Application.DTOs.Wallet.Responses;
using BackendProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendProject.API.Controllers;

[Route("api/wallet")]
[ApiController]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }

    [HttpGet]
    public async Task<ActionResult<WalletResponseDto>> GetWallet()
    {
        try
        {
            // پیدا کردن Guid (دومین NameIdentifier یا sub)
            var userIdStr = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && Guid.TryParse(c.Value, out _))?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid UserId format");

            var wallet = await _walletService.GetWalletAsync(userId);
            return Ok(wallet);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("charge")]
    public async Task<ActionResult<WalletResponseDto>> Charge([FromBody] ChargeWalletRequestDto request)
    {
        try
        {
            var userIdStr = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && Guid.TryParse(c.Value, out _))?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid UserId format");

            var result = await _walletService.ChargeWalletAsync(userId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("transactions")]
    public async Task<ActionResult<IEnumerable<WalletTransactionResponseDto>>> GetTransactions()
    {
        try
        {
            var userIdStr = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && Guid.TryParse(c.Value, out _))?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid UserId format");

            var transactions = await _walletService.GetTransactionsAsync(userId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}