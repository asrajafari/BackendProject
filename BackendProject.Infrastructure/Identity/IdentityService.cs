using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendProject.Application.DTOs.Auth;
using BackendProject.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BackendProject.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return new RegisterResponse
            {
                IsSuccess = false,
                Message = result.Errors.FirstOrDefault()?.Description ?? "خطای نامشخص"
            };
        }

        await _userManager.AddToRoleAsync(user, "User");

        return new RegisterResponse
        {
            IsSuccess = true,
            Message = "ثبت‌ نام با موفقیت انجام شد."
        };
    }

    public async Task<LoginResponse> LoginUserAsync(LoginRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "ایمیل یا رمز عبور اشتباه است."
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = result.IsLockedOut
                    ? "حساب کاربری شما به‌دلیل تلاش‌های ناموفق مکرر قفل شده است."
                    : "ایمیل یا رمز عبور اشتباه است."
            };
        }

        var token = await GenerateJwtTokenAsync(user);

        return new LoginResponse
        {
            IsSuccess = true,
            Token = token,
            Message = "ورود با موفقیت انجام شد."
        };
    }

    public async Task<AssignRoleResponse> AssignRoleToUserAsync(AssignRoleRequest model)
    {
        var validRoles = new[] { "User", "Admin", "SuperAdmin" };
        if (!validRoles.Contains(model.RoleName))
        {
            return new AssignRoleResponse
            {
                IsSuccess = false,
                Message = "رول نامعتبر است."
            };
        }

        var user = await _userManager.FindByIdAsync(model.UserId.ToString());
        if (user == null)
        {
            return new AssignRoleResponse
            {
                IsSuccess = false,
                Message = "کاربر یافت نشد."
            };
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        var result = await _userManager.AddToRoleAsync(user, model.RoleName);

        return new AssignRoleResponse
        {
            IsSuccess = result.Succeeded,
            Message = result.Succeeded
                ? "رول با موفقیت اختصاص یافت."
                : result.Errors.FirstOrDefault()?.Description ?? "خطا در اختصاص رول"
        };
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("FirstName", user.FirstName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtKey = _configuration["Jwt:Key"]
                     ?? throw new InvalidOperationException("Jwt:Key تنظیم نشده است.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "YourApp",
            audience: _configuration["Jwt:Audience"] ?? "YourApp",
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}