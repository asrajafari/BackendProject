using System.ComponentModel.DataAnnotations;

namespace BackendProject.Application.DTOs.Products.Requests;

public class GetProductRequestDto
{
    [Required]
    public int Id { get; set; }
}