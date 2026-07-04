using System.ComponentModel.DataAnnotations;

namespace BackendProject.Application.DTOs.Products.Requests;

public class DeleteProductRequestDto
{
    [Required]
    public int Id { get; set; }
}