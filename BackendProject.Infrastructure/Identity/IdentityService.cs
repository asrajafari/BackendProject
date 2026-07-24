using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendProject.Application.DTOs.Auth;
using BackendProject.Application.Interfaces;
using BackendProject.Domain.Entities;
using BackendProject.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BackendProject.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto model)
    {
        try
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
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault()?.Description ?? "Unknown error"
                };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var wallet = new Wallet
            {
                UserId = user.Id,
                Balance = 0
            };

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            var token = await GenerateJwtTokenAsync(user);

            return new RegisterResponseDto
            {
                IsSuccess = true,
                Token = token,
                Message = "Registration successful."
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RegisterUserAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                model.Password,
                lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = result.IsLockedOut
                        ? "Your account has been locked due to too many failed login attempts."
                        : "Invalid email or password."
                };
            }

            var token = await GenerateJwtTokenAsync(user);

            return new LoginResponseDto
            {
                IsSuccess = true,
                Token = token,
                Message = "Login successful."
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LoginUserAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<AssignRoleResponseDto> AssignRoleToUserAsync(AssignRoleRequestDto model)
    {
        try
        {
            var validRoles = new[] { "User", "Admin", "SuperAdmin" };

            if (!validRoles.Contains(model.RoleName))
            {
                return new AssignRoleResponseDto
                {
                    IsSuccess = false,
                    Message = "Role not found."
                };
            }

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
            {
                return new AssignRoleResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            return new AssignRoleResponseDto
            {
                IsSuccess = result.Succeeded,
                Message = result.Succeeded
                    ? "Role assigned successfully."
                    : result.Errors.FirstOrDefault()?.Description ?? "Failed to assign role."
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AssignRoleToUserAsync: {ex.Message}");
            throw;
        }
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("FirstName", user.FirstName ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtKey = _configuration["Jwt:Key"]
                     ?? throw new InvalidOperationException("JWT key is not configured.");

        var issuer = _configuration["Jwt:Issuer"] ?? "BackendProject";
        var audience = _configuration["Jwt:Audience"] ?? "BackendProject";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}