using System.ComponentModel.DataAnnotations;

namespace BackendProject.Application.DTOs.Products.Requests;

public class GetProductRequest
{
    [Required]
    public int Id { get; set; }
}