using System.ComponentModel.DataAnnotations;

namespace BackendProject.Application.DTOs.Products.Requests;

public class DeleteProductRequest
{
    [Required]
    public int Id { get; set; }
}