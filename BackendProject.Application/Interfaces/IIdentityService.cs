using BackendProject.Application.DTOs.Auth;

namespace BackendProject.Application.Interfaces;

public interface IIdentityService
{
    Task<RegisterResponse> RegisterUserAsync(RegisterRequest model);

    Task<LoginResponse> LoginUserAsync(LoginRequest model);

    Task<AssignRoleResponse> AssignRoleToUserAsync(AssignRoleRequest model);
}