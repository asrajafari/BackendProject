using Microsoft.AspNetCore.Identity;
using BackendProject.Domain.Entities;

namespace BackendProject.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Wallet? Wallet { get; set; }

    public ApplicationUser()
    {
    }
}