using System.ComponentModel.DataAnnotations;

namespace BackendProject.Application.DTOs.Auth;

public class AssignRoleRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string RoleName { get; set; } = string.Empty;
}