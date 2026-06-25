using BackendProject.DTOs;
using BackendProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers;

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
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var message = await _identityService.RegisterUserAsync(model);

            if (message == "Success")
            {
                return Ok(new { message = "ثبت ‌نام با موفقیت انجام شد." });
            }

            return BadRequest(new { message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطای داخلی سرور", details = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _identityService.LoginUserAsync(model);
        
            if (response.Message == "Success")
            {
                return Ok(new { token = response.Token });
            }
        
            return Unauthorized(new { message = response.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطای داخلی سرور", details = ex.Message });
        }
    }
}