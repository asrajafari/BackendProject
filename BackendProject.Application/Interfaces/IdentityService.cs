using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendProject.DTOs;
using BackendProject.Identity;
using BackendProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BackendProject.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<string> RegisterUserAsync(RegisterDto model)
    {
        var user = new ApplicationUser 
        { 
            UserName = model.Email, 
            Email = model.Email, 
            FirstName = model.FirstName, 
            LastName = model.LastName 
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        return result.Succeeded ? "Success" : (result.Errors.FirstOrDefault()?.Description ?? "خطای نامشخص");
    }

    public async Task<LoginResponseDto> LoginUserAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return new LoginResponseDto { Message = "ایمیل یا رمز عبور اشتباه است." };

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        
        if (!result.Succeeded) return new LoginResponseDto { Message = "ایمیل یا رمز عبور اشتباه است." };

        return new LoginResponseDto 
        { 
            Token = GenerateJwtToken(user), 
            Message = "Success" 
        };
    }

    public async Task<bool> AssignRoleToUserAsync(Guid userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[] 
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("FirstName", user.FirstName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "SuperSecretKey1234567890123456"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "YourApp",
            audience: "YourApp",
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}