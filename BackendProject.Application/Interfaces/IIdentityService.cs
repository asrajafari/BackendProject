using BackendProject.DTOs;

public interface IIdentityService
{
    Task<string> RegisterUserAsync(RegisterDto model);
    Task<LoginResponseDto> LoginUserAsync(LoginDto model); 
    Task<bool> AssignRoleToUserAsync(Guid userId, string roleName);
}