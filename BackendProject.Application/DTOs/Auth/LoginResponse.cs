namespace BackendProject.Application.DTOs.Auth;

public class LoginResponse
{
    public bool IsSuccess { get; set; }

    public string Token { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}