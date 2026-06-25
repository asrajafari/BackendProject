namespace BackendProject.DTOs;

public class RegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}