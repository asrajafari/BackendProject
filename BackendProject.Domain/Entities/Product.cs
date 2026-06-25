using System.ComponentModel.DataAnnotations;
using BackendProject.Entities;

namespace BackendProject.Entities;

public class Product : BaseEntity<int>
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    public decimal Price { get; set; }
}