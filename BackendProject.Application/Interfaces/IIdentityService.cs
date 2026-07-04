using BackendProject.Application.DTOs.Auth;

namespace BackendProject.Application.Interfaces;

public interface IIdentityService
{
    Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto model);

    Task<LoginResponseDto> LoginUserAsync(LoginRequestDto model);

    Task<AssignRoleResponseDto> AssignRoleToUserAsync(AssignRoleRequestDto model);
}