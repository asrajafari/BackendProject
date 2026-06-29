namespace BackendProject.Application.DTOs.Auth;

public class RegisterResponse
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;
}