using BackendProject.Application.DTOs.Auth;
using BackendProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _identityService.RegisterUserAsync(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _identityService.LoginUserAsync(request);

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AssignRole(AssignRoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _identityService.AssignRoleToUserAsync(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}